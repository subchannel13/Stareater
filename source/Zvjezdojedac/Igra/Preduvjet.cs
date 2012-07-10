using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci.Formule;

namespace Zvjezdojedac.Igra
{
	public class Preduvjet
	{
		public static List<Preduvjet> NaciniPreduvjete(string podaci)
		{
			return NaciniPreduvjete(podaci, true);
		}
		public static List<Preduvjet> NaciniPreduvjete(string podaci, bool dodajLvl)
		{
			List<Preduvjet> ret = new List<Preduvjet>();
			string[] preduvjetiStr = podaci.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < preduvjetiStr.Length; i += 2)
				ret.Add(new Preduvjet(preduvjetiStr[i].Trim(), Formula.IzStringa(preduvjetiStr[i + 1]), dodajLvl));
			return ret;
		}
		public static string UString(List<Preduvjet> preduvjeti, bool ukloniLvl)
		{
			if (preduvjeti.Count == 0)
				return "";

			StringBuilder sb = new StringBuilder();
			foreach (Preduvjet p in preduvjeti) {
				if (ukloniLvl) {
					int lvlIndex = p.kod.IndexOf("_LVL");
					if (lvlIndex > 0)
						sb.Append(p.kod.Substring(0, lvlIndex));
				}
				else
					sb.Append(p.kod);
				sb.Append(" | ");
				sb.Append(p.nivo.ToString());
				sb.Append(" | ");
			}
			sb.Remove(sb.Length - 3, 3);

			return sb.ToString();
		}

		public string kod;
		public Formula nivo;

		public Preduvjet(string kod, Formula nivo, bool dodajLvl)
		{
			if (dodajLvl)
				this.kod = kod + "_LVL";
			else
				this.kod = kod;
			this.nivo = nivo;
		}

		public bool zadovoljen(Dictionary<string, double> varijable)
		{
			if (Math.Round(varijable[kod]) >= nivo.iznos(varijable))
				return true;
			else
				return false;
		}
	}

}
