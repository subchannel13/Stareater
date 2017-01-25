using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Stareater.Controllers;
using Stareater.Controllers.Views.Combat;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Utils;
using Stareater.Utils.NumberFormatters;
using Stareater.GLData;
using Stareater.GLData.OrbitShader;
using Stareater.GraphicsEngine;
using Stareater.GLData.SpriteShader;

namespace Stareater.GLRenderers
{
	class SpaceCombatRenderer : AScene
	{
		private const float FarZ = 1;
		private const float Layers = 16.0f;
		
		private const float CellBackgroundZ = 6 / Layers;
		private const float GridZ = 5 / Layers;
		private const float PlanetColorZ = 4 / Layers;
		private const float StarColorZ = 4 / Layers;
		private const float CombatantZ = 3 / Layers;
		private const float MoreCombatantsZ = 2 / Layers;
		private const float MovemenentZ = 1 / Layers;
		
		private const float DefaultViewSize = 17;
		private const float HexHeightScale = 0.9f;
		private static readonly float HexHeight = (float)Math.Sqrt(3) * HexHeightScale;
		private static readonly Matrix4 PopulationTransform = Matrix4.CreateScale(0.2f, 0.2f, 1) * Matrix4.CreateTranslation(0.5f, -0.5f, 0);
		
		private const double AnimationPeriod = 1.5;
		private static readonly Color SelectionColor = Color.Yellow;
		
		private double animationTime = 0;
		private OrbitDrawable gridDrawable = null;
		private BatchDrawable<SpriteDrawable, SpriteData> bodySprites = null;
		private BatchDrawable<SpriteDrawable, SpriteData> unitSprites = null;
		
		private CombatantInfo currentUnit = null;
		private int currentUnitIndex = 0;
		
		public SpaceBattleController Controller { get; private set; }

		public SpaceCombatRenderer()
		{
			this.bodySprites = new BatchDrawable<SpriteDrawable, SpriteData>(
				ShaderLibrary.Sprite,
				(vao, i, data) => new SpriteDrawable(vao, i, data));

			this.unitSprites = new BatchDrawable<SpriteDrawable, SpriteData>(
				ShaderLibrary.Sprite,
				(vao, i, data) => new SpriteDrawable(vao, i, data));
		}
		
		public AbilityInfo SelectedAbility { private get; set; }
		
		public void StartCombat(SpaceBattleController controller)
		{
			this.Controller = controller;
			this.ResetLists();
		}
		
		#region ARenderer implementation
		protected override void FrameUpdate(double deltaTime)
		{
			this.animationTime += deltaTime;
			
			double animationPhase = Methods.GetPhase(this.animationTime, AnimationPeriod);
			var alpha = Math.Abs(animationPhase - 0.5) * 0.6 + 0.4;
			if (this.Controller.Units.Select(x => x.Owner).Distinct().All(x => this.currentUnit.CloakedFor(x) || x == currentUnit.Owner))
				alpha *= 0.65;
			this.unitSprites[this.currentUnitIndex].ObjectData.Color = new Color4(this.currentUnit.Owner.Color.R, this.currentUnit.Owner.Color.G, this.currentUnit.Owner.Color.B, (byte)(alpha * 255));
			
			this.gridDrawable.Draw(this.projection);
			this.bodySprites.Draw(this.projection);
			this.unitSprites.Draw(this.projection);
		}
		
