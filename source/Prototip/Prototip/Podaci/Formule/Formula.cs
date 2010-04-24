using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Prototip
{
	public abstract class Formula
	{
		#region Elementi izraza

		private static Dictionary<string, Operator> operatoriDict = null;

		protected static Dictionary<string, Operator> operatori()
		{
			if (operatoriDict == null)
			{
				operatoriDict = new Dictionary<string, Operator>();
				operatoriDict.Add("+", new OperatorPlus());
				operatoriDict.Add("-", new OperatorMinus());
				operatoriDict.Add("*", new OperatorPuta());
				operatoriDict.Add("/", new OperatorKroz());
				operatoriDict.Add("^", new OperatorNa());
				operatoriDict.Add("POW", new OperatorNa());
				operatoriDict.Add("MIN", new OperatorMin());
				operatoriDict.Add("MAX", new OperatorMax());
			}

			return operatoriDict;
		}

		public abstract class Element
		{
			public static Element naciniElement(Stack<String> niz)
			{
				Dictionary<string, Operator> op = operatori();

				double x;
				string s = niz.Pop().Trim();

				if (op.ContainsKey(s))
					return op[s].naciniOperator(niz);

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

		public abstract class Operator : Element
		{
			public abstract Operator naciniOperator(Stack<string> niz);
		}

		public abstract class BinarniOperator : Operator
		{
			protected Element lijeviOpernad;
			protected Element desniOperand;

			public override void popisiVarijable(List<Varijabla> varijable)
			{
				lijeviOpernad.popisiVarijable(varijable);
				desniOperand.popisiVarijable(varijable);
			}

			public override void obrniOperande()
			{
				Element tmp = lijeviOpernad;
				lijeviOpernad = desniOperand;
				desniOperand = tmp;
				lijeviOpernad.obrniOperande();
				desniOperand.obrniOperande();
			}
		}

		public class OperatorPlus : BinarniOperator
		{
			public OperatorPlus() : this(null, null)
			{}

			public OperatorPlus(Element lijeviOpernad, Element desniOperand)
			{
				this.lijeviOpernad = lijeviOpernad;
				this.desniOperand = desniOperand;
			}

			public override double vrijednost()
			{
				return lijeviOpernad.vrijednost() + desniOperand.vrijednost();
			}

			public override Operator naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOpernad = Element.naciniElement(niz);
				return new OperatorPlus(lijeviOpernad, desniOperand);
			}

			public override string ToString()
			{
				return "+ " + lijeviOpernad + " " + desniOperand;
			}
		}

		public class OperatorMinus : BinarniOperator
		{
			public OperatorMinus() : this(null, null)
			{}

			public OperatorMinus(Element lijeviOpernad, Element desniOperand)
			{
				this.lijeviOpernad = lijeviOpernad;
				this.desniOperand = desniOperand;
			}

			public override double vrijednost()
			{
				return lijeviOpernad.vrijednost() - desniOperand.vrijednost();
			}

			public override Operator naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOpernad = Element.naciniElement(niz);
				return new OperatorMinus(lijeviOpernad, desniOperand);
			}

			public override string ToString()
			{
				return "- " + lijeviOpernad + " " + desniOperand;
			}
		}

		public class OperatorPuta : BinarniOperator
		{
			public OperatorPuta() : this(null, null)
			{}

			public OperatorPuta(Element lijeviOpernad, Element desniOperand)
			{
				this.lijeviOpernad = lijeviOpernad;
				this.desniOperand = desniOperand;
			}

			public override double vrijednost()
			{
				return lijeviOpernad.vrijednost() * desniOperand.vrijednost();
			}

			public override Operator naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOpernad = Element.naciniElement(niz);
				return new OperatorPuta(lijeviOpernad, desniOperand);
			}

			public override string ToString()
			{
				return "* " + lijeviOpernad + " " + desniOperand;
			}
		}

		public class OperatorKroz : BinarniOperator
		{
			public OperatorKroz() : this(null, null)
			{}

			public OperatorKroz(Element lijeviOpernad, Element desniOperand)
			{
				this.lijeviOpernad = lijeviOpernad;
				this.desniOperand = desniOperand;
			}

			public override double vrijednost()
			{
				return lijeviOpernad.vrijednost() / desniOperand.vrijednost();
			}

			public override Operator naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOpernad = Element.naciniElement(niz);
				return new OperatorKroz(lijeviOpernad, desniOperand);
			}

			public override string ToString()
			{
				return "/ " + lijeviOpernad + " " + desniOperand;
			}
		}

		public class OperatorNa : BinarniOperator
		{
			public OperatorNa()
				: this(null, null)
			{ }

			public OperatorNa(Element lijeviOpernad, Element desniOperand)
			{
				this.lijeviOpernad = lijeviOpernad;
				this.desniOperand = desniOperand;
			}

			public override double vrijednost()
			{
				return Math.Pow(lijeviOpernad.vrijednost(), desniOperand.vrijednost());
			}

			public override Operator naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOpernad = Element.naciniElement(niz);
				return new OperatorNa(lijeviOpernad, desniOperand);
			}

			public override string ToString()
			{
				return "^ " + lijeviOpernad + " " + desniOperand;
			}
		}

		public class OperatorMin : BinarniOperator
		{
			public OperatorMin()
				: this(null, null)
			{ }

			public OperatorMin(Element lijeviOpernad, Element desniOperand)
			{
				this.lijeviOpernad = lijeviOpernad;
				this.desniOperand = desniOperand;
			}

			public override double vrijednost()
			{
				return Math.Min(lijeviOpernad.vrijednost(), desniOperand.vrijednost());
			}

			public override Operator naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOpernad = Element.naciniElement(niz);
				return new OperatorMin(lijeviOpernad, desniOperand);
			}

			public override string ToString()
			{
				return "MIN " + lijeviOpernad + " " + desniOperand;
			}
		}

		public class OperatorMax : BinarniOperator
		{
			public OperatorMax() : this(null, null)
			{}

			public OperatorMax(Element lijeviOpernad, Element desniOperand)
			{
				this.lijeviOpernad = lijeviOpernad;
				this.desniOperand = desniOperand;
			}

			public override double vrijednost()
			{
				return Math.Max(lijeviOpernad.vrijednost(), desniOperand.vrijednost());
			}

			public override Operator naciniOperator(Stack<string> niz)
			{
				Element desniOperand = Element.naciniElement(niz);
				Element lijeviOpernad = Element.naciniElement(niz);
				return new OperatorMax(lijeviOpernad, desniOperand);
			}

			public override string ToString()
			{
				return "MAX " + lijeviOpernad + " " + desniOperand;
			}
		}

		#endregion

		public static Formula NaciniFormulu(string niz)
		{
			niz.Replace("+", " + ");
			niz.Replace("-", " - ");
			niz.Replace("*", " * ");
			niz.Replace("/", " / ");
			niz.Replace("^", " ^ ");
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
				Formula formula = NaciniFormulu(niz);
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
