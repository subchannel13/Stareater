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
		#region Evaluation tests

		[TestMethod]
		public void EvaluateInfinityConstant()
		{
			double input = double.PositiveInfinity;
			ExpressionParser parser = new ExpressionParser("Inf");
			parser.Parse();

			Assert.AreEqual(input, parser.ParsedFormula.Evaluate(null));
		}

		#region Normal notation
		[TestMethod]
		public void EvaluateIntegerConstant()
		{
			double input = 2;
			ExpressionParser parser = new ExpressionParser(input.ToString());
			parser.Parse();

			Assert.AreEqual(input, parser.ParsedFormula.Evaluate(null));
		}

		[TestMethod]
		public void EvaluateDecimalConstant()
		{
			double input = 0.6;
			ExpressionParser parser = new ExpressionParser(input.ToString());
			parser.Parse();

			Assert.AreEqual(input, parser.ParsedFormula.Evaluate(null), 1e-9);
		}
		#endregion

		#region Scientific notation
		[TestMethod]
		public void EvaluateSciBigIntegerConstant()
		{
			double input = 8e9;
			ExpressionParser parser = new ExpressionParser(input.ToString());
			parser.Parse();

			Assert.AreEqual(input, parser.ParsedFormula.Evaluate(null), 1e-6);
		}

		[TestMethod]
		public void EvaluateSciBigDecimalConstant()
		{
			double input = 8.6e9;
			ExpressionParser parser = new ExpressionParser(input.ToString());
			parser.Parse();

			Assert.AreEqual(input, parser.ParsedFormula.Evaluate(null), 1e-6);
		}

		[TestMethod]
		public void EvaluateSciSmallIntegerConstant()
		{
			double input = 4e-6;
			ExpressionParser parser = new ExpressionParser(input.ToString());
			parser.Parse();

			Assert.AreEqual(input, parser.ParsedFormula.Evaluate(null), 1e-12);
		}

		[TestMethod]
		public void EvaluateSciSmallDecimalConstant()
		{
			double input = 3.14e-6;
			ExpressionParser parser = new ExpressionParser(input.ToString());
			parser.Parse();

			Assert.AreEqual(input, parser.ParsedFormula.Evaluate(null), 1e-12);
		}
		#endregion

		[TestMethod]
		public void EvaluateVariable()
		{
			string varName = "lvl";
			double varValue = 3.14;
			ExpressionParser parser = new ExpressionParser(varName);
			parser.Parse();

			Assert.AreEqual(varValue, parser.ParsedFormula.Evaluate(new Dictionary<string, double>()
			{
				{varName, varValue}
			}));
		}
		#endregion

		#region Parse tests

		[TestMethod]
		public void ParseInfinityConstant()
		{
			string input = "Inf";
			ExpressionParser parser = new ExpressionParser(input);
			parser.Parse();

			Assert.AreEqual(0, parser.errors.count, parser.errors.ToString());
		}

		#region Normal notation
		[TestMethod]
		public void ParseIntegerConstant()
		{
			string input = "2";
			ExpressionParser parser = new ExpressionParser(input);
			parser.Parse();

			Assert.AreEqual(0, parser.errors.count, parser.errors.ToString());
		}

		[TestMethod]
		public void ParseDecimalConstant()
		{
			string input = "0.6";
			ExpressionParser parser = new ExpressionParser(input);
			parser.Parse();

			Assert.AreEqual(0, parser.errors.count, parser.errors.ToString());
		}
		#endregion

		#region Scientific notation
		[TestMethod]
		public void ParseSciBigIntegerConstant()
		{
			string input = "8e9";
			ExpressionParser parser = new ExpressionParser(input);
			parser.Parse();

			Assert.AreEqual(0, parser.errors.count, parser.errors.ToString());
		}

		[TestMethod]
		public void ParseSciBigDecimalConstant()
		{
			string input = "8.6e9";
			ExpressionParser parser = new ExpressionParser(input);
			parser.Parse();

			Assert.AreEqual(0, parser.errors.count, parser.errors.ToString());
		}

		[TestMethod]
		public void ParseSciSmallIntegerConstant()
		{
			string input = "4e-6";
			ExpressionParser parser = new ExpressionParser(input);
			parser.Parse();

			Assert.AreEqual(0, parser.errors.count, parser.errors.ToString());
		}

		[TestMethod]
		public void ParseSciSmallDecimalConstant()
		{
			string input = "3.14e-6";
			ExpressionParser parser = new ExpressionParser(input);
			parser.Parse();

			Assert.AreEqual(0, parser.errors.count, parser.errors.ToString());
		}
		#endregion

		[TestMethod]
		public void ParseVariable()
		{
			string input = "lvl";
			ExpressionParser parser = new ExpressionParser(input);
			parser.Parse();

			Assert.AreEqual(0, parser.errors.count, parser.errors.ToString());
		}

		#endregion
	}
}
