using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;

namespace Stareater.GLRenderers
{
	class SpaceCombatRenderer : ARenderer
	{
		private const double DefaultViewSize = 20;
		private const double HexHeightScale = 0.9;
		
		private const float FarZ = -1;
		private const float Layers = 16.0f;
		
		private const float CellBackgroundZ = -4 / Layers;
		private const float GridZ = -3 / Layers;
		private const float StarColorZ = -2 / Layers;
		private const float CombatantZ = -1 / Layers;
		
		private Matrix4 invProjection;
		private int gridList = NoCallList;
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
			
			drawBackgrounds();
			drawList(gridList, setupGrid);
			drawBodies();
			drawUnits();
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
			
			int x = this.currentUnit.X;
			int y = this.currentUnit.Y;
			double yDist = Math.Sqrt(3) * HexHeightScale;
			double yOffset = Math.Abs(x) % 2 != 0 ? yDist / 2 : 0;
			
			GL.Disable(EnableCap.Texture2D);
			GL.Color4(Color.Yellow);
			GL.Begin(PrimitiveType.TriangleFan);
			
			GL.Vertex3(x * 1.5, y * yDist + yOffset, CellBackgroundZ);
			for(int i = 0; i <= 6; i++)
				GL.Vertex3(Math.Cos(i * Math.PI / 3) + x * 1.5, Math.Sin(i * Math.PI / 3) * HexHeightScale + y * yDist + yOffset, CellBackgroundZ);
			GL.End();
		}
		
		private void drawBodies()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.Color4(Controller.Star.Color);
			TextureUtils.DrawSprite(GalaxyTextures.Get.SystemStar, StarColorZ);
			
			//TODO(v0.5) draw planets
		}
		
		void drawUnits()
		{
			double yDist = Math.Sqrt(3) * HexHeightScale;
			foreach(var unit in this.Controller.Units)
			{
				GL.PushMatrix();
				GL.Translate(unit.X * 1.5, yDist * (unit.Y + (Math.Abs(unit.X) % 2 != 0 ? 0.5 : 0)), CombatantZ);
				GL.Color4(unit.Owner.Color); //TODO(v0.5) color units
				
				TextureUtils.DrawSprite(GalaxyTextures.Get.Sprite(unit.Design.ImagePath));
				GL.PopMatrix();
			}
		}
		
		private void setupGrid()
		{
			this.gridList = GL.GenLists(1);
			GL.NewList(gridList, ListMode.CompileAndExecute);
			
			GL.Disable(EnableCap.Texture2D);
			GL.Color4(Color.Green);
			
			double yDist = Math.Sqrt(3) * HexHeightScale;
			for(int x = -SpaceBattleController.BattlefieldRadius; x <= SpaceBattleController.BattlefieldRadius; x++)
			{
				int yHeight = (SpaceBattleController.BattlefieldRadius * 2 - Math.Abs(x));
				double yOffset = Math.Abs(x) % 2 != 0 ? yDist / 2 : 0;
					
				for(int y = -(int)Math.Ceiling(yHeight / 2.0); y <= (int)Math.Floor(yHeight / 2.0); y++)
				{
					GL.Begin(PrimitiveType.TriangleStrip);
					for(int i = 0; i <= 6; i++)
					{
						GL.Vertex3(0.95 * Math.Cos(i * Math.PI / 3) + x * 1.5, 0.95 * Math.Sin(i * Math.PI / 3) * HexHeightScale + y * yDist + yOffset, GridZ);
						GL.Vertex3(1.05 * Math.Cos(i * Math.PI / 3) + x * 1.5, 1.05 * Math.Sin(i * Math.PI / 3) * HexHeightScale + y * yDist + yOffset, GridZ);
					}
					GL.End();
				}
			}
			
			GL.EndList();
		}
	}
}
