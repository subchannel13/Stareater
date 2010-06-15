using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Prototip
{
	public abstract class Formula
	{
		#region Elementi izraza

		public delegate Element OperatorTvornica(Stack<String> niz);
		private static Dictionary<string, OperatorTvornica> operatoriDict = null;

		protected static Dictionary<string, OperatorTvornica> operatori()
		{
			if (operatoriDict == null)
			{
				operatoriDict = new Dictionary<string, OperatorTvornica>();
				operatoriDict.Add("+", OperatorPlus.naciniOperator);
				operatoriDict.Add("-", OperatorMinus.naciniOperator);
				operatoriDict.Add("*", OperatorPuta.naciniOperator);
				operatoriDict.Add("/", OperatorKroz.naciniOperator);
				operatoriDict.Add("^", OperatorNa.naciniOperator);
				operatoriDict.Add("POW", OperatorNa.naciniOperator);
				operatoriDict.Add("MIN", OperatorMin.naciniOperator);
				operatoriDict.Add("MAX", OperatorMax.naciniOperator);
				operatoriDict.Add("DIV", OperatorDiv.naciniOperator);
				operatoriDict.Add("%", OperatorMod.naciniOperator);
				operatoriDict.Add("MOD", OperatorMod.naciniOperator);

				operatoriDict.Add("INT", OperatorInt.naciniOperator);
				operatoriDict.Add("FIX", OperatorInt.naciniOperator);
				operatoriDict.Add("TRUNC", OperatorInt.naciniOperator);
				operatoriDict.Add("ROUND", OperatorRound.naciniOperator);
				operatoriDict.Add("SGN", OperatorSignum.naciniOperator);
				operatoriDict.Add("SIGN", OperatorSignum.naciniOperator);
				operatoriDict.Add("FLOOR", OperatorFloor.naciniOperator);
				operatoriDict.Add("CEIL", OperatorCeiling.naciniOperator);

				operatoriDict.Add("ITE", OperatorITE.naciniOperator);
				operatoriDict.Add("IF", OperatorITE.naciniOperator);
				operatoriDict.Add("LIMIT", OperatorLimit.naciniOperator);
				operatoriDict.Add("BATAK", OperatorFrom.naciniOperator);
				operatoriDict.Add("FROM", OperatorFrom.naciniOperator);
				operatoriDict.Add("INTER", OperatorFrom.naciniOperator);
				operatoriDict.Add("LFROM", OperatorLimitFrom.naciniOperator);
			}

			return operatoriDict;
		}

		public abstract class Element
		{
			public static Element naciniElement(Stack<String> niz)
			{
				Dictionary<string, OperatorTvornica> op = operatori();

				double x;
				string s = niz.Pop().Trim();
				string[] args = s.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

				if (op.ContainsKey(s)) {
					for (int i = 1; i < args.Length; i++)
						niz.Push(args[i]);
					return op[s](niz);
				}

				else if (Double.TryParse(s, NumberStyles.AllowDecimalPoint, Podaci.DecimalnaTocka, out x))
					return new Konstanta(x);
				else
					return new Varijabla(s);
			}

			public abstract double vrijednost();

			public abstract void popisiVarijable(List<Varijabla> varijable);

			public abstract void obrniOperande();

			public abstract override string ToString();
		}

		public class Konstanta : Element
		{
			protected double iznos;

			public Konstanta(double iznos)
			{
				this.iznos = iznos;
			}

			public override double vrijednost()
			{
				return iznos;
			}

			public override void popisiVarijable(List<Varijabla> varijable)
			{}

			public override void obrniOperande()
			{}

			public override string ToString()
			{
				return iznos.ToString(Podaci.DecimalnaTocka);
			}
		}

		public class Varijabla : Konstanta
		{
			public string ime;

			public Varijabla(string ime) : base(0)
			{
				this.ime = ime;
			}

			public void postaviVrijednost(double v)
			{
				iznos = v;
			}

			public override void popisiVarijable(List<Varijabla> varijable)
			{
				varijable.Add(this);
			}

			public override void obrniOperande()
			{ }

			public override string ToString()
			{
				return ime;
			}
		}

		public abstract class UnarniOperator : Element
		{
			protected Element operand;

			public UnarniOperator(Element operand)
			{
				this.operand = operand;
			}

			public override void popisiVarijable(List<Varijabla> varijable)
			{
				operand.popisiVarijable(varijable);
			}

			public override void obrniOperande()
			{ }
		}

		public class OperatorInt : UnarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element operand = Element.naciniElement(niz);
				return new OperatorInt(operand);
			}

			public OperatorInt(Element operand)
				: base(operand)
			{ }

			public override double vrijednost()
			{
				return Math.Truncate(operand.vrijednost());
			}

			public override string ToString()
			{
				return "INT " + operand;
			}
		}
		public class OperatorRound : UnarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element operand = Element.naciniElement(niz);
				return new OperatorRound(operand);
			}

			public OperatorRound(Element operand)
				: base(operand)
			{ }

			public override double vrijednost()
			{
				return Math.Round(operand.vrijednost());
			}

			public override string ToString()
			{
				return "ROUND " + operand;
			}
		}
		public class OperatorAbs : UnarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element operand = Element.naciniElement(niz);
				return new OperatorAbs(operand);
			}

			public OperatorAbs(Element operand)
				: base(operand)
			{ }

			public override double vrijednost()
			{
				return Math.Abs(operand.vrijednost());
			}

			public override string ToString()
			{
				return "ABS " + operand;
			}
		}
		public class OperatorSignum : UnarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element operand = Element.naciniElement(niz);
				return new OperatorSignum(operand);
			}

			public OperatorSignum(Element operand)
				: base(operand)
			{ }

			public override double vrijednost()
			{
				return Math.Sign(operand.vrijednost());
			}

			public override string ToString()
			{
				return "SIGN " + operand;
			}
		}
		public class OperatorFloor : UnarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element operand = Element.naciniElement(niz);
				return new OperatorFloor(operand);
			}

			public OperatorFloor(Element operand)
				: base(operand)
			{ }

			public override double vrijednost()
			{
				return Math.Floor(operand.vrijednost());
			}

			public override string ToString()
			{
				return "FLOOR " + operand;
			}
		}
		public class OperatorCeiling : UnarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element operand = Element.naciniElement(niz);
				return new OperatorCeiling(operand);
			}

			public OperatorCeiling(Element operand)
				: base(operand)
			{ }

			public override double vrijednost()
			{
				return Math.Ceiling(operand.vrijednost());
			}

			public override string ToString()
			{
				return "CEIL " + operand;
			}
		}

		public abstract class BinarniOperator : Element
		{
			protected Element lijeviOperand;
			protected Element desniOperand;

			public BinarniOperator(Element lijeviOperand, Element desniOperand)
			{
				this.lijeviOperand = lijeviOperand;
				this.desniOperand = desniOperand;
			}

			public override void popisiVarijable(List<Varijabla> varijable)
			{
				lijeviOperand.popisiVarijable(varijable);
				desniOperand.popisiVarijable(varijable);
			}

			public override void obrniOperande()
			{
				Element tmp = lijeviOperand;
				lijeviOperand = desniOperand;
				desniOperand = tmp;
				lijeviOperand.obrniOperande();
				desniOperand.obrniOperande();
			}
		}

		public class OperatorPlus : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorPlus(lijeviOperand, desniOperand);
			}

			public OperatorPlus(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				return lijeviOperand.vrijednost() + desniOperand.vrijednost();
			}

			public override string ToString()
			{
				return "+ " + lijeviOperand + " " + desniOperand;
			}
		}
		public class OperatorMinus : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorMinus(lijeviOperand, desniOperand);
			}
			
			public OperatorMinus(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				return lijeviOperand.vrijednost() - desniOperand.vrijednost();
			}

			public override string ToString()
			{
				return "- " + lijeviOperand + " " + desniOperand;
			}
		}
		public class OperatorPuta : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorPuta(lijeviOperand, desniOperand);
			}

			public OperatorPuta(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				return lijeviOperand.vrijednost() * desniOperand.vrijednost();
			}

			public override string ToString()
			{
				return "* " + lijeviOperand + " " + desniOperand;
			}
		}
		public class OperatorKroz : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorKroz(lijeviOperand, desniOperand);
			}

			public OperatorKroz(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				return lijeviOperand.vrijednost() / desniOperand.vrijednost();
			}

			public override string ToString()
			{
				return "/ " + lijeviOperand + " " + desniOperand;
			}
		}
		public class OperatorNa : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorNa(lijeviOperand, desniOperand);
			}

			public OperatorNa(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				return Math.Pow(lijeviOperand.vrijednost(), desniOperand.vrijednost());
			}

			public override string ToString()
			{
				return "^ " + lijeviOperand + " " + desniOperand;
			}
		}
		public class OperatorMin : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorMin(lijeviOperand, desniOperand);
			}

			public OperatorMin(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				return Math.Min(lijeviOperand.vrijednost(), desniOperand.vrijednost());
			}

			public override string ToString()
			{
				return "MIN " + lijeviOperand + " " + desniOperand;
			}
		}
		public class OperatorMax : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorMax(lijeviOperand, desniOperand);
			}

			public OperatorMax(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				return Math.Max(lijeviOperand.vrijednost(), desniOperand.vrijednost());
			}

			public override string ToString()
			{
				return "MAX " + lijeviOperand + " " + desniOperand;
			}
		}
		public class OperatorDiv : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorDiv(lijeviOperand, desniOperand);
			}

			public OperatorDiv(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				return Math.Truncate(lijeviOperand.vrijednost() / desniOperand.vrijednost());
			}

			public override string ToString()
			{
				return "DIV " + lijeviOperand + " " + desniOperand;
			}
		}
		public class OperatorMod : BinarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorMod(lijeviOperand, desniOperand);
			}

			public OperatorMod(Element lijeviOperand, Element desniOperand)
				: base(lijeviOperand, desniOperand)
			{ }

			public override double vrijednost()
			{
				double d = lijeviOperand.vrijednost() / desniOperand.vrijednost();
				return d - Math.Floor(d);
			}

			public override string ToString()
			{
				return "MOD " + lijeviOperand + " " + desniOperand;
			}
		}

		public abstract class TernarniOperator : BinarniOperator
		{
			protected Element srednjiOpernad;

			public TernarniOperator(Element lijeviOperand, 
				Element srednjiOpernad, Element desniOperand)
				:base(lijeviOperand, desniOperand)
			{
				this.srednjiOpernad = srednjiOpernad;
			}

			public override void popisiVarijable(List<Varijabla> varijable)
			{
				base.popisiVarijable(varijable);
				srednjiOpernad.popisiVarijable(varijable);
			}
		}

		public class OperatorITE : TernarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element srednjiOpernad = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorITE(lijeviOperand, srednjiOpernad, desniOperand);
			}

			public OperatorITE(Element lijeviOperand, 
				Element srednjiOpernad, Element desniOperand)
				: base(lijeviOperand, srednjiOpernad, desniOperand)
			{ }

			public override double vrijednost()
			{
				if (lijeviOperand.vrijednost() >= 0)
					return srednjiOpernad.vrijednost();
				else
					return desniOperand.vrijednost();
			}

			public override string ToString()
			{
				return "ITE " + lijeviOperand + " " + srednjiOpernad + " " + desniOperand;
			}
		}
		public class OperatorLimit : TernarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element srednjiOpernad = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorLimit(lijeviOperand, srednjiOpernad, desniOperand);
			}

			public OperatorLimit(Element lijeviOperand,
				Element srednjiOpernad, Element desniOperand)
				: base(lijeviOperand, srednjiOpernad, desniOperand)
			{ }

			public override double vrijednost()
			{
				double t = lijeviOperand.vrijednost();
				double min = srednjiOpernad.vrijednost(); ;
				if (t <= min)
					return min;

				double max = desniOperand.vrijednost(); ;
				if (t >= max)
					return max;
				else
					return t;
			}

			public override string ToString()
			{
				return "LIMIT " + lijeviOperand + " " + srednjiOpernad + " " + desniOperand;
			}
		}
		public class OperatorFrom : TernarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element srednjiOpernad = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorFrom(lijeviOperand, srednjiOpernad, desniOperand);
			}

			public OperatorFrom(Element lijeviOperand,
				Element srednjiOpernad, Element desniOperand)
				: base(lijeviOperand, srednjiOpernad, desniOperand)
			{ }

			public override double vrijednost()
			{
				double t = lijeviOperand.vrijednost();
				double min = srednjiOpernad.vrijednost(); ;
				double max = desniOperand.vrijednost(); ;
				
				return min + t * (max - min);
			}

			public override string ToString()
			{
				return "FROM " + lijeviOperand + " " + srednjiOpernad + " " + desniOperand;
			}
		}
		public class OperatorLimitFrom : TernarniOperator
		{
			public static Element naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element srednjiOpernad = Element.naciniElement(niz);
				Element lijeviOperand = Element.naciniElement(niz);
				return new OperatorLimitFrom(lijeviOperand, srednjiOpernad, desniOperand);
			}

			public OperatorLimitFrom(Element lijeviOperand,
				Element srednjiOpernad, Element desniOperand)
				: base(lijeviOperand, srednjiOpernad, desniOperand)
			{ }

			public override double vrijednost()
			{
				double t = lijeviOperand.vrijednost();
				double min = srednjiOpernad.vrijednost(); ;
				double max = desniOperand.vrijednost(); ;

				if (t <= min) return min;
				if (t >= max) return max;
				return min + t * (max - min);
			}

			public override string ToString()
			{
				return "LFROM " + lijeviOperand + " " + srednjiOpernad + " " + desniOperand;
			}
		}
		#endregion

		public static Formula IzStringa(string niz)
		{
			niz.Replace("+", " + ");
			niz.Replace("-", " - ");
			niz.Replace("*", " * ");
			niz.Replace("/", " / ");
			niz.Replace("^", " ^ ");
			niz.Replace("%", " % ");
			niz = niz.ToUpper();
			List<string> lista = new List<string>(niz.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
			bool obrnutNiz = false;
			if (operatori().ContainsKey(lista[0]))
			{
				lista.Reverse();
				obrnutNiz = true;
			}
			Stack<string> elementi = new Stack<string>(lista);

			Element vrh = null;
			try
			{
				vrh = Element.naciniElement(elementi);
				if (elementi.Count > 0)
					throw new FormatException("Neispravna formula,\n\n" + niz + "\n \nnije niti u ispravnom prefiks niti u ispravnom postfiks obliku.");
			}
			catch (InvalidOperationException)
			{
				throw new FormatException("Neispravna formula,\n\n" + niz + "\n \nnije niti u ispravnom prefiks niti u ispravnom postfiks obliku.");
			}
			if (obrnutNiz) vrh.obrniOperande();

			List<Varijabla> varijable = new List<Varijabla>();
			vrh.popisiVarijable(varijable);

			if (varijable.Count == 0)
				return new KonstantnaFormula(vrh.vrijednost());

			return new VarijabilnaFormula(vrh, varijable);
		}

		public static bool ValjanaFormula(string niz)
		{
			try
			{
				Formula formula = IzStringa(niz);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public abstract double iznos(Dictionary<string, double> varijable);

		public abstract List<Varijabla> popisVarijabli();

		public abstract void preimenujVarijablu(string staroIme, string novoIme);

		public abstract override string ToString();

	}
}
