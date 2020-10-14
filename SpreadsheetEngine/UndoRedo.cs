namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUndoRedoCommand
    {
        IUndoRedoCommand Execute(Spreadsheet spreadsheet);
    }

    public class UndoRedo
    {
        private Stack<UndoRedoCollection> undoStack = new Stack<UndoRedoCollection>();
        private Stack<UndoRedoCollection> redoStack = new Stack<UndoRedoCollection>();

 
        /// <summary>
        /// Gets a value indicating whether the undo stack is not empty.
        /// If the stack is not empty it means there are "undo objects" that can be popped off.
        /// Otherwise if its empty there is nothing to undo!
        /// </summary>
        public bool CanUndo
        {
            get { return this.undoStack.Count != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the redo stack is not empty.
        /// See summary of CanUndo.
        /// </summary>
        public bool CanRedo
        {
            get { return this.redoStack.Count != 0; }
        }

        /// <summary>
        /// Pushes undo object to undo stack and clears redo stack.
        /// </summary>
        /// <param name="undos">.</param>
        public void AddUndo(UndoRedoCollection undos)
        {
            this.undoStack.Push(undos);
            this.redoStack.Clear();
        }

        /// <summary>
        /// Pops the undo stack and executes the undo.
        /// </summary>
        /// <param name="sheet"> Spreadsheet object.</param>
        public void Undo(Spreadsheet sheet)
        {
            UndoRedoCollection commands = this.undoStack.Pop();

            this.redoStack.Push(commands.Restore(sheet));
        }

        /// <summary>
        /// Pops the redo stack and executes the redo.
        /// </summary>
        /// <param name="sheet"> Spreadsheet object. </param>
        public void Redo(Spreadsheet sheet)
        {
            UndoRedoCollection commands = this.redoStack.Pop();

            this.undoStack.Push(commands.Restore(sheet));
        }

        /// <summary>
        /// Method that returns top of undo stack if it is not empty.
        /// </summary>
        public string CheckUndo
        {
            get
            {
                if (this.CanUndo)
                {
                    return this.undoStack.Peek().title;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Method that returns top of redo stack if it is not empty.
        /// </summary>
        public string CheckRedo
        {
            get
            {
                if (this.CanRedo)
                {
                    return this.redoStack.Peek().title;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Method to clear both undo and redo stacks.
        /// </summary>
        public void ClearStacks()
        {
            this.redoStack.Clear();
            this.undoStack.Clear();
        }
    }

    public class UndoRedoCollection
    {
        private IUndoRedoCommand[] commandObjects;
        public string title;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollection"/> class.
        /// Default constructor.
        /// </summary>
        public UndoRedoCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollection"/> class.
        /// Constructor that accepts an array.
        /// </summary>
        /// <param name="commands">.</param>
        /// <param name="newTitle">..</param>
        public UndoRedoCollection(IUndoRedoCommand[] commands, string newTitle)
        {
            this.commandObjects = commands;
            this.title = newTitle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollection"/> class.
        /// Constructor that accepts a List.
        /// </summary>
        /// <param name="commands">.</param>
        /// <param name="newTitle">..</param>
        public UndoRedoCollection(List<IUndoRedoCommand> commands, string newTitle)
        {
            this.commandObjects = commands.ToArray();
            this.title = newTitle;
        }

        /// <summary>
        /// Method to execute an undo or redo command on the input spreadsheet.
        /// </summary>
        /// <param name="spreadsheet">.</param>
        /// <returns>UndoRedoCollection object.</returns>
        public UndoRedoCollection Restore(Spreadsheet spreadsheet)
        {
            List<IUndoRedoCommand> commandList = new List<IUndoRedoCommand>();

            foreach (IUndoRedoCommand command in this.commandObjects)
            {
                commandList.Add(command.Execute(spreadsheet));
            }

            return new UndoRedoCollection(commandList.ToArray(), this.title);
        }

    }
}
