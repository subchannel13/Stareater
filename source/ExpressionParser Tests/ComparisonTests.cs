using System;
using NUnit.Framework;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class ComparisonTests
	{
		[Test]
		public void EqualityFalse()
		{
			var test = new ParserTester("5 = 7", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void EqualityTolerance()
		{
			var test = new ParserTester("5 = 7 ~ 2", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void EqualityTrue()
		{
			var test = new ParserTester("9 = 9", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterEqual()
		{
			var test = new ParserTester("5 > 5", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterGreater()
		{
			var test = new ParserTester("9 > 8", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterLess()
		{
			var test = new ParserTester("9 > 10", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterEqualEqual()
		{
			var test = new ParserTester("5 >= 5", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterEqualGreater()
		{
			var test = new ParserTester("9 >= 8", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterEqualLess()
		{
			var test = new ParserTester("9 >= 10", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterEqualUnicodeFalse()
		{
			var test = new ParserTester("9 ≥ 10", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterEqualUnicodeTrue()
		{
			var test = new ParserTester("9 ≥ 9", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void GreaterEqualTolerance()
		{
			var test = new ParserTester("5 ≥ 3 ~ 2", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void InequalityNormalFalse()
		{
			var test = new ParserTester("5 <> 5", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void InequalityNormalTrue()
		{
			var test = new ParserTester("9 <> 7", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void InequalityTolerance()
		{
			var test = new ParserTester("5 ≠ 3 ~ 2", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void InequalityUnicodeFalse()
		{
			var test = new ParserTester("5 ≠ 5", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void InequalityUnicodeTrue()
		{
			var test = new ParserTester("9 ≠ 7", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessEqual()
		{
			var test = new ParserTester("5 < 5", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessGreater()
		{
			var test = new ParserTester("9 < 8", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessLess()
		{
			var test = new ParserTester("9 < 10", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessEqualEqual()
		{
			var test = new ParserTester("5 <= 5", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessEqualGreater()
		{
			var test = new ParserTester("9 <= 8", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessEqualLess()
		{
			var test = new ParserTester("9 <= 10", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessEqualUnicodeFalse()
		{
			var test = new ParserTester("11 ≤ 10", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessEqualUnicodeTrue()
		{
			var test = new ParserTester("9 ≤ 9", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void LessEqualTolerance()
		{
			var test = new ParserTester("5 ≤ 3 ~ 2", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
