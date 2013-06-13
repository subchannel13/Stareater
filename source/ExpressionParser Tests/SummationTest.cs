using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParser_Tests
{
	[TestClass]
	public class SummationTest
	{
		[TestMethod]
		public void AdditionConst()
		{
			var test = new ParserTester("2+2", null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void AdditionVarBoth()
		{
			var test = new ParserTester("x+y", new Var("x", 2).And("y", 3), 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void AdditionVarLeft()
		{
			var test = new ParserTester("x+2", new Var("x", 2), 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void AdditionVarRight()
		{
			var test = new ParserTester("2+x", new Var("x", 3), 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SubstractionConst()
		{
			var test = new ParserTester("2-5", null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SubstractionVarBoth()
		{
			var test = new ParserTester("x-y", new Var("x", 2).And("y", 5), -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SubstractionVarLeft()
		{
			var test = new ParserTester("x-5", new Var("x", 2), -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SubstractionVarRight()
		{
			var test = new ParserTester("2-x", new Var("x", 5), -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SegmentSummationConst()
		{
			var test = new ParserTester("1-2+3+6-1+9", null, 16);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SegmentSummationVar()
		{
			var test = new ParserTester("a-2+b+6-a+9", new Var("a", 1).And("b", 3), 16);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
