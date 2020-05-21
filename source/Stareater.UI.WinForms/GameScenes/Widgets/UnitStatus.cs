using OpenTK;
using Stareater.Controllers;
using Stareater.Controllers.Views.Combat;
using Stareater.GLData;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using System.Drawing;

namespace Stareater.GameScenes.Widgets
{
	class UnitStatus : GuiPanel
	{
		private readonly GuiText shipCount;
		private readonly GuiText movementInfo;
		private readonly GuiButton doneButon;
		private readonly GuiText armorInfo;
		private readonly GuiText shieldsInfo;

		private SpaceBattleController controller;

		public UnitStatus()
		{
			this.Background = new BackgroundTexture(GalaxyTextures.Get.PanelBackground, 6);
			this.Position.FixedSize(360, 50);

			this.shipCount = new GuiText
			{
				Margins = new Vector2(8, 4),
				TextColor = Color.Black,
				TextHeight = 12
			};
			this.shipCount.Position.WrapContent().Then.ParentRelative(-1, 1).UseMargins();
			this.AddChild(this.shipCount);

			this.movementInfo = new GuiText
			{
				Margins = new Vector2(0, 4),
				TextColor = Color.Black,
				TextHeight = 12
			};
			this.movementInfo.Position.WrapContent().Then.RelativeTo(this.shipCount, -1, -1, -1, 1).UseMargins();
			this.AddChild(this.movementInfo);

			this.doneButon = new GuiButton
			{
				BackgroundHover = new BackgroundTexture(GalaxyTextures.Get.ButtonHover, 9),
				BackgroundNormal = new BackgroundTexture(GalaxyTextures.Get.ButtonNormal, 9),
				Padding = 6,
				Margins = new Vector2(0, 4),
				TextColor = Color.Black,
				TextHeight = 12,
				Text = textFor("UnitDone").Text(),
				ClickCallback = this.unitDone
			};
			this.doneButon.Position.FixedSize(80, 40).ParentRelative(0, 1).UseMargins().StretchBottomTo(this, -1);
			this.AddChild(this.doneButon);

			this.armorInfo = new GuiText
			{
				Margins = new Vector2(15, 4),
				TextColor = Color.Black,
				TextHeight = 12
			};
			this.armorInfo.Position.WrapContent().Then.RelativeTo(this.doneButon, 1, 1, -1, 1).UseMargins();
			this.AddChild(this.armorInfo);

			this.shieldsInfo = new GuiText
			{
				Margins = new Vector2(0, 4),
				TextColor = Color.Black,
				TextHeight = 12
			};
			this.shieldsInfo.Position.WrapContent().Then.RelativeTo(this.armorInfo, -1, -1, -1, 1).UseMargins();
			this.AddChild(this.shieldsInfo);
		}

		public void SetView(CombatantInfo unit, SpaceBattleController controller)
		{
			this.controller = controller;

			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			var formatter = new ThousandsFormatter();
			var decimalFormat = new DecimalsFormatter(0, 0);

			shipCount.Text = context["ShipCount"].Text() + ": " + formatter.Format(unit.Count);
			armorInfo.Text = hpText("ArmorLabel", unit.ArmorHp, unit.ArmorHpMax);
			shieldsInfo.Text = hpText("ShieldLabel", unit.ShieldHp, unit.ShieldHpMax);

			if (unit.MovementEta > 0)
				movementInfo.Text = context["MovementEta"].Text(
					new Var("eta", unit.MovementEta).Get,
					new TextVar("eta", unit.MovementEta.ToString()).Get
				);
			else
				movementInfo.Text = context["MovementPoints"].Text() + " (" + decimalFormat.Format(unit.MovementPoints * 100) + " %)";
		}

		private void unitDone()
		{
			this.controller.UnitDone();
		}

		private string hpText(string label, double x, double max)
		{
			var hpFormat = ThousandsFormatter.MaxMagnitudeFormat(x, max);
			return $"{textFor(label).Text()}: {hpFormat.Format(x)} / {hpFormat.Format(max)}";
		}

		private static IText textFor(string key) => LocalizationManifest.Get.CurrentLanguage["FormMain"][key];
	}
}
