using NUnit.Framework;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class LogicalTests
	{
		[Test]
		public void ConjunctionDisjunction()
		{
			var test = new ParserTester("1 & 1 | -1", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}

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
		public void ConjunctionSimplificationConstants()
		{
			var test = new ParserTester("5 & 7 & x", new Var("x", 1).Get, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionSimplificationLazy()
		{
			var test = new ParserTester("5 & -7 & x", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ConjunctionVar()
		{
			var test = new ParserTester("a ∧ 1", new Var("a", -1).Get, -1);
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
		public void DisjunctionConjunction()
		{
			var test = new ParserTester("-1 | -1 & 1", null, -1);
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
		public void DisjunctionSimplificationConstant()
		{
			var test = new ParserTester("-2 | -1 | x", new Var("x", 1).Get, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DisjunctionSimplificationLazy()
		{
			var test = new ParserTester("0 | 1 | x", null, 1);
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
		public void DisjunctionVar()
		{
			var test = new ParserTester("a ∨ 1", new Var("a", -1).Get, 1);
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
		public void XorSimplificationLazy()
		{
			var test = new ParserTester("1 @ 1 @ -1 @ x", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void XorSimplificationConstantsNoTruth()
		{
			var test = new ParserTester("-1 @ -1 @ -1 @ x", new Var("x", 1).Get, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void XorSimplificationConstantsOneTruth()
		{
			var test = new ParserTester("1 @ -1 @ -1 @ x", new Var("x", 1).Get, -1);
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
		public void XorVar()
		{
			var test = new ParserTester("a ⊕ 1", new Var("a", -1).Get, 1);
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
