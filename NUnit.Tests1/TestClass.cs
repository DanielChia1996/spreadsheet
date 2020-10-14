// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using CptS321;

namespace Spreadsheet_Daniel_Chia.Tests1
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod1()
        {
            double expected = 50;

            CptS321.ExpressionTree testTree = new CptS321.ExpressionTree("A+25");
            testTree.SetVariable("A", 25);

            Assert.AreEqual(expected, testTree.Evaluate());
        }

        [Test]
        public void TestMethod2()
        {
            double expected = 50;

            CptS321.ExpressionTree testTree = new CptS321.ExpressionTree("25+25");

            Assert.AreEqual(expected, testTree.Evaluate());
        }

        [Test]
        public void TestMethod3()
        {
            double expected = 50;

            CptS321.ExpressionTree testTree = new CptS321.ExpressionTree("A*25");
            testTree.SetVariable("A", 2);

            Assert.AreEqual(expected, testTree.Evaluate());
        }

        [Test]
        public void TestMethod4()
        {
            double expected = 50;

            CptS321.ExpressionTree testTree = new CptS321.ExpressionTree("A-25");
            testTree.SetVariable("A", 75);

            Assert.AreEqual(expected, testTree.Evaluate());
        }
    }
}
