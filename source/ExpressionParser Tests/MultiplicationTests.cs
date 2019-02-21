using NUnit.Framework;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	[TestFixture]
	public class MultiplicationTests
	{
		[Test]
		public void Division()
		{
			var test = new ParserTester("2/3", null, null, 2.0 / 3, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void DivisionVar()
		{
			var test = new ParserTester("x/3", null, new Var("x", 2), 2.0 / 3, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void DivisionReminder()
		{
			var test = new ParserTester("10%3", null, null, 1, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void DivisionReminderDecimal()
		{
			var test = new ParserTester("1%0.8", null, null, 0.2, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void DivisionReminderNegativeBoth()
		{
			var test = new ParserTester("-10%-3", null, null, 1, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void DivisionReminderNegativeLeft()
		{
			var test = new ParserTester("-10%3", null, null, 2, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void DivisionReminderNegativeRigth()
		{
			var test = new ParserTester("10%-3", null, null, 2, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void DivisionReminderVar()
		{
			var test = new ParserTester("x%-3", null, new Var("x", 10), 2, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void IntegerDivision()
		{
			var test = new ParserTester("10\\3", null, null, 3, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void IntegerDivisionNegative()
		{
			var test = new ParserTester("-10\\3", null, null, -4, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}
		
		[Test]
		public void IntegerDivisionVar()
		{
			var test = new ParserTester("x\\3", null, new Var("x", -10), -4, 1e-9);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void Multiplication()
		{
			var test = new ParserTester("2*3", null, null, 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void MultiplicationReminder()
		{
			var test = new ParserTester("7*3%4", null, null, 1);
			Assert.IsTrue(test.IsOK, test.Message);
		}

		[Test]
		public void MultiplicationVar()
		{
			var test = new ParserTester("x*3", null, new Var("x", 2), 6);
			Assert.IsTrue(test.IsOK, test.Message);
		}
	}
}
