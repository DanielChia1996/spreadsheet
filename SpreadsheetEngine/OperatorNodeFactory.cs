namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for operator node, factory method implementation.
    /// </summary>
    internal abstract class OperatorNodeFactory
    {
        /// <summary>
        /// Base method for operator nodes.
        /// </summary>
        /// <param name="op">.</param>
        /// <returns>..</returns>
        public abstract OperatorNode FactoryMethod(string op);
    }
}
