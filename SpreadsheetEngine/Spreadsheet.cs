namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Class to represent a spreadsheet of 2d array of cell objects.
    /// </summary>
    public class Spreadsheet
    {
        public ArrayOfCells[,] cellsArray;

        private Dictionary<string, HashSet<string>> dependencyDict;

        public UndoRedo undoRedo_ = new UndoRedo();

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="numberOfRows">total amount of rows in spreadsheet.</param>
        /// <param name="numberOfColumns">total amount of columns in spreadsheet.</param>
        public Spreadsheet(int numberOfRows, int numberOfColumns)
        {
            this.RowCount = numberOfRows;
            this.ColumnCount = numberOfColumns;

            this.cellsArray = new ArrayOfCells[numberOfRows, numberOfColumns];

            this.dependencyDict = new Dictionary<string, HashSet<string>>();

            for (int r = 0; r < numberOfRows; r++)
            {
                for (int c = 0; c < numberOfColumns; c++)
                {
                    char letter = (char)(c + 64);

                    this.cellsArray[r, c] = new ArrayOfCells(r+1, letter);

                    this.cellsArray[r, c].PropertyChanged += this.OnPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Concrete Implementation of abstract cell class.
        /// </summary>
        public class ArrayOfCells : Cell
        {
            public ArrayOfCells(int row, char col)
                : base(row, col)
            {

            }

            public void setValue(string val)
            {
                this.cellValue = val;
                this.OnPropertyChanged("Value");
            }

        }

        private int columnCount;

        public int ColumnCount
        {
            get { return this.columnCount; }
            set { this.columnCount = value; }
        }

        private int rowCount;

        public int RowCount
        {
            get { return this.rowCount; }
            set { this.rowCount = value; }
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                var tempCell = sender as ArrayOfCells;
                string cellName = tempCell.GetCellName();

                this.DeleteDependency(cellName);

                if (tempCell.Text != "" && tempCell.Text[0] == '=' && tempCell.Text.Length > 1)
                {
                    ExpressionTree tree = new ExpressionTree(tempCell.Text.Substring(1));

                    this.SetDependency(cellName, tree.GetVariables());
                }

                this.EvaluateCell(sender as Cell);
            }
            else if (e.PropertyName == "BGColor")
            {
                this.CellPropertyChanged(sender, new PropertyChangedEventArgs("BGColor"));
            }
        }

        private void EvaluateCell(Cell cell)
        {
            ArrayOfCells evalCell = cell as ArrayOfCells;

            bool error = false;

            if (string.IsNullOrWhiteSpace(evalCell.Text))
            {
                evalCell.setValue("");
                this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }
            else if (evalCell.Text.Length > 1 && evalCell.Text[0] == '=')
            {
                string text = evalCell.Text.Substring(1);
                ExpressionTree evalTree = new ExpressionTree(text);
                string[] variables = evalTree.GetVariables();

                foreach (string variableName in variables)
                {
                    if (this.GetCell(variableName) == null)
                    {
                        //HW 10 - BAD REFERENCE ERROR 
                        this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                        error = true;
                        break;
                    }

                    Cell variableCell = this.GetCell(variableName);
                    double variableValue;

                    if (string.IsNullOrEmpty(variableCell.Cell_Value))
                    {
                        evalTree.SetVar(variableName, 0);
                    }
                    else if (!double.TryParse(variableCell.Cell_Value, out variableValue))
                    {
                        evalTree.SetVar(variableName, 0);
                    }
                    else
                    {
                        evalTree.SetVar(variableName, variableValue);
                    }
                    //HW 10 - SELF REFERENCE ERROR
                    string cellToEval = evalCell.ColIndex.ToString() + evalCell.RowIndex.ToString();
                    if (variableName == cellToEval)
                    {
                        evalCell.setValue("!(Self Reference)");
                        this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));

                        error = true;
                        break;
                    }
                }
                if (error)
                {
                    return;
                }

                //CIRCULAR REFERENCES - CASE 3 (HW 10)
                //check each variable in the cell's list of variables
                // foreach (string variableName in variables)
                // {
                //     //Check for circular references
                //     if (IsCircularReference(variableName, evalCell.GetCellName()))
                //     {
                //         evalCell.setValue("!(Circular Reference!)");
                //         CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                //         error = true;
                //         break;
                //     }
                // }
                // if (error)
                // {
                //     return;
                // }

                //Now, all variables should be set and we can evaluate the formula using the expression tree
                evalCell.setValue(evalTree.Eval().ToString());
                this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }
            else
            {
                evalCell.setValue(evalCell.Text);
                this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }

            string cellName = evalCell.GetCellName();
            if (this.dependencyDict.ContainsKey(cellName))
            {
                foreach (string dependentCell in this.dependencyDict[cellName])
                {
                    this.EvaluateCell(this.GetCell(dependentCell));
                }
            }
        }

        private ArrayOfCells Convert_NametoCell(string cellName)
        {
            if (!char.IsLetter(cellName[1]))
            {
                return null;
            }

            int col = char.ToUpper(cellName[1]) - 65; // convert alphanumeric to index

            int row;
            bool isInt = int.TryParse(cellName.Substring(2), out row);
            if (!isInt || row > this.RowCount)
            {
                return null;
            }

            return this.cellsArray[row - 1, col];
        }

        /// <summary>
        /// Get a cell from the spreadsheet
        /// </summary>
        /// <param name="row">r.</param>
        /// <param name="column">c.</param>
        /// <returns>cell.</returns>
        public Cell GetCell(int row, int column)
        {
            return this.cellsArray[row, column];
        }

        public Cell GetCell(string location)
        {
            char column = location[0];
            int row;
            Cell cell;

            if (!char.IsLetter(column))
            {
                return null;
            }

            if (!int.TryParse(location.Substring(1), out row))
            {
                return null;
            }

            cell = this.GetCell(row - 1, column - 64);

            return cell;
        }

        /// <summary>
        /// Method to add dependent cell to dictionary.
        /// Called in the event handler.
        /// </summary>
        /// <param name="cellName">.</param>
        /// <param name="variables">..</param>
        private void SetDependency(string cellName, string[] variables)
        {
            foreach (string variableName in variables)
            {
                if (!this.dependencyDict.ContainsKey(variableName))
                {
                    this.dependencyDict[variableName] = new HashSet<string>();
                }

                this.dependencyDict[variableName].Add(cellName);
            }
        }

        /// <summary>
        /// Method to delete dependent cells.
        /// Called in the event handler.
        /// </summary>
        /// <param name="cellName">.</param>
        private void DeleteDependency(string cellName)
        {
            List<string> dependencyList = new List<string>();

            foreach (string key in this.dependencyDict.Keys)
            {
                if (this.dependencyDict[key].Contains(cellName))
                {
                    dependencyList.Add(key);
                }
            }

            foreach (string item in dependencyList)
            {
                HashSet<string> removeSet = this.dependencyDict[item];

                if (removeSet.Contains(cellName))
                {
                    removeSet.Remove(cellName);
                }
            }
        }

        /// <summary>
        /// Method to save the spreadsheet via an XML file format.
        /// </summary>
        /// <param name="saveFile">.</param>
        public void Save(Stream saveFile)
        {
            XmlWriter saveXML = XmlWriter.Create(saveFile);

            saveXML.WriteStartDocument();
            saveXML.WriteStartElement("Spreadsheet");

            foreach (Cell indCell in this.cellsArray)
            {
                if (indCell.Text != string.Empty | indCell.BackgroundColor != 4294967295)
                {
                    saveXML.WriteStartElement("cell");

                    string cellname = indCell.GetCellName();

                    saveXML.WriteAttributeString("name", cellname);

                    saveXML.WriteElementString("text", indCell.Text);
                    saveXML.WriteElementString("bgcolor", indCell.BackgroundColor.ToString());

                    saveXML.WriteWhitespace("\n");

                    saveXML.WriteEndElement();
                }
            }

            saveXML.WriteEndElement();
            saveXML.Close();
        }

        /// <summary>
        /// Method to load a spreadsheet that exists in XML format.
        /// </summary>
        /// <param name="loadFile">.</param>
        public void Load(Stream loadFile)
        {
            XmlDocument loadedFile = new XmlDocument();
            loadedFile.Load(loadFile);

            XmlNode sheet = loadedFile.SelectSingleNode("Spreadsheet");
            XmlNodeList cellList = sheet.SelectNodes("cell");

            foreach (XmlNode cell in cellList)
            {
                string cellname = cell.Attributes.GetNamedItem("name").Value;

                Cell editCell = this.GetCell(cellname);

                if (cell.SelectSingleNode("text").InnerText.ToString() != string.Empty)
                {
                    editCell.Text = cell.SelectSingleNode("text").InnerText.ToString();
                }

                if (cell.SelectSingleNode("bgcolor").InnerText.ToString() != "4294967295")
                {
                    uint Color;

                    if (uint.TryParse(cell.SelectSingleNode("bgcolor").InnerText, out Color))
                    {
                        editCell.BackgroundColor = Color;
                    }
                }
            }
        }


    }
}
