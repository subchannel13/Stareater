using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
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
		
		private const float CellBackgroundZ = -4 / Layers;
		private const float GridZ = -3 / Layers;
		private const float StarColorZ = -2 / Layers;
		private const float CombatantZ = -1 / Layers;
		
		private const double AnimationPeriod = 3;
		private static readonly Color SelectionColor = Color.Yellow;
		
		private Matrix4 invProjection;
		private int gridList = NoCallList;
		private double animationTime = 0;
		private CombatantInfo currentUnit = null;
		
		public SpaceBattleController Controller { get; private set; }

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
			//no op
		}
		
		protected override void detachEventHandlers()
		{
			//no op
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
			foreach(var unit in this.Controller.Units)
			{
				GL.PushMatrix();
				GL.Translate(hexX(unit.Position), hexY(unit.Position), CombatantZ);
				GL.Color4(unit.Owner.Color); //TODO(v0.5) color units
				
				TextureUtils.DrawSprite(GalaxyTextures.Get.Sprite(unit.Design.ImagePath));
				GL.PopMatrix();
			}
		}

		private void drawValidMoves()
		{
			var center = new Vector2d(hexX(this.currentUnit.Position), hexY(this.currentUnit.Position));
			foreach(var move in this.Controller.ValidMoves)
			{
				GL.PushMatrix();
				GL.Translate(hexX(move), hexY(move), CombatantZ);
				
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
				
				GL.Color4(insideMap(move) ? Color.Green : Color.White);
				
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
		
		private static double hexX(NGenerics.DataStructures.Mathematical.Vector2D coordinate)
		{
			return coordinate.X * 1.5;
		}
		
		private static double hexY(NGenerics.DataStructures.Mathematical.Vector2D coordinate)
		{
			return HexHeight * (coordinate.Y + ((int)Math.Abs(coordinate.X) % 2 != 0 ? 0.5 : 0));
		}

		//TODO(v0.5) unify with similar check in SpaceBattleProcessor.correctPosition
		private static bool insideMap(NGenerics.DataStructures.Mathematical.Vector2D coordinate)
		{
			double yHeight = (SpaceBattleController.BattlefieldRadius * 2 - Math.Abs(coordinate.X));
			
			return Math.Abs(coordinate.X) <= SpaceBattleController.BattlefieldRadius &&
				coordinate.Y >= -Math.Ceiling(yHeight / 2.0) &&
				coordinate.Y <= Math.Floor(yHeight / 2.0);
		}
	}
}
