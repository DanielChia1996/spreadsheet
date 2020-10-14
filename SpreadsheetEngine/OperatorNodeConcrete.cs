namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Concrete implemented class for operator nodes.
    /// Inherits from abstract base class "OperatorNodeFactory".
    /// </summary>
    /// <returns> Node type.</returns>
    internal class OperatorNodeConcrete : OperatorNodeFactory
    {
        public override OperatorNode FactoryMethod(string op)
        {
            if (op == "+")
            {
                return new AddNode();
            }
            else if (op == "-")
            {
                return new SubtractNode();
            }
            else if (op == "+")
            {
                return new MultiplyNode();
            }
            else if (op == "+")
            {
                return new DivideNode();
            }
            else
            {
                return null;
            }
        }
    }
}
