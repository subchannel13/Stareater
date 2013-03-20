
using System;
using System.Collections.Generic;
using System.IO;

namespace Stareater.AppData.Expressions {



public partial class ExpressionParser {
	public const int _EOF = 0;
	public const int _number = 1;
	public const int _identifier = 2;
	public const int _infinity = 3;
	public const int maxT = 36;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public Formula ParsedFormula { get; private set; }



	public ExpressionParser(String input) :
		this(new Scanner(input))
	{ }

	public ExpressionParser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Expression() {
		IExpressionNode root; 
		ComparisonExpr(out root);
		ParsedFormula = new Formula(root); 
	}

	void ComparisonExpr(out IExpressionNode node) {
		IExpressionNode child; 
		LogicalExpr(out child);
		IExpressionNode leftSide = child; 
		while (StartOf(1)) {
			double tolerance = 0; 
			switch (la.kind) {
			case 4: {
				Get();
				break;
			}
			case 5: {
				Get();
				break;
			}
			case 6: {
				Get();
				break;
			}
			case 7: {
				Get();
				break;
			}
			case 8: {
				Get();
				break;
			}
			case 9: {
				Get();
				break;
			}
			case 10: {
				Get();
				break;
			}
			case 11: {
				Get();
				break;
			}
			case 12: {
				Get();
				break;
			}
			}
			string operatorSymbol = t.val; 
			LogicalExpr(out child);
			IExpressionNode rightSide = child; 
			if (la.kind == 13) {
				Get();
				Expect(1);
				tolerance = toDouble(t.val); 
			}
			leftSide = makeComparison(leftSide, operatorSymbol, rightSide, tolerance); 
		}
		node = leftSide; 
	}

	void LogicalExpr(out IExpressionNode node) {
		IExpressionNode child; 
		Queue<IExpressionNode> children = new Queue<IExpressionNode>();
		Queue<string> operators = new Queue<string>(); 
		AddExpr(out child);
		children.Enqueue(child); 
		while (StartOf(2)) {
			switch (la.kind) {
			case 14: {
				Get();
				break;
			}
			case 15: {
				Get();
				break;
			}
			case 16: {
				Get();
				break;
			}
			case 17: {
				Get();
				break;
			}
			case 18: {
				Get();
				break;
			}
			case 19: {
				Get();
				break;
			}
			case 20: {
				Get();
				break;
			}
			case 21: {
				Get();
				break;
			}
			}
			operators.Enqueue(t.val); 
			AddExpr(out child);
			children.Enqueue(child); 
		}
		node = makeBooleanAritmenthics(children, operators); 
	}

	void AddExpr(out IExpressionNode node) {
		IExpressionNode child; 
		Queue<IExpressionNode> children = new Queue<IExpressionNode>();
		Queue<string> operators = new Queue<string>(); 
		MultExpr(out child);
		children.Enqueue(child); 
		while (la.kind == 22 || la.kind == 23) {
			if (la.kind == 22) {
				Get();
			} else {
				Get();
			}
			operators.Enqueue(t.val); 
			MultExpr(out child);
			children.Enqueue(child); 
		}
		node = makeSummation(children, operators); 
	}

	void MultExpr(out IExpressionNode node) {
		IExpressionNode child; 
		Queue<IExpressionNode> children = new Queue<IExpressionNode>();
		Queue<string> operators = new Queue<string>(); 
		ExpExpr(out child);
		children.Enqueue(child); 
		while (StartOf(3)) {
			if (la.kind == 24) {
				Get();
			} else if (la.kind == 25) {
				Get();
			} else if (la.kind == 26) {
				Get();
			} else {
				Get();
			}
			operators.Enqueue(t.val); 
			ExpExpr(out child);
			children.Enqueue(child); 
		}
		node = makeMultiplications(children, operators); 
	}

	void ExpExpr(out IExpressionNode node) {
		IExpressionNode child; 
		List<IExpressionNode> children = new List<IExpressionNode>(); 
		UnaryExpr(out child);
		children.Add(child); 
		while (la.kind == 28) {
			Get();
			UnaryExpr(out child);
			children.Add(child); 
		}
		node = new ExponentSequence(children).Simplified(); 
	}

