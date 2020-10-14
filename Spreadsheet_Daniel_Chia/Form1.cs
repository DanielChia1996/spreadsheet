namespace Spreadsheet_Daniel_Chia
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using CptS321;
    using System.IO;

    public partial class Form1 : Form
    {
        private Spreadsheet Sheet = new Spreadsheet(50, 26);

        public UndoRedo undoRedo = new UndoRedo();

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();

            //this.Sheet.CellPropertyChanged += this.Sheet_CellPropertyChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Columns.Clear();

            this.dataGridView1.ColumnCount = 26;

            char tempChar = 'A';
            int columnIndex = 0;
            for (tempChar = 'A'; tempChar <= 90; tempChar++)
            {
                this.dataGridView1.Columns[columnIndex].Name = tempChar.ToString();
                columnIndex++;
            }

            for (int rowIndex = 1; rowIndex <= 50; rowIndex++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[rowIndex - 1].HeaderCell.Value = rowIndex.ToString();
            }

            this.Sheet.CellPropertyChanged += this.Sheet_CellPropertyChanged;

            // Subscribe to cellBeginEdit and cellEndEdit
            this.dataGridView1.CellBeginEdit += this.dataGridView1_CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.dataGridView1_CellEndEdit;
        }

        /// <summary>
        /// This is a event to fire when sheet is created.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sheet_CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //var cell = sender as Cell;

            //if (e.PropertyName == "Value")
            //{
            //    this.dataGridView1[cell.ColIndex, cell.RowIndex].Value = cell.Cell_Value;
            //}
            //else if (e.PropertyName == "Text")
            //{
            //    this.dataGridView1[cell.ColIndex, cell.RowIndex].Value = cell.Text;
            //}

            Cell updatedCell = sender as Cell;
            int cellCol = (int)(updatedCell.ColIndex - 65);

            if (e.PropertyName == "Value" && updatedCell != null)
            {
                //this.dataGridView1.Rows[updatedCell.RowIndex - 1].Cells[updatedCell.ColIndex].Value = updatedCell.Cell_Value;
                this.dataGridView1.Rows[updatedCell.RowIndex - 1].Cells[cellCol].Value = updatedCell.Cell_Value;
            }
            if (e.PropertyName == "BGColor" && updatedCell != null)
            {
                this.dataGridView1.Rows[updatedCell.RowIndex - 1].Cells[cellCol + 1].Style.BackColor = Color.FromArgb((int)updatedCell.BackgroundColor);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DemoButton_Click(object sender, EventArgs e)
        {
            //Random rnd = new Random();

            ////random tests
            //for (int i = 0; i < 50; i++)
            //{
            //    this.Sheet.GetCell(rnd.Next(0, 49), rnd.Next(0, 25)).Text = "foobar";
            //}

            ////test the B column
            //for (int i = 0; i < 50; i++)
            //{
            //    this.Sheet.GetCell(i, 1).Text = $"THIS IS CELL B{i + 1}";
            //}


            ////test the B column to A column 
            //for (int i = 0; i < 50; i++)
            //{
            //    this.Sheet.GetCell(i, 0).Text = $"=B{i + 1}";
            //}
        }

        /// <summary>
        /// Event Handler for cellBeginEdit.
        /// When cell is being edited, display cell's text before its evaluated.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e">.</param>
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string msg = String.Format("Editing Cell at ({0}, {1})", e.ColumnIndex, e.RowIndex);
            this.Text = msg;

            int cellEditRow = e.RowIndex;
            int cellEditCol = e.ColumnIndex;

            Cell cellBeingEdited = this.Sheet.GetCell(cellEditRow, cellEditCol + 1);

            this.dataGridView1.Rows[cellEditRow].Cells[cellEditCol].Value = cellBeingEdited.Text;
        }

        /// <summary>
        /// Event Handler for cellEndEdit.
        /// After cell has been edited, record change in undo/redo stack.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e">.</param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int cellEditRow = e.RowIndex;
            int cellEditCol = e.ColumnIndex;

            IUndoRedoCommand[] undoHolder = new IUndoRedoCommand[1];
            string textHolder;

            Cell cellAfterEdit = this.Sheet.GetCell(cellEditRow, cellEditCol + 1);

            if (this.dataGridView1.Rows[cellEditRow].Cells[cellEditCol].Value == null)
            {
                textHolder = " ";
            }
            else
            {
                textHolder = this.dataGridView1.Rows[cellEditRow].Cells[cellEditCol].Value.ToString();
            }

            undoHolder[0] = new RestoreText(cellAfterEdit, cellAfterEdit.Text);
            cellAfterEdit.Text = textHolder;
            this.undoRedo.AddUndo(new UndoRedoCollection(undoHolder, "Cell Text Change"));

            this.dataGridView1.Rows[cellEditRow].Cells[cellEditCol].Value = cellAfterEdit.Cell_Value;
        }

        private void undocellcolor_Click(object sender, EventArgs e)
        {
            this.undoRedo.Undo(this.Sheet);
        }

        private void redocelltext_Click(object sender, EventArgs e)
        {
            this.undoRedo.Redo(this.Sheet);
        }

        private void cellcolor_Click(object sender, EventArgs e)
        {
            uint chosenColor = 0;
            ColorDialog colorDialog = new ColorDialog();
            List<IUndoRedoCommand> undo = new List<IUndoRedoCommand>();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                chosenColor = (uint)colorDialog.Color.ToArgb();

                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    Cell spreadsheetCell = this.Sheet.GetCell(cell.RowIndex, cell.ColumnIndex);

                    undo.Add(new RestoreBackgroundColor(spreadsheetCell, spreadsheetCell.BackgroundColor));

                    spreadsheetCell.BackgroundColor = chosenColor;
                }

                this.undoRedo.AddUndo(new UndoRedoCollection(undo, "Cell Background Color Change."));
            }
        }

        /// <summary>
        /// Save spreadsheet button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSpreadsheet_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream outfile = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);

                this.Sheet.Save(outfile);

                outfile.Dispose();
            }
        }

        /// <summary>
        /// Load Spreadsheet button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadSpreadsheet_Click(object sender, EventArgs e)
        {
            var loadFileDialog = new OpenFileDialog();

            if (loadFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.eraseSheet();

                Stream infile = new FileStream(loadFileDialog.FileName, FileMode.Open, FileAccess.Read);

                this.Sheet.Load(infile);

                infile.Dispose();

                this.undoRedo.ClearStacks();
            }
        }

        public void eraseSheet()
        {
            for (int r = 0; r < 50; r++)
            {
                for (int c = 0; c < 26; c++)
                {
                    if (this.Sheet.cellsArray[r, c].Text != string.Empty || this.Sheet.cellsArray[r, c].Cell_Value != string.Empty || this.Sheet.cellsArray[r, c].BackgroundColor != 4294967295)
                    {
                        this.Sheet.cellsArray[r, c].Clear();
                    }
                }
            }
        }
    }
}
