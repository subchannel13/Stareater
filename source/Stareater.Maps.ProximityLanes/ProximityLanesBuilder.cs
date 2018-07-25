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
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.ProximityLanes
{
	public class ProximityLanesBuilder : IStarConnector
	{
		const string ParametersFile = "proximityLanes.txt";

		const string LanguageContext = "ProximityLanes";
		const string DegreeKey = "Degree";
		const double Epsilon = 1e-9;
		const double MinimalAngleCos = 0.90;

		private SelectorParameter degreesParameter;
		private DegreeOption[] degreeOptions;

		public void Initialize(string dataPath)
		{
			TaggableQueue<object, IkadnBaseObject> data;
			using (var parser = new IkonParser(new StreamReader(dataPath + ParametersFile)))
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
			Vertex<Vector2D> stareaterMain = null;

            for (int i = 0; i < starPositions.Stars.Length; i++)
			{
				var vertex = new Vertex<Vector2D>(new Vector2D(starPositions.Stars[i].X, starPositions.Stars[i].Y));
				maxGraph.AddVertex(vertex);
				starIndex[vertex] = i;

				if (starPositions.HomeSystems.Contains(i))
					homeNodes.Add(vertex);

				if (i == starPositions.StareaterMain)
					stareaterMain = vertex;
			}
			foreach(var edge in genMaxEdges(maxGraph.Vertices.ToList()))
				maxGraph.AddEdge(edge);

			this.removeOutliers(maxGraph);
			var treeEdges = new HashSet<Edge<Vector2D>>(genMinEdges(maxGraph, homeNodes, stareaterMain));
			
			return genFinal(maxGraph, treeEdges).Select(e => new WormholeEndpoints(starIndex[e.FromVertex], starIndex[e.ToVertex])).ToList();
		}

		private IEnumerable<Edge<Vector2D>> genMaxEdges(IList<Vertex<Vector2D>> vertices)
		{
			var edges = new List<Edge<Vector2D>>();
			for(int i = 0; i < vertices.Count; i++)
				for(int j = i + 1; j < vertices.Count; j++)
					edges.Add(new Edge<Vector2D>(vertices[i], vertices[j], (vertices[i].Data - vertices[j].Data).Magnitude(), false));

			edges.Sort((a, b) => a.Weight.CompareTo(b.Weight));
			var acceptedEdges = new List<Edge<Vector2D>>();
			foreach (var edge in edges)
			{
				if (LineIntersects(edge, acceptedEdges) || !LineAngledWell(edge, acceptedEdges, MinimalAngleCos))
					continue;

				acceptedEdges.Add(edge);
				yield return edge;
			}
		}

		private void removeOutliers(Graph<Vector2D> graph)
		{
			var orderedEdges = graph.Edges.OrderByDescending(e => e.Weight).ToList();
			foreach (var e in orderedEdges)
			{
				graph.RemoveEdge(e);
				var pathPoints = Astar(graph, e.FromVertex, e.ToVertex).ToList();
				var longestHop = 0.0;
				var length = 0.0;
				var lastHop = e.ToVertex;
				foreach (var v in pathPoints)
				{
					var dist = (lastHop.Data - v.Data).Magnitude();
					longestHop = Math.Max(longestHop, dist);
					length += dist;
					lastHop = v;
				}

				if (pathPoints.Count <= 2 || longestHop > e.Weight || length > e.Weight * 1.5)
					graph.AddEdge(e);
			}
		}

		private IEnumerable<Edge<Vector2D>> genMinEdges(Graph<Vector2D> graph, IEnumerable<Vertex<Vector2D>> homeNodes, Vertex<Vector2D> stareaterMain)
		{
			var criticalNodes = new HashSet<Vertex<Vector2D>>();
			criticalNodes.Add(stareaterMain);
			foreach(var home in homeNodes)
			{
				var current = stareaterMain;
				foreach(var node in Astar(graph, home, stareaterMain))
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
		
		private IEnumerable<Edge<Vector2D>> genFinal(Graph<Vector2D> maxGraph, ICollection<Edge<Vector2D>> treeEdges)
		{
			var degree = new Dictionary<Vertex<Vector2D>, int>();
			foreach (var v in maxGraph.Vertices)
				degree[v] = 0;

			var edgeQueues = new Dictionary<int, PriorityQueue<Edge<Vector2D>, double>>();
			foreach (var e in maxGraph.Edges)
			{
				if (treeEdges.Contains(e))
				{
					degree[e.FromVertex]++;
					degree[e.ToVertex]++;
					yield return e;
				}
				else
				{
					int eDegree = Math.Max(degree[e.FromVertex], degree[e.ToVertex]);
					if (!edgeQueues.ContainsKey(eDegree))
						edgeQueues[eDegree] = new PriorityQueue<Edge<Vector2D>, double>(PriorityQueueType.Minimum);
					edgeQueues[eDegree].Enqueue(e, e.Weight);
				}
			}

			int minDegree = -1;
			var neededCount = (int)((maxGraph.Edges.Count - treeEdges.Count) * this.degreeOptions[this.degreesParameter.Value].Ratio);
			while (neededCount > 0)
			{
				if (!edgeQueues.ContainsKey(minDegree))
					minDegree = edgeQueues.Keys.Min();
				
				var e = edgeQueues[minDegree].Dequeue();
				if (edgeQueues[minDegree].Count == 0)
					edgeQueues.Remove(minDegree);
				
				var eDegree = Math.Max(degree[e.FromVertex], degree[e.ToVertex]);
				if (eDegree != minDegree)
				{
					if (!edgeQueues.ContainsKey(eDegree))
						edgeQueues[eDegree] = new PriorityQueue<Edge<Vector2D>, double>(PriorityQueueType.Minimum);
					edgeQueues[eDegree].Enqueue(e, e.Weight);
				}
				else
				{
					degree[e.FromVertex]++;
					degree[e.ToVertex]++;
					neededCount--;
					yield return e;
				}
			}
		}
		
		private static IEnumerable<Vertex<Vector2D>> Astar(Graph<Vector2D> graph, Vertex<Vector2D> fromNode, Vertex<Vector2D> toNode)
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

		private static bool LineAngledWell(Edge<Vector2D> line, IEnumerable<Edge<Vector2D>> otherLines, double maxCos)
		{
			foreach (var otherLine in otherLines)
			{
				if (line == otherLine)
					continue;

				var aa = (line.FromVertex.Data - otherLine.FromVertex.Data).Magnitude();
				var ab = (line.FromVertex.Data - otherLine.ToVertex.Data).Magnitude();
				var ba = (line.ToVertex.Data - otherLine.FromVertex.Data).Magnitude();
				var bb = (line.ToVertex.Data - otherLine.ToVertex.Data).Magnitude();

				if (aa > Epsilon && ab > Epsilon && ba > Epsilon && bb > Epsilon)
					continue;

				var intersection = aa < Epsilon || ab < Epsilon ? line.FromVertex.Data : line.ToVertex.Data;
				var aSpan = (aa < Epsilon || ab < Epsilon ? line.ToVertex.Data : line.FromVertex.Data) - intersection;
				var bSpan = (aa < Epsilon || ba < Epsilon ? otherLine.ToVertex.Data : otherLine.FromVertex.Data) - intersection;
				aSpan.Normalize();
				bSpan.Normalize();

				if (aSpan.DotProduct(bSpan) >= maxCos)
					return false;
			}

			return true;
		}

		private static bool LineIntersects(Edge<Vector2D> line, IEnumerable<Edge<Vector2D>> otherLines)
		{
			Vector2D x0 = line.FromVertex.Data;
			Vector2D v0 = line.ToVertex.Data - x0;
			var n0 = new Vector2D(-v0.Y, v0.X);
			double v0magSquare = v0.X * v0.X + v0.Y * v0.Y;

			foreach (var usedEdge in otherLines) {
				Vector2D x1 = usedEdge.FromVertex.Data;
				Vector2D v1 = usedEdge.ToVertex.Data - x1;
				var cross = v0.X * v1.Y - v0.Y * v1.X; //FIX workaraound for NGenerics bug

				if (Math.Abs(cross) < Epsilon)
					if ((x0 - x1).Magnitude() < Epsilon)
						return true;
					else
						continue;

				double t1 = n0.DotProduct(x0 - x1) / n0.DotProduct(v1);
				double t0 = v0.DotProduct(x1 + v1 * t1 - x0) / v0magSquare;

				if (t0 > 0 && t0 < 1 && t1 > 0 && t1 < 1)
					return true;
			}

			return false;
		}
	}
}
