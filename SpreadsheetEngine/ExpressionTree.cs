namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    ////////////////////////////////////////////////////////// -- nodes -- /////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Base class for all node types.
    /// Inherited by:
    /// 1. Value node
    /// 2. Operator node
    /// 3. cell reference node.
    /// </summary>
    internal abstract class TreeNode
    {
        public abstract double Eval();
    }

    internal class ValueNode : TreeNode
    {
        private int intValue = 0;
        private double doubleValue = 0.0;

        public ValueNode(int val)
        {
            this.intValue = val;
        }

        public ValueNode(double val)
        {
            this.doubleValue = val;
        }

        public override double Eval()
        {
            // Return the value that was set in the constructor
            if (this.intValue != 0)
            {
                return this.intValue;
            }
            else
            {
                return this.doubleValue;
            }
        }
    }

    internal abstract class OperatorNode : TreeNode
    {
        public override abstract double Eval();
    }

    internal abstract class BinaryOperatorNode : OperatorNode
    {
        public TreeNode left;
        public TreeNode right;

        public BinaryOperatorNode()
        {
            this.left = null;
            this.right = null;
        }

        public override abstract double Eval();
    }

    internal class AddNode : BinaryOperatorNode
    {
        public override double Eval()
        {
            return this.left.Eval() + this.right.Eval();
        }
    }

    internal class SubtractNode : BinaryOperatorNode
    {
        public override double Eval()
        {
            return this.left.Eval() - this.right.Eval();
        }
    }

    internal class MultiplyNode : BinaryOperatorNode
    {
        public override double Eval()
        {
            return this.left.Eval() * this.right.Eval();
        }
    }

    internal class DivideNode : BinaryOperatorNode
    {
        public override double Eval()
        {
            return this.left.Eval() / this.right.Eval();
        }
    }

    internal class CellReferenceNode : TreeNode
    {
        private string variableName = string.Empty;
        private double variableValue = double.NaN;

        public CellReferenceNode(string varN)
        {
            this.variableName = varN;
        }

        /// <summary>
        /// Gets variable name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.variableName;
            }
        }

        /// <summary>
        /// Sets variable value.
        /// </summary>
        public double Value
        {
            set
            {
                this.variableValue = value;
            }
        }

        /// <summary>
        /// Evaluate only if value has been set.
        /// </summary>
        public override double Eval()
        {
            if (this.variableValue != double.NaN)
            {
                return this.variableValue;
            }
            else
            {
                throw new NullReferenceException(string.Format("Variable {0}'s value is unknown", this.variableName));
            }
        }
    }
    ////////////////////////////////////////////////////////// -- nodes -- /////////////////////////////////////////////////////////////////////

    public class ExpressionTree
    {
        // Declare object of ExpressionTreeNode to hold root of tree
        private TreeNode root;

        // Private string to store expression entered by user
        private string expressionHolder;

        // Private dictionary to hold references to other cells.
        // HashSet allows for indentical cell references in the same expression
        private Dictionary<string, HashSet<CellReferenceNode>> _variable_dict;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expressionInput">.</param>
        public ExpressionTree(string expressionInput)
        {
            // Intiallize dictionary to store cell references.
            this._variable_dict = new Dictionary<string, HashSet<CellReferenceNode>>();

            // set expression input to private string expressionHolder
            this.expressionHolder = expressionInput;

            // Parse expression -- NEW METHOD
            string result = this.InfixToPostfix(this.expressionHolder); 
            //var result = temp.Select(c => c.ToString()).ToList();  // List<string>
            //this.postfixTokens = result;

            // Construct Tree
            this.root = this.ConstructTree(result);
        }

        private TreeNode ConstructTree(string expression)
        {
            // Step 1: Tokenize once again.
            var list = Tokenize(expression);

            // Step 2: Walk through tokens and build tree using a stack
            Stack<TreeNode> stack = new Stack<TreeNode>();
            TreeNodeFactory treeNodeFactory = new TreeNodeConcrete();
            foreach (string tok in list)
            {
                TreeNode tree = treeNodeFactory.FactoryMethod(tok);
                switch (tree)
                {
                    case OperatorNode opnode:
                        switch (opnode)
                        {
                            case BinaryOperatorNode bopnode:
                                BinaryOperatorNode binaryOperator = bopnode as BinaryOperatorNode;
                                TreeNode right = stack.Pop(), left = stack.Pop();
                                binaryOperator.left = left; binaryOperator.right = right;
                                stack.Push(binaryOperator);
                                break;
                            default:
                                break;
                        }

                        break;
                    case CellReferenceNode crnode:
                        CellReferenceNode cellReferenceNode = tree as CellReferenceNode;
                        if (!_variable_dict.ContainsKey(cellReferenceNode.Name))
                        {
                            _variable_dict.Add(cellReferenceNode.Name, new HashSet<CellReferenceNode>());
                        }

                        _variable_dict[cellReferenceNode.Name].Add(cellReferenceNode);
                        stack.Push(tree);
                        break;
                    case ValueNode vnode:
                        stack.Push(tree);
                        break;
                    default:
                        break;
                }
            }

            return stack.Pop();
        }


        /// <summary>
        /// Add variable to internal dictionary to keep track of cell references and their values
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public void SetVar(string varName, double varValue)
        {
            if (this._variable_dict.ContainsKey(varName))
            {
                foreach (CellReferenceNode node in this._variable_dict[varName])    // for each identical cell reference, set its value
                {
                    node.Value = varValue;
                }
            }
            else
            {
                throw new KeyNotFoundException(string.Format("{0} is not a variable in the expression", varName));
            }
        }

        /// <summary>
        /// Tokenize the expression into a list of strings.
        /// </summary>
        /// <returns> List. </returns>
        private List<string> Tokenize(string expression)
        {
            // This pattern will match:
            // decimal numbers (first part),
            // cell label (second part),
            // the operators: +-*/() (third part)
            string @pattern = @"[\d]+\.?[\d]*|[A-Za-z]+[0-9]+|[-/\+\*\(\)]";

            Regex r = new Regex(@pattern);
            MatchCollection matchList = Regex.Matches(expression, @pattern);

            return matchList.Cast<Match>().Select(match => match.Value).ToList();
        }

        /// <summary>
        /// Parses expression using Shunting Yard Algorithm.
        /// </summary>
        /// <returns> Nothing. </returns>
        private string InfixToPostfix(string expression)
        {
            HashSet<char> operators = new HashSet<char>(new char[] { '+', '-', '*', '/' });
            Dictionary<char, int> precedence = new Dictionary<char, int>
            {
                ['('] = 0,
                ['+'] = 1,
                ['-'] = 1,
                ['*'] = 2,
                ['/'] = 2,
                [')'] = 10,
            };

            var list = this.Tokenize(expression);

            Queue<string> output_list = new Queue<string>(list.Capacity);
            Stack<char> opStack = new Stack<char>();

            foreach (string tok in list)
            {
                if (int.TryParse(tok, out int int_result) || double.TryParse(tok, out double dec_result)
                    || Regex.Match(tok, @"[A-Za-z]+[0-9]+").Success)
                {
                    output_list.Enqueue(tok);
                }
                else
                {
                    if (operators.Contains(tok[0]))
                    {
                        while (opStack.Count != 0 && precedence[opStack.Peek()] > precedence[tok[0]])
                        {
                            output_list.Enqueue(opStack.Pop().ToString());
                        }

                        opStack.Push(tok[0]);
                    }
                    else if (tok.StartsWith("("))
                    {
                        opStack.Push(tok[0]);
                    }
                    else if (tok.StartsWith(")"))
                    {
                        try
                        {
                            while (opStack.Peek() != '(')
                            {
                                output_list.Enqueue(opStack.Pop().ToString());
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            throw new Exception("Mismatched Parenthesis in expression");
                        }
                        opStack.Pop();
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("{0} is not a valid formula value.", tok));
                    }
                }
            }

            // If there are still operators on the opstack, pop them to the result queue.
            while (opStack.Count > 0)
            {
                if (opStack.Peek() != '(' || opStack.Peek() != ')')
                {
                    output_list.Enqueue(opStack.Pop().ToString());
                }
                else
                {
                    throw new Exception("Mismatched Parenthesis in expression");
                }
            }

            return string.Join(" ", output_list.ToArray());
        }

        /// <summary>
        /// Recursively evaluate the expression tree from the root.
        /// Throws a null reference exception if value's (i.e. a variable's) value is unknown
        /// </summary>
        /// <returns>.</returns>
        public double Eval()
        {
            try
            {
                return this.root.Eval();
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }

        public string[] GetVariables()
        {
            return this._variable_dict.Keys.ToArray();
        }

    }
}
