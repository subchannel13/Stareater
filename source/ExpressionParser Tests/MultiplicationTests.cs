using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParser_Tests
{
	[TestClass]
	public class MultiplicationTests
	{
		[TestMethod]
		public void Division()
		{
			var test = new ParserTester("2/3", null, 2.0 / 3, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void DivisionReminder()
		{
			var test = new ParserTester("10%3", null, 1, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void DivisionReminderDecimal()
		{
			var test = new ParserTester("1%0.8", null, 0.2, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void DivisionReminderNegativeBoth()
		{
			var test = new ParserTester("-10%-3", null, 1, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void DivisionReminderNegativeLeft()
		{
			var test = new ParserTester("-10%3", null, 2, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void DivisionReminderNegativeRigth()
		{
			var test = new ParserTester("10%-3", null, 2, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void IntegerDivision()
		{
			var test = new ParserTester("10\\3", null, 3, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void IntegerDivisionNegative()
		{
			var test = new ParserTester("-10\\3", null, -4, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void Multiplication()
		{
			var test = new ParserTester("2*3", null, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
