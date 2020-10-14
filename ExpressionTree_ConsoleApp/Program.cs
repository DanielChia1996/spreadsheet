using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            int result = 0;
            string expString = "5+5";
            string varName = String.Empty;
            double varValue = 0.0;
            double evalResult = 0.0;

            CptS321.ExpressionTree treeObj = new CptS321.ExpressionTree(expString);

            do
            {
                Console.WriteLine("Menu (current expression= '{0}')", expString);
                Console.WriteLine("1 = Enter a new expression");
                Console.WriteLine("2 = Set a variable value");
                Console.WriteLine("3 = Evaluate tree");
                Console.WriteLine("4 = Quit");

                string input = String.Empty;

                try
                {
                    input = Console.ReadLine();
                    result = Int32.Parse(input);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Input, enter an integer!");
                    Console.WriteLine();
                }

                if (result == 1) // prompt user to input new expression
                {
                    Console.WriteLine("Enter new expression: ");
                    expString = Console.ReadLine();

                    treeObj = new CptS321.ExpressionTree(expString);

                    Console.WriteLine("You entered expression:'{0}'", expString);
                    Console.WriteLine();
                }
                else if (result == 2) // prompt user for a varible value
                {
                    Console.WriteLine("Enter variable name: ");
                    varName = Console.ReadLine();

                    Console.WriteLine("Enter variable value: ");
                    varValue = Convert.ToDouble(Console.ReadLine());

                    treeObj.SetVar(varName, varValue);
                }
                else if (result == 3) // call evaluate method in ExpressionTree class
                {
                    evalResult = treeObj.Eval();
                    Console.WriteLine("Result of Expression Tree = {0}", evalResult);
                }

            } while (result != 4);


            // wait for user enter before closing program
            Console.WriteLine("Hit any key to exit...");
            Console.ReadLine();
        }
    }
}
