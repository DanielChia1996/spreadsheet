namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Concrete implemented class for tree nodes.
    /// Inherits from abstract base class "TreeNodeFactory".
    /// </summary>
    internal class TreeNodeConcrete : TreeNodeFactory
    {
        /// <summary>
        /// Method to determine if the input expression is an operator, value or cell reference.
        /// </summary>
        /// <param name="exp"> input expression.</param>
        /// <returns> type of node corressponding to expression type.</returns>
        public override TreeNode FactoryMethod(string exp)
        {
            // Instantiate a concrete operator node object.
            OperatorNodeFactory factoryOpNode = new OperatorNodeConcrete();

            // Determine what the operator is by calling the factory method that returns type of operator node.
            OperatorNode actualOperator = factoryOpNode.FactoryMethod(exp);

            if (actualOperator != null) // operator is determined as +/-/*/-
            {
                return actualOperator;
            }
            else
            {
                bool intValue = Int32.TryParse(exp, out int int_result),
                        doubleValue = Double.TryParse(exp, out double double_result);

                if (intValue)
                {
                    return new ValueNode(int_result);
                }
                else if (doubleValue)
                {
                    return new ValueNode(double_result);
                }
                else
                {
                    return new CellReferenceNode(exp);
                }
            }
        }
    }
}
