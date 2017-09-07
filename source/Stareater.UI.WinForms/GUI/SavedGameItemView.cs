using System;
using System.Windows.Forms;

using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.Properties;
using Stareater.Utils.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.GUI
{
	public partial class SavedGameItemView : UserControl
	{
		private SavedGameInfo gameData;
		private Image previewImage = null;
		
		public SavedGameItemView()
		{
			InitializeComponent();
		}
		
		public SavedGameInfo Data 
		{
			get
			{
				return this.gameData;
			}
			set
			{
				this.gameData = value;
				
				if (this.previewImage != null)
				{
					this.previewImage.Dispose();
					this.previewImage = null;
                }

				var context = LocalizationManifest.Get.CurrentLanguage[FormSaveLoad.LanguageContext];
				
				if (value != null)
				{
					this.gameName.Text = gameData.Title;
                    this.turnText.Text = context["Turn"].Text(null, new TextVar("turn", SavePreviewGenerator.TurnOf(gameData.PreviewData).ToString()).Get);

					this.previewImage = this.drawPreview(SavePreviewGenerator.StarsOf(gameData.PreviewData).ToList());
					this.preview.Image = this.previewImage;
                }
				else
				{
					this.preview.Image = Resources.newSave;
					this.gameName.Text = context["NewSave"].Text();
					this.turnText.Text = "";
				}
			}
		}

		public string GameName
		{
			get { return this.gameName.Text; }
		}

		public void OnSelect()
		{
			this.gameName.Enabled = true;
			this.gameName.Focus();
		}
		
		public void Deselect()
		{
			this.gameName.Enabled = false;
		}
		
		void PreviewClick(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void GameNameClick(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void TurnTextClick(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}

		private Image drawPreview(IEnumerable<Tuple<PointF, Color>> stars)
		{
			const int padding = 3;
			const float dotSize = 1.5f;

			var minX = stars.Select(p => p.Item1.X).Min();
			var minY = stars.Select(p => p.Item1.Y).Min();
			var maxX = stars.Select(p => p.Item1.X).Max();
			var maxY = stars.Select(p => p.Item1.Y).Max();

			var scaleX = (this.preview.Width - 2 * padding) / (maxX - minX);
			var scaleY = (this.preview.Height - 2 * padding) / (maxY - minY);

			var image = new Bitmap(this.preview.Width, this.preview.Height);
			using (var g = Graphics.FromImage(image))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.Clear(Color.Black);

				foreach (var star in stars)
					using (var starColor = new SolidBrush(star.Item2))
						g.FillEllipse(
							starColor,
							(star.Item1.X - minX) * scaleX + padding,
							(star.Item1.Y - minY) * scaleY + padding,
							dotSize,
							dotSize
						);
			}

			return image;
        }
	}
}
