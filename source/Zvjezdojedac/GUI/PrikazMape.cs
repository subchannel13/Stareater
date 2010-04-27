using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Prototip
{
	public class PrikazMape
	{
		private Igra igra;

		private double minX;

		private double maxX;

		private double minY;

		private double maxY;

		private double skala;

		private Image _slikaMape;

		public Image slikaMape
		{
			get { return _slikaMape; }
		}

		public const int VELICINA_SLIKE_ZVIJEZDE = 48;

		public const int MIN_SKALA_MAPE = 10;

		public const int MAX_SKALA_MAPE = 100;

		public const int POCETNA_SKALA_MAPE = 50;

		public PrikazMape(Igra igra)
		{
			this.igra = igra;
			this.skala = POCETNA_SKALA_MAPE;

			#region Određivanje dimenzija mape
			minX = igra.mapa.zvijezde[0].x;
			maxX = igra.mapa.zvijezde[0].x;
			minY = igra.mapa.zvijezde[0].y;
			maxY = igra.mapa.zvijezde[0].y;
			foreach (Zvijezda zvj in igra.mapa.zvijezde)
			{
				if (zvj.x < minX) minX = zvj.x;
				if (zvj.x > maxX) maxX = zvj.x;
				if (zvj.y < minY) minY = zvj.y;
				if (zvj.y > maxY) maxY = zvj.y;
			}
			#endregion

			osvjezi();
		}

		public Image osvjezi()
		{
			Bitmap bmpSlika = new Bitmap((int) (skala * (maxX - minX + 2)), (int) (skala * (maxY - minY + 2)));
			Graphics g = Graphics.FromImage(bmpSlika);
			g.Clear(Color.Black);
			Font font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
			Igrac igrac = igra.trenutniIgrac();

			foreach (Zvijezda zvj in igra.mapa.zvijezde)
			{
				if (zvj.tip == Zvijezda.Tip_Nikakva)
					continue;

				int x = (int) ((zvj.x - minX + 1) * skala);
				int y = (int) ((zvj.y - minY + 1) * skala);
                int d = (int)(Math.Sqrt(zvj.velicina) * VELICINA_SLIKE_ZVIJEZDE);

				g.DrawImage(
					Slike.ZvijezdaMapa[zvj.tip],
					new Rectangle(x - d/2, y - d/2, d, d)
					);

				if (skala >= VELICINA_SLIKE_ZVIJEZDE / 2)
				{
					Brush bojaImena = new SolidBrush(Color.FromArgb(64, 64, 64));
					if (igrac.posjeceneZvjezde.Contains(zvj))
						bojaImena = new SolidBrush(igrac.boja);
					g.DrawString(zvj.ime, font, bojaImena, x - g.MeasureString(zvj.ime, font).Width / 2, y + VELICINA_SLIKE_ZVIJEZDE / 2);
				}

				if (igrac.floteStacionarne.ContainsKey(zvj))
					g.DrawImage(Slike.Flota[igrac.boja], x + VELICINA_SLIKE_ZVIJEZDE / 2, y - VELICINA_SLIKE_ZVIJEZDE / 2);

				if (zvj == igrac.odabranaZvijezda)
				{
					Image img = Slike.SlikaOdabiraZvijezde;
					g.DrawImage(img, new Rectangle(x - img.Width/2, y - img.Height/2, img.Width, img.Height));
				}
			}

			g.Dispose();

			this._slikaMape = bmpSlika;
			return bmpSlika;
		}

		public double XnaMapi(int x)
		{
			return x / skala + minX - 1;
		}

		public double YnaMapi(int y)
		{
			return y / skala + minY - 1;
		}

		public int XsaMape(double x)
		{
			return (int)((x - minX + 1) * skala);
		}

		public int YsaMape(double y)
		{
			return (int)((y - minY + 1) * skala);
		}

		public void zoom(double skala)
		{
			this.skala = MIN_SKALA_MAPE + skala * (MAX_SKALA_MAPE - MIN_SKALA_MAPE);
			osvjezi();
		}
	}
}
