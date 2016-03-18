using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Controllers.Views.Ships;
using Stareater.Utils;

namespace Stareater.GLRenderers
{
	class SpaceCombatRenderer : ARenderer
	{
		private const double DefaultViewSize = 20;
		private const double HexHeightScale = 0.9;
		private static readonly double HexHeight = Math.Sqrt(3) * HexHeightScale;
		
		private const float FarZ = -1;
		private const float Layers = 16.0f;
		
		private const float CellBackgroundZ = -6 / Layers;
		private const float GridZ = -5 / Layers;
		private const float StarColorZ = -4 / Layers;
		private const float CombatantZ = -3 / Layers;
		private const float MoreCombatantsZ = -2 / Layers;
		private const float MovemenentZ = -1 / Layers;
		
		private const double AnimationPeriod = 3;
		private static readonly Color SelectionColor = Color.Yellow;
		
		private Matrix4 invProjection;
		private int gridList = NoCallList;
		private double animationTime = 0;
		private CombatantInfo currentUnit = null;
		
		public SpaceBattleController Controller { get; private set; }

		public AbilityInfo SelectedAbility { private get; set; }
		
		public void StartCombat(SpaceBattleController controller)
		{
			this.Controller = controller;
			this.ResetLists();
			this.ResetProjection();
		}
		
		#region ARenderer implementation
		public override void Draw(double deltaTime)
		{
			base.checkPerspective();
			this.animationTime += deltaTime;
			
			drawBackgrounds();
			drawList(gridList, setupGrid);
			drawBodies();
			drawUnits();
			drawValidMoves();
		}
		
		protected override void attachEventHandlers()
		{
			this.eventDispatcher.MouseClick += this.mouseClick;
		}
		
		protected override void detachEventHandlers()
		{
			this.eventDispatcher.MouseClick -= this.mouseClick;
		}
		
		public override void Load()
		{
			GalaxyTextures.Get.Load();
		}
		
		public override void ResetLists()
		{
			GL.DeleteLists(gridList, 1);
			this.gridList = NoCallList;
		}
		
		protected override void setupPerspective()
		{
			double aspect = eventDispatcher.Width / (double)eventDispatcher.Height;
			const double semiRadius = 0.5 * DefaultViewSize;

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(
				-aspect * semiRadius, aspect * semiRadius,
				-semiRadius, semiRadius, 
				0, -FarZ);

			GL.GetFloat(GetPName.ProjectionMatrix, out invProjection);
			invProjection.Invert();
			GL.MatrixMode(MatrixMode.Modelview);
		}
		#endregion
		
		#region Mouse events
		private void mouseClick(object sender, MouseEventArgs e)
		{
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			
			var x = Math.Round(mousePoint.X / 1.5, MidpointRounding.AwayFromZero);
			var hex = new NGenerics.DataStructures.Mathematical.Vector2D(
				x,
				Math.Round(mousePoint.Y / HexHeight - ((int)Math.Abs(x) % 2 != 0 ? 0.5 : 0), MidpointRounding.AwayFromZero)
			);
			
			this.Controller.MoveTo(hex);
		}
		#endregion
		
		public void OnUnitTurn(CombatantInfo unitInfo)
		{
			this.currentUnit = unitInfo;
		}

		private void drawBackgrounds()
		{
			if (this.currentUnit == null)
				return;
			
			double x = hexX(this.currentUnit.Position);
			double y = hexY(this.currentUnit.Position);
			double animationPhase = Methods.GetPhase(this.animationTime, AnimationPeriod);
				
			var color = new double[] {
				SelectionColor.R, SelectionColor.G, SelectionColor.B,
				Math.Abs(animationPhase - 0.5) * 0.8 + 0.2};
			
			GL.Disable(EnableCap.Texture2D);
			GL.Color4(color);
			GL.Begin(PrimitiveType.TriangleFan);
			
			GL.Vertex3(x, y, CellBackgroundZ);
			for(int i = 0; i <= 6; i++)
				GL.Vertex3(Math.Cos(i * Math.PI / 3) + x, Math.Sin(i * Math.PI / 3) * HexHeightScale + y, CellBackgroundZ);
			GL.End();
		}
		
		private void drawBodies()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.Color4(Controller.Star.Color);
			TextureUtils.DrawSprite(GalaxyTextures.Get.SystemStar, StarColorZ);
			
			//TODO(v0.5) draw planets
		}
		
