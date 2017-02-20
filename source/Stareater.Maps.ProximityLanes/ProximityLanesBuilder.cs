using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using NGenerics.DataStructures.General;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy.Builders;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.ProximityLanes
{
	public class ProximityLanesBuilder : IStarConnector
	{
		const string MapsFolder = "./maps/"; //TODO(v0.6) try to move it to view
		const string ParametersFile = "proximityLanes.txt";

		const string LanguageContext = "ProximityLanes";
		const string DegreeKey = "Degree";
		const double Epsilon = 1e-9;

		private SelectorParameter degreesParameter;
		private DegreeOption[] degreeOptions;

		public ProximityLanesBuilder()
		{
			TaggableQueue<object, IkadnBaseObject> data;
			using (var parser = new IkonParser(new StreamReader(MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			degreesParameter = loadDegrees(data);
		}

		private SelectorParameter loadDegrees(TaggableQueue<object, IkadnBaseObject> data)
		{
			this.degreeOptions = new DegreeOption[data.CountOf(DegreeKey)];
			var parameterOptions = new Dictionary<int, string>();
			for (int i = 0; i < degreeOptions.Length; i++) {
				degreeOptions[i] = new DegreeOption(data.Dequeue(DegreeKey).To<IkonComposite>());
				parameterOptions.Add(i, degreeOptions[i].Name);
			}

			return new SelectorParameter(LanguageContext, DegreeKey, parameterOptions, (int)Math.Ceiling(parameterOptions.Count / 2.0));
		}

		public string Code
		{
			get { return "ProximityLanes"; }
		}
		
		public string Name
		{
			get { return LocalizationManifest.Get.CurrentLanguage[LanguageContext]["name"].Text(); }
		}

		public string Description
		{
			get
			{
				var vars = new TextVar(
					"minDegree", degreeOptions[degreesParameter.Value].Min.ToString()).And(
					"maxDegree", degreeOptions[degreesParameter.Value].Max.ToString());
				
				return LocalizationManifest.Get.CurrentLanguage[LanguageContext]["description"].Text(null, vars.Get);
			}
		}

		public IEnumerable<AParameterBase> Parameters
		{
			get { yield return degreesParameter; }
		}


		public IEnumerable<WormholeEndpoints> Generate(Random rng, StarPositions starPositions)
		{
			var maxGraph = new Graph<Vector2D>(false);
			var starIndex = new Dictionary<Vertex<Vector2D>, int>();
			for(int i = 0; i < starPositions.Stars.Length; i++)
			{
				var v = new Vertex<Vector2D>(starPositions.Stars[i]);
				maxGraph.AddVertex(v);
				starIndex[v] = i;
			}
			foreach(var edge in genMaxEdges(maxGraph.Vertices.ToList()))
				maxGraph.AddEdge(edge);
			
			return maxGraph.Edges.Select(e => new WormholeEndpoints(starIndex[e.FromVertex], starIndex[e.ToVertex])).ToList();
		}

		private IEnumerable<Edge<Vector2D>> genMaxEdges(IList<Vertex<Vector2D>> vertices)
		{
			var starEdges = new List<Edge<Vector2D>>();

			for (int i = 0; i < vertices.Count; i++)
			{
				for (int j = 0; j < vertices.Count; j++)
				{
					if (i == j) continue;
					var lane = vertices[j].Data - vertices[i].Data;
					var length = lane.Magnitude();

					var shadowedEdges = new List<Edge<Vector2D>>();
					var overshadowed = false;
					var duplicate = false;
					foreach (var edge in starEdges.Where(e => e.FromVertex == vertices[i] || e.ToVertex == vertices[i]))
					{
						var otherVertex = edge.FromVertex != vertices[i] ? edge.FromVertex : edge.ToVertex;
						var k = vertices.IndexOf(otherVertex);

						if (k == j)
						{
							duplicate = true;
							break;
						}

						var edgeLane = vertices[k].Data - vertices[i].Data;
						var edgeLength = edgeLane.Magnitude();

						var projection = lane.DotProduct(edgeLane) / length;

						if (projection > length)
							shadowedEdges.Add(edge);

						projection = edgeLane.DotProduct(lane) / edgeLength;
						overshadowed |= projection > edgeLength;
					}

					starEdges.RemoveAll(shadowedEdges.Contains);
					if (!overshadowed && !duplicate)
						starEdges.Add(new Edge<Vector2D>(vertices[i], vertices[j], false));
				}
			}

			return starEdges;
		}
	}
}