	void UnaryExpr(out IExpressionNode node) {
		IExpressionNode child; 
		UnaryOperator operatorNode = null; 
		if (la.kind == 23 || la.kind == 29 || la.kind == 30) {
			if (la.kind == 23) {
				Get();
				operatorNode = new Negation(); 
			} else if (la.kind == 29) {
				Get();
				operatorNode = new ToBoolean(); 
			} else {
				Get();
				operatorNode = new NegateToBoolean(); 
			}
		}
		List(out child);
		if (operatorNode != null) operatorNode.child = child; 
		node = (operatorNode != null) ? operatorNode : child; 
	}

	void List(out IExpressionNode node) {
		IExpressionNode child, index; 
		List<IExpressionNode> children = new List<IExpressionNode>();
		int listStart = 0; 
		Term(out index);
		if (la.kind == 31) {
			Get();
			listStart = t.charPos; 
			ComparisonExpr(out child);
			children.Add(child); 
			while (la.kind == 32) {
				Get();
				ComparisonExpr(out child);
				children.Add(child); 
			}
			Expect(33);
		}
		node = makeList(index, children, listStart); 
	}

	void Term(out IExpressionNode node) {
		IExpressionNode child; 
		node = new Constant(double.NaN); 
		if (la.kind == 34) {
			Get();
			ComparisonExpr(out child);
			Expect(35);
			node = child; 
		} else if (la.kind == 1) {
			Get();
			node = new Constant(toDouble(t.val)); 
		} else if (la.kind == 3) {
			Get();
			node = new Constant(double.PositiveInfinity); 
		} else if (la.kind == 2) {
			Get();
			string identifierName = t.val;
			List<IExpressionNode> children = new List<IExpressionNode>();
			int functionStart = 0; 
			if (la.kind == 34) {
				Get();
				ComparisonExpr(out child);
				children.Add(child); 
				while (la.kind == 32) {
					Get();
					ComparisonExpr(out child);
					children.Add(child); 
				}
				Expect(35);
			}
			node = makeFunction(identifierName, children, functionStart); 
		} else SynErr(37);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Expression();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,x,x,x, T,T,T,T, T,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, x,x,x,x, x,x,x,x, x,x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.Text.StringBuilder errorMessages  = new System.Text.StringBuilder();   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}" + Environment.NewLine; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "number expected"; break;
			case 2: s = "identifier expected"; break;
			case 3: s = "infinity expected"; break;
			case 4: s = "\"=\" expected"; break;
			case 5: s = "\"<>\" expected"; break;
			case 6: s = "\"\u2260\" expected"; break;
			case 7: s = "\"<\" expected"; break;
			case 8: s = "\"<=\" expected"; break;
			case 9: s = "\"\u2264\" expected"; break;
			case 10: s = "\">\" expected"; break;
			case 11: s = "\">=\" expected"; break;
			case 12: s = "\"\u2265\" expected"; break;
			case 13: s = "\"~\" expected"; break;
			case 14: s = "\"&\" expected"; break;
			case 15: s = "\"\u2227\" expected"; break;
			case 16: s = "\"\u22c0\" expected"; break;
			case 17: s = "\"|\" expected"; break;
			case 18: s = "\"\u2228\" expected"; break;
			case 19: s = "\"\u22c1\" expected"; break;
			case 20: s = "\"@\" expected"; break;
			case 21: s = "\"\u2295\" expected"; break;
			case 22: s = "\"+\" expected"; break;
			case 23: s = "\"-\" expected"; break;
			case 24: s = "\"*\" expected"; break;
			case 25: s = "\"/\" expected"; break;
			case 26: s = "\"\\\\\" expected"; break;
			case 27: s = "\"%\" expected"; break;
			case 28: s = "\"^\" expected"; break;
			case 29: s = "\"\'\" expected"; break;
			case 30: s = "\"-\'\" expected"; break;
			case 31: s = "\"[\" expected"; break;
			case 32: s = "\",\" expected"; break;
			case 33: s = "\"]\" expected"; break;
			case 34: s = "\"(\" expected"; break;
			case 35: s = "\")\" expected"; break;
			case 36: s = "??? expected"; break;
			case 37: s = "invalid Term"; break;

			default: s = "error " + n; break;
		}
		errorMessages.AppendFormat(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorMessages.AppendFormat(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorMessages.AppendFormat(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorMessages.AppendFormat(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorMessages.AppendFormat(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}