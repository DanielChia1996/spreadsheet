namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RestoreBackgroundColor : IUndoRedoCommand
    {
        private Cell cell;
        private uint color;

        public RestoreBackgroundColor(Cell newCell, uint newColor)
        {
            this.cell = newCell;
            this.color = newColor;
        }

        public IUndoRedoCommand Execute(Spreadsheet spreadsheet)
        {
            string cellName = this.cell.ColIndex.ToString() + this.cell.RowIndex.ToString();
            uint currentColor = this.cell.BackgroundColor;

            this.cell.BackgroundColor = this.color;

            return new RestoreBackgroundColor(this.cell, currentColor);
        }
    }
}
