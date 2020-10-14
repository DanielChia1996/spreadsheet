namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for tree nodes, factory method implementation.
    /// </summary>
    internal abstract class TreeNodeFactory
    {
        /// <summary>
        /// Base method to be overridden for nodes.
        /// </summary>
        /// <param name="exp"> input expression -- the numbers, operators or cell refs.</param>
        /// <returns>.</returns>
        public abstract TreeNode FactoryMethod(string exp);
    }
}