		private void drawUnits()
		{
			var units = this.Controller.Units.GroupBy(x => x.Position);
			
			foreach(var hex in units)
			{
				var unit = hex.First(); //TODO(v0.5) select most prominent unit
				
				GL.PushMatrix();
				GL.Translate(hexX(hex.Key), hexY(hex.Key), CombatantZ);
				GL.Color4(unit.Owner.Color); //TODO(v0.5) color units
				
				TextureUtils.DrawSprite(GalaxyTextures.Get.Sprite(unit.Design.ImagePath));
				
				var otherUnits = hex.Where(x => x != unit).Select(x => x.Owner).Distinct().ToList();
				for(int i = 0; i < otherUnits.Count; i++)
				{
					GL.PushMatrix();
					GL.Translate(0.5, 0.2 * i + 0.5, MoreCombatantsZ - CombatantZ);
					GL.Scale(0.2, 0.2, 1);
					GL.Color4(otherUnits[i].Color);
					
					TextureUtils.DrawSprite(GalaxyTextures.Get.FleetIndicator);
					GL.PopMatrix();
				}
				GL.PopMatrix();
			}
		}

		private void drawValidMoves()
		{
			if (this.currentUnit == null)
				return;
			
			var center = new Vector2d(hexX(this.currentUnit.Position), hexY(this.currentUnit.Position));
			foreach(var move in this.currentUnit.ValidMoves)
			{
				GL.PushMatrix();
				GL.Translate(hexX(move), hexY(move), MovemenentZ);
				
				var direction = new Vector2d(hexX(move), hexY(move)) - center;
				if (direction.LengthSquared > 0)
				{
					direction.Normalize();
					GL.MultMatrix(new double[] {
						direction.X, direction.Y, 0, 0,
						direction.Y, -direction.X, 0, 0,
						0, 0, 1, 0,
						0, 0, 0, 1
					});
				}
				GL.Translate(-0.25, 0, 0);
				GL.Scale(0.4, 0.4, 1);
				
				GL.Color4(Methods.InsideHexGrid(move, SpaceBattleController.BattlefieldRadius) ? Color.Green : Color.White);
				
				TextureUtils.DrawSprite(GalaxyTextures.Get.MoveToArrow);
				GL.PopMatrix();
			}
		}
		
		private void setupGrid()
		{
			this.gridList = GL.GenLists(1);
			GL.NewList(gridList, ListMode.CompileAndExecute);
			
			GL.Disable(EnableCap.Texture2D);
			GL.Color4(Color.Green);
			
			for(int x = -SpaceBattleController.BattlefieldRadius; x <= SpaceBattleController.BattlefieldRadius; x++)
			{
				int yHeight = (SpaceBattleController.BattlefieldRadius * 2 - Math.Abs(x));
				double yOffset = Math.Abs(x) % 2 != 0 ? HexHeight / 2 : 0;
					
				for(int y = -(int)Math.Ceiling(yHeight / 2.0); y <= (int)Math.Floor(yHeight / 2.0); y++)
				{
					GL.Begin(PrimitiveType.TriangleStrip);
					for(int i = 0; i <= 6; i++)
					{
						GL.Vertex3(0.95 * Math.Cos(i * Math.PI / 3) + x * 1.5, 0.95 * Math.Sin(i * Math.PI / 3) * HexHeightScale + y * HexHeight + yOffset, GridZ);
						GL.Vertex3(1.05 * Math.Cos(i * Math.PI / 3) + x * 1.5, 1.05 * Math.Sin(i * Math.PI / 3) * HexHeightScale + y * HexHeight + yOffset, GridZ);
					}
					GL.End();
				}
			}
			
			GL.EndList();
		}
		
		#region Helper methods
		private Vector4 mouseToView(int x, int y)
		{
			return new Vector4(
				2 * x / (float)this.eventDispatcher.Width - 1,
				1 - 2 * y / (float)this.eventDispatcher.Height, 
				0, 1
			);
		}
		
		private static double hexX(NGenerics.DataStructures.Mathematical.Vector2D coordinate)
		{
			return coordinate.X * 1.5;
		}
		
		private static double hexY(NGenerics.DataStructures.Mathematical.Vector2D coordinate)
		{
			return HexHeight * (coordinate.Y + ((int)Math.Abs(coordinate.X) % 2 != 0 ? 0.5 : 0));
		}
		#endregion
	}
}
