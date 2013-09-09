using System;
using NUnit.Framework;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class ListTests
	{
		[Test]
		public void ListDecimalFirstLess()
		{
			var test = new ParserTester("-0.01 [10, 20, 30, 40, 50]", null, 9.9, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListDecimalFirstMore()
		{
			var test = new ParserTester("0.01 [10, 20, 30, 40, 50]", null, 10.1, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListDecimalLastLess()
		{
			var test = new ParserTester("3.99 [10, 20, 30, 40, 50]", null, 49.9, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListDecimalLastMore()
		{
			var test = new ParserTester("4.01 [10, 20, 30, 40, 50]", null, 50.1, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListDecimalMiddle()
		{
			var test = new ParserTester("1.5 [10, 20, 30, 40, 50]", null, 25, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListIntegerFirst()
		{
			var test = new ParserTester("0 [10, 20, 30, 40, 50]", null, 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListIntegerLast()
		{
			var test = new ParserTester("4 [10, 20, 30, 40, 50]", null, 50);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListIntegerMiddle()
		{
			var test = new ParserTester("1 [10, 20, 30]", null, 20);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListVariableInside()
		{
			var test = new ParserTester("i [a, b, c]", new Var("i", 0.5).And("a", 10).And("b", 20).And("c", 30).Get, 15);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ListVariableOutside()
		{
			var test = new ParserTester("i [a, b, c]", new Var("i", -5.5).And("a", 10).And("b", 20).And("c", 30).Get, -45);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