		//TODO(0.6) refactor and remove
		public void ResetLists()
		{
			this.setupGrid();
			this.setupBodies();
			this.setupUnits();
		}
		
		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2());
		}
		#endregion
		
		#region Mouse events
		public override void OnMouseClick(MouseEventArgs e)
		{
			Vector4 mousePoint = Vector4.Transform(mouseToView(e.X, e.Y), invProjection);
			
			var hexX = Math.Round(mousePoint.X / 1.5, MidpointRounding.AwayFromZero);
			var hex = new NGenerics.DataStructures.Mathematical.Vector2D(
				hexX,
				Math.Round(mousePoint.Y / HexHeight - ((int)Math.Abs(hexX) % 2 != 0 ? 0.5 : 0), MidpointRounding.AwayFromZero)
			);
			
			var enemies = this.Controller.Units.Where(x => x.Position == hex && x.Owner != this.currentUnit.Owner).ToList();
			if (enemies.Any() && this.SelectedAbility != null)
				this.Controller.UseAbility(this.SelectedAbility, biggestStack(enemies));
			else if (this.Controller.Planets.Any(x => x.Position == hex && x.Owner != this.currentUnit.Owner) && this.SelectedAbility != null)
				this.Controller.UseAbility(this.SelectedAbility, this.Controller.Planets.First(x => x.Position == hex));
			else
				this.Controller.MoveTo(hex);
			
			this.ResetLists();
		}
		#endregion
		
		public void OnUnitTurn(CombatantInfo unitInfo)
		{
			this.currentUnit = unitInfo;
			this.SelectedAbility = unitInfo.Abilities.FirstOrDefault(x => x.Quantity > 0);
			this.setupUnits();
		}
		
		public void OnUnitDone()
		{
			this.Controller.UnitDone();
		}
		
		private void setupBodies()
		{
			var batchData = new List<SpriteData>();
			var vaoBuilder = new VertexArrayBuilder();
			var formatter = new ThousandsFormatter();
			
			vaoBuilder.BeginObject();
			vaoBuilder.AddTexturedRect(GalaxyTextures.Get.SystemStar.Texture);
			vaoBuilder.EndObject();
			batchData.Add(new SpriteData(Matrix4.Identity, StarColorZ, GalaxyTextures.Get.StarColor.Texture.Id, this.Controller.Star.Color));
			
			foreach(var planet in this.Controller.Planets)
			{
				var planetTransform = Matrix4.CreateTranslation(hexX(planet.Position), hexY(planet.Position), 0);
				SpriteInfo planetTexture = null;
				
				switch(planet.Type)
				{
					case PlanetType.Asteriod:
						planetTexture = GalaxyTextures.Get.Asteroids;
						break;
					case PlanetType.GasGiant:
						planetTexture = GalaxyTextures.Get.GasGiant;
						break;
					case PlanetType.Rock:
						planetTexture = GalaxyTextures.Get.RockPlanet;
						break;
				}
				
				vaoBuilder.BeginObject();
				vaoBuilder.AddTexturedRect(planetTexture.Texture); //FIXME sometimes bugs out
				vaoBuilder.EndObject();
				batchData.Add(new SpriteData(
					planetTransform, 
					PlanetColorZ,
					planetTexture.Texture.Id,
					Color.White));
				
				if (planet.Population > 0)
				{
					vaoBuilder.BeginObject();
					TextRenderUtil.Get.BufferText(formatter.Format(planet.Population), -1, Matrix4.Identity, vaoBuilder);
					vaoBuilder.EndObject();
					
					batchData.Add(new SpriteData(
						PopulationTransform * planetTransform, 
						MoreCombatantsZ,
						TextRenderUtil.Get.TextureId,
						planet.Owner != null ? planet.Owner.Color : Color.Gray));
				}
			}

			this.bodySprites.Update(vaoBuilder, batchData);
		}
		
		private void setupGrid()
		{
			if (this.gridDrawable != null)
				return;
			
			var vaoBuilder = new VertexArrayBuilder();
			vaoBuilder.BeginObject();
			
			for(int x = -SpaceBattleController.BattlefieldRadius; x <= SpaceBattleController.BattlefieldRadius; x++)
			{
				int yHeight = (SpaceBattleController.BattlefieldRadius * 2 - Math.Abs(x));
				var yOffset = Math.Abs(x) % 2 != 0 ? HexHeight / 2 : 0;
					
				for(int y = -(int)Math.Ceiling(yHeight / 2.0); y <= (int)Math.Floor(yHeight / 2.0); y++)
				{
					var nearPoints = new List<Vector2>();
					var farPoints = new List<Vector2>();
					for(int i = 0; i < 6; i++)
					{
						var cos = (float)Math.Cos(i * Math.PI / 3);
						var sin = (float)Math.Sin(i * Math.PI / 3);
						nearPoints.Add(new Vector2(0.95f * cos + x * 1.5f, 0.95f * sin * HexHeightScale + y * HexHeight + yOffset));
						farPoints.Add(new Vector2(1.05f * cos + x * 1.5f, 1.05f * sin * HexHeightScale + y * HexHeight + yOffset));
					}
					
					for(int i = 0; i < 6; i++)
					{
						var j = (i + 1) % 6;
						vaoBuilder.AddFlatOrbitVertex(nearPoints[j].X, nearPoints[j].Y);
						vaoBuilder.AddFlatOrbitVertex(farPoints[j].X, farPoints[j].Y);
						vaoBuilder.AddFlatOrbitVertex(farPoints[i].X, farPoints[i].Y);
						
						vaoBuilder.AddFlatOrbitVertex(farPoints[i].X, farPoints[i].Y);
						vaoBuilder.AddFlatOrbitVertex(nearPoints[i].X, nearPoints[i].Y);
						vaoBuilder.AddFlatOrbitVertex(nearPoints[j].X, nearPoints[j].Y);
					}
				}
			}
			vaoBuilder.EndObject();
			
			this.gridDrawable = new OrbitDrawable(
				vaoBuilder.Generate(ShaderLibrary.PlanetOrbit),
				0,
				new OrbitData(
					GridZ,
					0, 1,
					Color.Green,
					Matrix4.Identity)
			);
		}
		
		private void setupUnits()
		{
			var batchData = new List<SpriteData>();
			var vaoBuilder = new VertexArrayBuilder();
			
			var units = this.Controller.Units.GroupBy(x => x.Position);
			var players = this.Controller.Units.Select(x => x.Owner).Distinct();
			var formatter = new ThousandsFormatter();
			
			foreach(var hex in units)
			{
				var hexTransform = Matrix4.CreateTranslation(hexX(hex.Key), hexY(hex.Key), 0);
				
				var unitSelected = (this.currentUnit != null && this.currentUnit.Position == hex.Key);
				var unit = unitSelected ? this.currentUnit : biggestStack(hex);
				var unitSprite = GalaxyTextures.Get.Sprite(unit.Design.ImagePath);
				var alpha = players.All(x => unit.CloakedFor(x) || x == unit.Owner) ? 0.65 : 1;
				
				if (unitSelected)
					currentUnitIndex = batchData.Count;
				
				vaoBuilder.BeginObject();
				vaoBuilder.AddTexturedRect(unitSprite.Texture);
				vaoBuilder.EndObject();
				batchData.Add(new SpriteData(
					hexTransform, 
					CombatantZ, 
					unitSprite.Texture.Id, 
					Color.FromArgb((int)(alpha * 255), unit.Owner.Color)));
				
				var otherUnits = hex.Where(x => x != unit).Select(x => x.Owner).Distinct().ToList();
				for(int i = 0; i < otherUnits.Count; i++)
				{
					vaoBuilder.BeginObject();
					vaoBuilder.AddTexturedRect(GalaxyTextures.Get.FleetIndicator.Texture);
					vaoBuilder.EndObject();
					
					batchData.Add(new SpriteData(
						Matrix4.CreateScale(0.2f, 0.2f, 1) * Matrix4.CreateTranslation(0.5f, 0.2f * i + 0.5f, 0) * hexTransform, 
						MoreCombatantsZ, 
						GalaxyTextures.Get.FleetIndicator.Texture.Id, 
						otherUnits[i].Color));
				}
				
				vaoBuilder.BeginObject();
				TextRenderUtil.Get.BufferText(formatter.Format(unit.Count), -1, Matrix4.Identity, vaoBuilder);
				vaoBuilder.EndObject();
				
				batchData.Add(new SpriteData(
					Matrix4.CreateScale(0.2f, 0.2f, 1) * Matrix4.CreateTranslation(0.5f, -0.5f, 0) * hexTransform, 
					MoreCombatantsZ,
					TextRenderUtil.Get.TextureId,
					Color.Gray));
			}
			
			if (this.currentUnit != null)
				setupValidMoves(vaoBuilder, batchData);
			
			this.unitSprites.Update(vaoBuilder, batchData);
		}
		
		private void setupValidMoves(VertexArrayBuilder vaoBuilder, ICollection<SpriteData> batchData)
		{
			var center = new Vector2(hexX(this.currentUnit.Position), hexY(this.currentUnit.Position));
			foreach(var move in this.currentUnit.ValidMoves)
			{
				var moveTransform = Matrix4.CreateTranslation(hexX(move), hexY(move), 0);
				
				var direction = new Vector2(hexX(move), hexY(move)) - center;
				if (direction.LengthSquared > 0)
				{
					direction.Normalize();
					moveTransform = new Matrix4(
						direction.X, direction.Y, 0, 0,
						direction.Y, -direction.X, 0, 0,
						0, 0, 1, 0,
						0, 0, 0, 1
					) * moveTransform;
				}
				
				vaoBuilder.BeginObject();
				vaoBuilder.AddTexturedRect(GalaxyTextures.Get.MoveToArrow.Texture);
				vaoBuilder.EndObject();
				batchData.Add(new SpriteData(
					Matrix4.CreateScale(0.4f, 0.4f, 1) * Matrix4.CreateTranslation(-0.25f, 0, 0) * moveTransform, 
					MovemenentZ, 
					GalaxyTextures.Get.MoveToArrow.Texture.Id, 
					Methods.HexDistance(move) <= SpaceBattleController.BattlefieldRadius ? Color.Green : Color.White));
			}
		}
		
		#region Helper methods
		private CombatantInfo biggestStack(IEnumerable<CombatantInfo> combatants)
		{
			return combatants.Aggregate((a, b) => a.Count * a.Design.Size > b.Count * b.Design.Size ? a : b);
		}

		private static float hexX(NGenerics.DataStructures.Mathematical.Vector2D coordinate)
		{
			return (float)(coordinate.X * 1.5);
		}
		
		private static float hexY(NGenerics.DataStructures.Mathematical.Vector2D coordinate)
		{
			return (float)(HexHeight * (coordinate.Y + ((int)Math.Abs(coordinate.X) % 2 != 0 ? 0.5 : 0)));
		}
		#endregion
	}
}
