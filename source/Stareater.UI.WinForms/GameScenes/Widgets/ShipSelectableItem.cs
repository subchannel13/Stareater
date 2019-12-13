using OpenTK;
using Stareater.Controllers.Views.Ships;
using Stareater.GraphicsEngine.GuiElements;
using System;
using System.Windows.Forms;

namespace Stareater.GameScenes.Widgets
{
	class ShipSelectableItem : AMapSelectableItem<ShipGroupInfo>
	{
		private bool mIsSelected = false;

		public bool IsSelected
		{
			get => this.mIsSelected;
			set => this.apply(ref this.mIsSelected, value);
		}

		public ShipSelectableItem(ShipGroupInfo data) : base(data)
		{ }

		public Action<ShipSelectableItem> OnSelect { get; set; }
		public Action<ShipSelectableItem> OnDeselect { get; set; }
		public Action<ShipSelectableItem> OnSplit { get; set; }

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			this.apply(ref this.mIsSelected, !this.mIsSelected);

			return true;
		}

		public override void OnMouseDownCanceled()
		{
			this.apply(ref this.mIsSelected, !this.mIsSelected);
		}

		public override void OnMouseUp(Keys modiferKeys)
		{
			if (modiferKeys == Keys.Shift)
				this.OnSplit?.Invoke(this);
			else if (this.mIsSelected)
				this.OnSelect?.Invoke(this);
			else
				this.OnDeselect?.Invoke(this);
		}

		protected override BackgroundTexture backgroundTexture()
		{
			if (this.isHovered)
				return this.backgroundHover;
			if (this.mIsSelected)
				return this.backgroundSelected;

			return this.backgroundUnselected;
		}
	}
}
