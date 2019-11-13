using NUnit.Framework;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class ExponentiationTest
	{
		[Test]
		public void ExponentiationConstDecimal()
		{
			var test = new ParserTester("25^0.5", null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ExponentiationConstNegativeBase()
		{
			var test = new ParserTester("(-3)^3", null, -27);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ExponentiationConstNegativePower()
		{
			var test = new ParserTester("2^-3", null, 0.125);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ExponentiationConstPositive()
		{
			var test = new ParserTester("3^4", null, 81);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ExponentiationVarBoth()
		{
			var test = new ParserTester("x^y", new Var("x", 2).And("y", 3), 8);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void ExponentiationVarLeft()
		{
			var test = new ParserTester("x^2", new Var("x", 5), 25);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void ExponentiationVarRight()
		{
			var test = new ParserTester("2^x", new Var("x", 5), 32);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void SegmentExponentiationConst()
		{
			var test = new ParserTester("4^0.5^-1", null, 16);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void SegmentExponentiationVar()
		{
			var test = new ParserTester("a^b^a", new Var("a", 2).And("b", 3), 512);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
