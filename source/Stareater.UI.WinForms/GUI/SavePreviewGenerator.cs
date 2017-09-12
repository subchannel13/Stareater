using Ikadn.Ikon.Types;
using Stareater.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stareater.GUI
{
	class SavePreviewGenerator
	{
		public const string Tag = "WinformsPreview";
		private const string StarTag = "Star";
		private const string StarsKey = "Stars";
		private const string TurnKey = "turn";
		private const string ColorKey = "color";

		private GameController controller;

		public SavePreviewGenerator(GameController controller)
		{
			this.controller = controller;
		}

		public IkonBaseObject Make()
		{
			//TODO(later) assumes single player
			var player = this.controller.LocalHumanPlayers().First();
			var starData = new IkonArray();
			foreach(var star in player.Stars)
			{
				var data = new IkonComposite(StarTag);
				data.Add("x", new IkonFloat(star.Position.X));
				data.Add("y", new IkonFloat(star.Position.Y));
				data.Add(ColorKey, new IkonArray(
					new []
					{
						new IkonInteger(star.Color.R),
						new IkonInteger(star.Color.G),
						new IkonInteger(star.Color.B)
					}
				));
				starData.Add(data);
			}

			var previewData = new IkonComposite(Tag);
			previewData.Add(TurnKey, new IkonInteger(player.Turn));
			previewData.Add(StarsKey, starData);

			return previewData;
		}

		public static int TurnOf(IkonBaseObject previewData)
		{
			return previewData.To<IkonComposite>()[TurnKey].To<int>();
        }

		public static IEnumerable<Tuple<PointF, Color>> StarsOf(IkonBaseObject previewData)
		{
			var starData = previewData.To<IkonComposite>()[StarsKey].To<IkonArray>();

			foreach (var item in starData.Select(x => x.To<IkonComposite>()))
			{
				var colorData = item[ColorKey].To<int[]>();
				yield return new Tuple<PointF, Color>(
					new PointF(
						item["x"].To<float>(),
						-item["y"].To<float>()
					),
					Color.FromArgb(colorData[0], colorData[1], colorData[2])
				);
			}
        }
	}
}
