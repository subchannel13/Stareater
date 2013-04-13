using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParser_Tests
{
	[TestClass]
	public class SummationTest
	{
		[TestMethod]
		public void Addition()
		{
			var test = new ParserTester("2+2", null, 4);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void Substraction()
		{
			var test = new ParserTester("2-5", null, -3);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[TestMethod]
		public void SegmentSummation()
		{
			var test = new ParserTester("1-2+3+6-1+9", null, 16);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
