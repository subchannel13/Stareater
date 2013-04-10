using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stareater.AppData.Expressions;
using System.IO;

namespace ExpressionParser_Tests
{
	[TestClass]
	public class TermTests
	{

		[TestMethod]
		public void InfinityConstant()
		{
			var test = new ParserTester("Inf", null, double.PositiveInfinity);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void IntegerConstant()
		{
			double input = 2;

			var test = new ParserTester(input.ToString(), null, input);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void DecimalConstant()
		{
			double input = 0.6;

			var test = new ParserTester(input.ToString(), null, input, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SciBigIntegerConstant()
		{
			double input = 8e9;

			var test = new ParserTester(input.ToString(), null, input, 1e-6);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SciBigDecimalConstant()
		{
			double input = 8.6e9;

			var test = new ParserTester(input.ToString(), null, input, 1e-6);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SciSmallIntegerConstant()
		{
			double input = 4e-6;

			var test = new ParserTester(input.ToString(), null, input, 1e-12);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SciSmallDecimalConstant()
		{
			double input = 3.14e-6;

			var test = new ParserTester(input.ToString(), null, input, 1e-12);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void Variable()
		{
			string varName = "lvl";
			double varValue = 3.14;

			var test = new ParserTester(varName, new Dictionary<string, double>()
			{
				{varName, varValue}
			}, varValue);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
