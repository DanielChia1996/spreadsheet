namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract base class to represent one cell in the worksheet.
    /// Implemented in the CptS321 namespace.
    /// Implements the INotifyPropertyChanged interface, declared in System.ComponentModel.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        protected int rowIndex;
        protected char colIndex;
        protected string text = string.Empty;
        protected string cellValue;

        /// <summary>
        /// Declared event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="row">.</param>
        /// <param name="col">..</param>
        public Cell(int row, char col)
        {
            this.rowIndex = row;
            this.colIndex = col;
        }

        /// <summary>
        /// Gets rowIndex.
        /// </summary>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Gets colIndex.
        /// </summary>
        public char ColIndex
        {
            get { return this.colIndex; }
        }

        /// <summary>
        /// Gets or sets text property.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                // If the text is actually being changed then.
                // update the protected member variable and fire the PropertyChanged event.
                if (value != this.text)
                {
                    this.text = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// Gets cellValue.
        /// cellValue is NOT set in Cell class as it will be set ONLY IN Spreadsheet class.
        /// </summary>
        public string Cell_Value
        {
            get { return this.cellValue; }
        }

        ///// <summary>
        ///// Property-change notification method.
        ///// </summary>
        ///// <param name="e"> event. </param>
        //protected void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        /// <summary>
        /// Property-change notification method.
        /// </summary>
        /// <param name="propertyName"> name of property. </param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string GetCellName()
        {
            string cellname = this.ColIndex.ToString() + this.RowIndex.ToString();

            return cellname;
        }

        /// <summary>
        /// Background cell color is intially set to white.
        /// </summary>
        private uint backgroundcolor = 4294967295;

        /// <summary>
        /// Gets or sets cell's background color.
        /// </summary>
        public uint BackgroundColor
        {
            get
            {
                return this.backgroundcolor;
            }

            set
            {
                if (value != this.backgroundcolor)
                {
                    this.backgroundcolor = value;

                    this.OnPropertyChanged("BGColor");
                }
            }
        }

        /// <summary>
        /// Clears the cell contents and reset background color to white.
        /// </summary>
        public void Clear()
        {
            this.Text = string.Empty;

            this.backgroundcolor = 4294967295;
        }
    }
}
