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
using NGenerics.DataStructures.Queues;
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
				return LocalizationManifest.Get.CurrentLanguage[LanguageContext]["description"].Text(degreesParameter.Value);
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
			var homeNodes = new List<Vertex<Vector2D>>();
			for(int i = 0; i < starPositions.Stars.Length; i++)
			{
				var vertex = new Vertex<Vector2D>(starPositions.Stars[i]);
				maxGraph.AddVertex(vertex);
				starIndex[vertex] = i;
				if (starPositions.HomeSystems.Contains(i))
					homeNodes.Add(vertex);
			}
			foreach(var edge in genMaxEdges(maxGraph.Vertices.ToList()))
				maxGraph.AddEdge(edge);
			
			var tree = genMinEdges(maxGraph, homeNodes).ToList();
			var extraEdgeCount = (int)((maxGraph.Edges.Count - tree.Count) * this.degreeOptions[this.degreesParameter.Value].Ratio);
			
			return (extraEdgeCount == 0) ?
				tree.Select(e => new WormholeEndpoints(starIndex[e.FromVertex], starIndex[e.ToVertex])).ToList() :
				maxGraph.Edges.Select(e => new WormholeEndpoints(starIndex[e.FromVertex], starIndex[e.ToVertex])).ToList();
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
		
		private IEnumerable<Edge<Vector2D>> genMinEdges(Graph<Vector2D> graph, IEnumerable<Vertex<Vector2D>> homeNodes)
		{
			var centroid = graph.Vertices.Aggregate(new Vector2D(0, 0), (subsum, vertex) => subsum + vertex.Data) / graph.Vertices.Count;
			var centralNode = graph.Vertices.Aggregate((a, b) => ((a.Data - centroid).Magnitude() < (b.Data - centroid).Magnitude()) ? a : b);
				
			var criticalNodes = new HashSet<Vertex<Vector2D>>();
			criticalNodes.Add(centralNode);
			foreach(var home in homeNodes)
			{
				var current = centralNode;
				foreach(var node in Astar(graph, home, centralNode))
				{
					criticalNodes.Add(node);
					yield return node.GetIncidentEdgeWith(current);
					current = node;
				}
			}
			
			var treeNodes = new HashSet<Vertex<Vector2D>>(criticalNodes);
			var potentialEdges = new Dictionary<Edge<Vector2D>, double>();
			var critPathDistance = new Dictionary<Vertex<Vector2D>, double>();
			foreach(var node in criticalNodes)
				critPathDistance[node] = 0;
			
			while(treeNodes.Count < graph.Vertices.Count)
			{
				potentialEdges.Clear();
				foreach(var node in treeNodes)
				{
					var degree = node.IncidentEdges.Count(e => treeNodes.Contains(e.FromVertex) && treeNodes.Contains(e.ToVertex));
					
					foreach(var edge in node.IncidentEdges)
					{
						if (treeNodes.Contains(edge.FromVertex) && treeNodes.Contains(edge.ToVertex))
							continue;
						
						potentialEdges[edge] = (edge.FromVertex.Data - edge.ToVertex.Data).Magnitude() + degree *0 + critPathDistance[node];
					}
				}
				
				var currentEdge = potentialEdges.Aggregate((a, b) => a.Value < b.Value ? a : b).Key;
				yield return currentEdge;
				
				var fromNode = treeNodes.Contains(currentEdge.FromVertex) ? currentEdge.FromVertex : currentEdge.ToVertex;
				var toNode = treeNodes.Contains(currentEdge.FromVertex) ? currentEdge.ToVertex : currentEdge.FromVertex;
				var length = (currentEdge.FromVertex.Data - currentEdge.ToVertex.Data).Magnitude();
				
				treeNodes.Add(toNode);
				critPathDistance[toNode] = critPathDistance[fromNode] + length;
			}
		}
		
		private IEnumerable<Vertex<Vector2D>> Astar(Graph<Vector2D> graph, Vertex<Vector2D> fromNode, Vertex<Vector2D> toNode)
		{
			var cameFrom = new Dictionary<Vertex<Vector2D>, Vertex<Vector2D>>();
			var closedSet = new HashSet<Vertex<Vector2D>>();
			var openSet = new PriorityQueue<Vertex<Vector2D>, double>(PriorityQueueType.Minimum);
			openSet.Enqueue(fromNode, (fromNode.Data - toNode.Data).Magnitude());
			
			var gScore = graph.Vertices.ToDictionary(x => x, x => double.PositiveInfinity);
			gScore[fromNode] = 0;
			Vertex<Vector2D> current;
		
		    while (openSet.Count > 0)
		    {
		    	current = openSet.Dequeue();
		    	if (current == toNode)
		    		break;
		
		    	openSet.Remove(current);
		    	closedSet.Add(current);
		    	foreach(var neighbor in current.IncidentEdges.SelectMany(e => new [] {e.FromVertex, e.ToVertex}.Where(v => v != current)))
		    	{
		    		if (closedSet.Contains(neighbor))
		    			continue;
		    		
		            var tentative_gScore = gScore[current] + (current.Data - neighbor.Data).Magnitude();
		            if (!openSet.Contains(neighbor))
		            	openSet.Add(neighbor, tentative_gScore);
		            else if (tentative_gScore >= gScore[neighbor])
		            	continue;

		            cameFrom[neighbor] = current;
		            gScore[neighbor] = tentative_gScore;
		    	}
		    }
		    
		    current = toNode;
		    while (cameFrom.ContainsKey(current))
			{
				current = cameFrom[current];
				yield return current;
			}
		}
	}
}
