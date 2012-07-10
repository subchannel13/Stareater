using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.GUI.Events;
using Zvjezdojedac.Igra.Bitka;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.GUI.SmallData;

namespace Zvjezdojedac.GUI
{
	public partial class CombatantPositions : UserControl
	{
		const int MaxPresenceFlags = 8;

		HashSet<Igrac> thisSidePlayers = new HashSet<Igrac>();
		Dictionary<int, ICollection<Borac>> fighersPerSlot = new Dictionary<int, ICollection<Borac>>();

		public CombatantPositions()
		{
			InitializeComponent();

			this.Interactive = true;
		}

		#region Properties
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DescriptionAttribute("Indicates whether control is interactive or readonly.")]
		[DefaultValue(true)]
		public bool Interactive { get; set; }

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DescriptionAttribute("Indicates whether ship positions should be from left to right (normal direction) or from right to left (reverse direction)")]
		[DefaultValue(false)]
		public bool ReverseDirection { get; set; }
		#endregion

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public event ObjectEventArgs<ICollection<Borac>>.Handler OnPositionClick;
		
		public void ThisSidePlayers(IEnumerable<Igrac> thisSidePlayers)
		{
			this.thisSidePlayers.Clear();
			this.thisSidePlayers.UnionWith(thisSidePlayers);
		}

		public void SetCombatants(IEnumerable<Borac> borci)
		{
			fighersPerSlot.Clear();

			Dictionary<int, Dictionary<Color, double>> playerPresence = new Dictionary<int, Dictionary<Color, double>>();
			for (int position = 0; position <= maxPosition; position++) {
				playerPresence.Add(position, new Dictionary<Color,double>());
				fighersPerSlot.Add(position, new List<Borac>());
			}

			HashSet<FuzzyCombatPosition> occupiedSlots = new HashSet<FuzzyCombatPosition>();

			foreach (Borac borac in borci) {
				FuzzyCombatPosition position = new FuzzyCombatPosition(borac, maxPosition, thisSidePlayers);
				if (position.slot < 0 || (position.slot == 0 && !thisSidePlayers.Contains(borac.Igrac)))
					continue;

				occupiedSlots.Add(position);
				if (!playerPresence[position.slot].ContainsKey(borac.Igrac.boja))
					playerPresence[position.slot].Add(borac.Igrac.boja, 0);
				playerPresence[position.slot][borac.Igrac.boja] += borac.Dizajn.trup.velicina;

				fighersPerSlot[position.slot].Add(borac);
			}

			for (int position = 0; position <= maxPosition; position++) {
				PictureBox pictureBox = ((ReverseDirection) ?
					Controls[maxPosition - position] :
					Controls[position]) as PictureBox;
				if (pictureBox.Image == null) pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
				Graphics g = Graphics.FromImage(pictureBox.Image);

				g.Clear(Color.Transparent);

				if (occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.StationaryLeft)))
					drawToTheLeft(g, Slike.BoracStacionarni, pictureBox.Width / 2, 0, false);

				if (occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.StationaryRigth)))
					drawFlipped(g, Slike.BoracStacionarni, pictureBox.Width / 2, 0);

				if (occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.LeftIncoming)) &&
					occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.LeftOutgoing)))
					g.DrawImageUnscaled(Slike.BoracMimoilazak, 0, 0);

				else if (occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.LeftIncoming)))
					g.DrawImageUnscaled(Slike.BoracJedanPomak, 0, 0);

				else if (occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.LeftOutgoing)))
					drawFlipped(g, Slike.BoracJedanPomak, 0, 0);

				if (occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.RightIncoming)) &&
					occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.RightOutgoing)))
					drawToTheLeft(g, Slike.BoracMimoilazak, pictureBox.Width, 0, false);

				else if (occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.RightIncoming)))
					drawToTheLeft(g, Slike.BoracJedanPomak, pictureBox.Width, 0, true);

				else if (occupiedSlots.Contains(new FuzzyCombatPosition(position, CombatSlotDelta.RightOutgoing)))
					drawToTheLeft(g, Slike.BoracJedanPomak, pictureBox.Width, 0, false);

				List<Color> flagColors = new List<Color>(playerPresence[position].Keys);
				flagColors.Sort((a, b) => playerPresence[position][a].CompareTo(playerPresence[position][b]));
				for (int i = 0, xOffset = 0; i < flagColors.Count && i < MaxPresenceFlags; i++) {
					Image flagImage = Slike.BoracZastavica[flagColors[i]];
					if (ReverseDirection)
						drawFlipped(g, flagImage, xOffset, 0);
					else
						g.DrawImageUnscaled(flagImage, xOffset, 0);
					xOffset += flagImage.Width;
				}

				g.Dispose();
				if (ReverseDirection)
					pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
				pictureBox.Refresh();
			}
		}

		private void drawToTheLeft(Graphics g, Image image, int x ,int y, bool flip)
		{
			if (flip)
				g.DrawImage(image, x, y, -image.Width, image.Height);
			else
				g.DrawImageUnscaled(image, x - image.Width, y);
		}

		private void drawFlipped(Graphics g, Image image, int x, int y)
		{
			g.DrawImage(image, x + image.Width, y, -image.Width, image.Height);
		}

		private int maxPosition
		{
			get { return Controls.Count - 1; }
		}

		private void positionClick(int positionIndex)
		{
			if (!Interactive || !Enabled)
				return;

			if (ReverseDirection)
				OnPositionClick(this, new ObjectEventArgs<ICollection<Borac>>(fighersPerSlot[maxPosition - positionIndex]));
			else
				OnPositionClick(this, new ObjectEventArgs<ICollection<Borac>>(fighersPerSlot[positionIndex]));
		}

		private void picPosition0_Click(object sender, EventArgs e)
		{
			positionClick(3);
		}

		private void picPosition1_Click(object sender, EventArgs e)
		{
			positionClick(2);
		}

		private void picPosition2_Click(object sender, EventArgs e)
		{
			positionClick(1);
		}

		private void picPosition3_Click(object sender, EventArgs e)
		{
			positionClick(0);
		}
	}

	class PlayerPresence : IComparable<PlayerPresence>
	{
		public int PlayerId { get; private set; }
		public double Presence { get; private set; }

		public PlayerPresence(int playerId)
		{
			this.PlayerId = playerId;
		}

		public void Increase(double presenceDelta)
		{
			Presence += presenceDelta;
		}

		public int CompareTo(PlayerPresence other)
		{
			return this.Presence.CompareTo(other.Presence);
		}
	}
}
