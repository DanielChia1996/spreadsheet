namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RestoreText : IUndoRedoCommand
    {
        private Cell cell;
        private string text;

        public RestoreText(Cell c, string t)
        {
            this.cell = c;
            this.text = t;
        }

        /// <summary>
        /// Restores a cell's previous text.
        /// </summary>
        /// <param name="spreadsheet"></param>
        /// <returns>.</returns>
        public IUndoRedoCommand Execute(Spreadsheet spreadsheet)
        {
            string cellName = this.cell.ColIndex.ToString() + this.cell.RowIndex.ToString();
            Cell cell = spreadsheet.GetCell(cellName);

            string currentText = cell.Text;

            cell.Text = this.text;

            return new RestoreText(cell, currentText);
        }
    }
}
