﻿using System;
using NUnit.Framework;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class UnaryTests
	{
		[Test]
		public void NegateConstNegative()
		{
			var test = new ParserTester("-5", null, -5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void NegateConstPositive()
		{
			var test = new ParserTester("-(-5)", null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void NegateVarNegative()
		{
			var test = new ParserTester("-a", new Var("a", 4).Get, -4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void NegateVarPositive()
		{
			var test = new ParserTester("-a", new Var("a", -4).Get, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void NegateToBooleanConst()
		{
			var test = new ParserTester("-'5", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void NegateToBooleanVar()
		{
			var test = new ParserTester("-'x", new Var("x", -2).Get, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ToBooleanConstNegative()
		{
			var test = new ParserTester("'5", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ToBooleanConstPositive()
		{
			var test = new ParserTester("'(-5)", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ToBooleanConstZero()
		{
			var test = new ParserTester("'0", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ToBooleanVarNegative()
		{
			var test = new ParserTester("'a", new Var("a", 4).Get, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void ToBooleanVarPositive()
		{
			var test = new ParserTester("'a", new Var("a", -4).Get, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
