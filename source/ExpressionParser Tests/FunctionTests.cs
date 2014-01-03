/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 22.7.2013.
 * Time: 13:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class FunctionTests
	{
		[Test]
		public void FunctionAbsNegative()
		{
			var test = new ParserTester("abs(-2.1)", null, 2.1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionAbsPositive()
		{
			var test = new ParserTester("abs(5.2)", null, 5.2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionAbsVar()
		{
			var test = new ParserTester("abs(x)", new Var("x", -1).Get, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionCeilInteger()
		{
			var test = new ParserTester("Ceil(4)", null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionCeilNegative()
		{
			var test = new ParserTester("Ceil(-2.1)", null, -2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionCeilPositive()
		{
			var test = new ParserTester("Ceil(5.2)", null, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionCeilVar()
		{
			var test = new ParserTester("Ceil(x)", new Var("x", 5.2).Get, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionFloorInteger()
		{
			var test = new ParserTester("floor(4)", null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionFloorNegative()
		{
			var test = new ParserTester("floor(-2.1)", null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionFloorPositive()
		{
			var test = new ParserTester("floor(5.2)", null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionFloorVar()
		{
			var test = new ParserTester("floor(x)", new Var("x", 5.2).Get, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteElse()
		{
			var test = new ParserTester("if(-1, 10, 20)", null, 20);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteElseVar()
		{
			var test = new ParserTester("if(c, x, y)", new Var("c", -1).And("x", 10).And("y", 20).Get, 20);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteThen()
		{
			var test = new ParserTester("if(1, 10, 20)", null, 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteThenVar()
		{
			var test = new ParserTester("if(c, x, y)", new Var("c", 1).And("x", 10).And("y", 20).Get, 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionIteZero()
		{
			var test = new ParserTester("if(0, 10, 20)", null, 10);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionLimitGreater()
		{
			var test = new ParserTester("limit(10, 1, 3)", null, 3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionLimitInside()
		{
			var test = new ParserTester("limit(2, 1, 3)", null, 2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionLimitLower()
		{
			var test = new ParserTester("limit(-2, 1, 3)", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionLimitVar()
		{
			var test = new ParserTester("limit(x, a, b)", new Var("x", 10).And("a", 1).And("b", 3).Get, 3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMax2ParamsConst()
		{
			var test = new ParserTester("max(2, 5)", null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMax2ParamsVar()
		{
			var test = new ParserTester("max(x, y)", new Var("x", 3).And("y", 5).Get, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMax5Params()
		{
			var test = new ParserTester("max(-2, 10.1, -3, 5, 1)", null, 10.1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMin2Params()
		{
			var test = new ParserTester("min(2, 5)", null, 2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMin2ParamsVar()
		{
			var test = new ParserTester("min(x, y)", new Var("x", 3).And("y", 5).Get, 3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionMin5Params()
		{
			var test = new ParserTester("min(-2, 2, -3, 5, 1.5)", null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundInteger()
		{
			var test = new ParserTester("round(4)", null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundNegativeDown()
		{
			var test = new ParserTester("round(-2.7)", null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundNegativeMidpointEven()
		{
			var test = new ParserTester("round(-2.5)", null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundNegativeMidpointOdd()
		{
			var test = new ParserTester("round(-3.5)", null, -4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundNegativeUp()
		{
			var test = new ParserTester("round(-2.1)", null, -2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundPositiveDown()
		{
			var test = new ParserTester("round(5.2)", null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundPositiveMidpointEven()
		{
			var test = new ParserTester("round(4.5)", null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundPositiveMidpointOdd()
		{
			var test = new ParserTester("round(5.5)", null, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundPositiveUp()
		{
			var test = new ParserTester("round(5.7)", null, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionRoundVar()
		{
			var test = new ParserTester("round(x)", new Var("x", 5.7).Get, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionSngNegative()
		{
			var test = new ParserTester("sgn(-4)", null, -1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionSngPositive()
		{
			var test = new ParserTester("sgn(2)", null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionSngVar()
		{
			var test = new ParserTester("sgn(x)", new Var("x", 5.7).Get, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionSngZero()
		{
			var test = new ParserTester("sgn(0)", null, 0);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionTruncInteger()
		{
			var test = new ParserTester("Trunc(4)", null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionTruncNegative()
		{
			var test = new ParserTester("Trunc(-2.1)", null, -2);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionTruncPositive()
		{
			var test = new ParserTester("Trunc(5.2)", null, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void FunctionTruncVar()
		{
			var test = new ParserTester("Trunc(x)", new Var("x", 5.7).Get, 5);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
