using System;
using NUnit.Framework;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class LogicalTests
	{
		[Test]
		public void ConjunctionNormalFalseFalse()
		{
			var test = new ParserTester("-5 & -7", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionNormalFalseTrue()
		{
			var test = new ParserTester("-5 & 2", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionNormalTrueFalse()
		{
			var test = new ParserTester("2 & -3", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionNormalTrueTrue()
		{
			var test = new ParserTester("0 & 1", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionSequenceFalse()
		{
			var test = new ParserTester("2 ∧ -3 ∧ 0 ∧ -1", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionSequenceTrue()
		{
			var test = new ParserTester("2 ∧ 3 ∧ 0 ∧ 1", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionUnicodeBinary()
		{
			var test = new ParserTester("2 ∧ -3", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionUnicodeNary()
		{
			var test = new ParserTester("2 ⋀ -3", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionNormalFalseFalse()
		{
			var test = new ParserTester("-5 | -7", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionNormalFalseTrue()
		{
			var test = new ParserTester("-5 | 2", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionNormalTrueFalse()
		{
			var test = new ParserTester("2 | -3", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionNormalTrueTrue()
		{
			var test = new ParserTester("0 | 1", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionSequenceFalse()
		{
			var test = new ParserTester("-2 ∨ -3 ∨ -0.1 ∨ -1", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionSequenceTrue()
		{
			var test = new ParserTester("2 ∨ -3 ∨ -0.1 ∨ -1", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionUnicodeBinary()
		{
			var test = new ParserTester("2 ∨ -3", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionUnicodeNary()
		{
			var test = new ParserTester("2 ⋁ -3", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void XorNormalFalseFalse()
		{
			var test = new ParserTester("-5 @ -7", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void XorNormalFalseTrue()
		{
			var test = new ParserTester("-5 @ 2", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void XorNormalTrueFalse()
		{
			var test = new ParserTester("2 @ -3", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void XorNormalTrueTrue()
		{
			var test = new ParserTester("0 @ 1", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void XorSequenceFalse()
		{
			var test = new ParserTester("-2 ⊕ -3 ⊕ -0.1 ⊕ -1", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void XorSequenceTrue()
		{
			var test = new ParserTester("2 ⊕ -3 ⊕ -0.1 ⊕ -1", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
			
		[Test]
		public void XorUnicode()
		{
			var test = new ParserTester("2 ⊕ -3", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
