using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Galaxy.Builders;
using Stareater.Localization;
using Stareater.Utils;
using Stareater.Utils.Collections;
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
			var maxGraph = new Graph<Vector2D>();
			var starIndex = new Dictionary<Vertex<Vector2D>, int>();
			var homeNodes = new List<Vertex<Vector2D>>();
			Vertex<Vector2D> stareaterMain = null;

            for (int i = 0; i < starPositions.Stars.Length; i++)
			{
				var vertex = maxGraph.MakeVertex(new Vector2D(starPositions.Stars[i].X, starPositions.Stars[i].Y));
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
			
			return genFinal(maxGraph, treeEdges).Select(e => new WormholeEndpoints(starIndex[e.FirstEnd], starIndex[e.SecondEnd])).ToList();
		}

		private IEnumerable<Edge<Vector2D>> genMaxEdges(IList<Vertex<Vector2D>> vertices)
		{
			var edges = new List<Edge<Vector2D>>();
			for(int i = 0; i < vertices.Count; i++)
				for(int j = i + 1; j < vertices.Count; j++)
					edges.Add(new Edge<Vector2D>(vertices[i], vertices[j], (vertices[i].Data - vertices[j].Data).Length));

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
				var pathPoints = Astar(graph, e.FirstEnd, e.SecondEnd).ToList();
				var longestHop = 0.0;
				var length = 0.0;
				var lastHop = e.SecondEnd;
				foreach (var v in pathPoints)
				{
					var dist = (lastHop.Data - v.Data).Length;
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
			var criticalNodes = new HashSet<Vertex<Vector2D>>
			{
				stareaterMain
			};
			foreach (var home in homeNodes)
			{
				var current = stareaterMain;
				foreach(var node in Astar(graph, home, stareaterMain))
				{
					criticalNodes.Add(node);
					yield return graph.GetEdge(node, current);
					current = node;
				}
			}
			
			var treeNodes = new HashSet<Vertex<Vector2D>>(criticalNodes);
			var potentialEdges = new Dictionary<Edge<Vector2D>, double>();
			var critPathDistance = new Dictionary<Vertex<Vector2D>, double>();
			foreach(var node in criticalNodes)
				critPathDistance[node] = 0;
			
			while(treeNodes.Count < graph.VertexCount)
			{
				potentialEdges.Clear();
				foreach(var node in treeNodes)
				{
					var edges = graph.GetEdges(node).
						Where(e => !treeNodes.Contains(e.FirstEnd) || !treeNodes.Contains(e.SecondEnd));

					foreach (var edge in edges)
						potentialEdges[edge] = (edge.FirstEnd.Data - edge.SecondEnd.Data).Length + critPathDistance[node];
				}
				
				var currentEdge = potentialEdges.Aggregate((a, b) => a.Value < b.Value ? a : b).Key;
				yield return currentEdge;
				
				var fromNode = treeNodes.Contains(currentEdge.FirstEnd) ? currentEdge.FirstEnd : currentEdge.SecondEnd;
				var toNode = treeNodes.Contains(currentEdge.FirstEnd) ? currentEdge.SecondEnd : currentEdge.FirstEnd;
				var length = (currentEdge.FirstEnd.Data - currentEdge.SecondEnd.Data).Length;
				
				treeNodes.Add(toNode);
				critPathDistance[toNode] = critPathDistance[fromNode] + length;
			}
		}
		
		private IEnumerable<Edge<Vector2D>> genFinal(Graph<Vector2D> maxGraph, ICollection<Edge<Vector2D>> treeEdges)
		{
			var degree = new Dictionary<Vertex<Vector2D>, int>();
			foreach (var v in maxGraph.Vertices)
				degree[v] = 0;

			var edgeQueues = new Dictionary<int, PriorityQueue<Edge<Vector2D>>>();
			foreach (var e in maxGraph.Edges)
			{
				if (treeEdges.Contains(e))
				{
					degree[e.FirstEnd]++;
					degree[e.SecondEnd]++;
					yield return e;
				}
				else
				{
					int eDegree = Math.Max(degree[e.FirstEnd], degree[e.SecondEnd]);
					if (!edgeQueues.ContainsKey(eDegree))
						edgeQueues[eDegree] = new PriorityQueue<Edge<Vector2D>>();
					edgeQueues[eDegree].Enqueue(e, e.Weight);
				}
			}

			int minDegree = -1;
			var neededCount = (int)((maxGraph.EdgeCount - treeEdges.Count) * this.degreeOptions[this.degreesParameter.Value].Ratio);
			while (neededCount > 0)
			{
				if (!edgeQueues.ContainsKey(minDegree))
					minDegree = edgeQueues.Keys.Min();
				
				var e = edgeQueues[minDegree].Dequeue();
				if (edgeQueues[minDegree].Count == 0)
					edgeQueues.Remove(minDegree);
				
				var eDegree = Math.Max(degree[e.FirstEnd], degree[e.SecondEnd]);
				if (eDegree != minDegree)
				{
					if (!edgeQueues.ContainsKey(eDegree))
						edgeQueues[eDegree] = new PriorityQueue<Edge<Vector2D>>();
					edgeQueues[eDegree].Enqueue(e, e.Weight);
				}
				else
				{
					degree[e.FirstEnd]++;
					degree[e.SecondEnd]++;
					neededCount--;
					yield return e;
				}
			}
		}
		
		//TODO(v0.8) move A* to core, to Methods or Graph
		private static IEnumerable<Vertex<Vector2D>> Astar(Graph<Vector2D> graph, Vertex<Vector2D> fromNode, Vertex<Vector2D> toNode)
		{
			var cameFrom = new Dictionary<Vertex<Vector2D>, Vertex<Vector2D>>();
			var closedSet = new HashSet<Vertex<Vector2D>>();
			var openSet = new PriorityQueue<Vertex<Vector2D>>();
			openSet.Enqueue(fromNode, (fromNode.Data - toNode.Data).Length);
			
			var gScore = graph.Vertices.ToDictionary(x => x, x => double.PositiveInfinity);
			gScore[fromNode] = 0;
			Vertex<Vector2D> current;
		
		    while (openSet.Count > 0)
		    {
		    	current = openSet.Dequeue();
		    	if (current == toNode)
		    		break;
		
		    	closedSet.Add(current);
				var edges = graph.GetEdges(current).
					SelectMany(e => new[] { e.FirstEnd, e.SecondEnd }.Where(v => v != current));
				foreach (var neighbor in edges)
		    	{
		    		if (closedSet.Contains(neighbor))
		    			continue;
		    		
		            var tentative_gScore = gScore[current] + (current.Data - neighbor.Data).Length;
		            if (!openSet.Contains(neighbor))
		            	openSet.Enqueue(neighbor, tentative_gScore);
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

				var aa = (line.FirstEnd.Data - otherLine.FirstEnd.Data).Length;
				var ab = (line.FirstEnd.Data - otherLine.SecondEnd.Data).Length;
				var ba = (line.SecondEnd.Data - otherLine.FirstEnd.Data).Length;
				var bb = (line.SecondEnd.Data - otherLine.SecondEnd.Data).Length;

				if (aa > Epsilon && ab > Epsilon && ba > Epsilon && bb > Epsilon)
					continue;

				var intersection = aa < Epsilon || ab < Epsilon ? line.FirstEnd.Data : line.SecondEnd.Data;
				var aSpan = (aa < Epsilon || ab < Epsilon ? line.SecondEnd.Data : line.FirstEnd.Data) - intersection;
				var bSpan = (aa < Epsilon || ba < Epsilon ? otherLine.SecondEnd.Data : otherLine.FirstEnd.Data) - intersection;
				aSpan = aSpan.Unit;
				bSpan = bSpan.Unit;

				if (aSpan.Dot(bSpan) >= maxCos)
					return false;
			}

			return true;
		}

		private static bool LineIntersects(Edge<Vector2D> line, IEnumerable<Edge<Vector2D>> otherLines)
		{
			Vector2D x0 = line.FirstEnd.Data;
			Vector2D v0 = line.SecondEnd.Data - x0;
			var n0 = new Vector2D(-v0.Y, v0.X);
			double v0magSquare = v0.X * v0.X + v0.Y * v0.Y;

			foreach (var usedEdge in otherLines) {
				Vector2D x1 = usedEdge.FirstEnd.Data;
				Vector2D v1 = usedEdge.SecondEnd.Data - x1;
				var cross = v0.X * v1.Y - v0.Y * v1.X; //FIX workaraound for NGenerics bug

				if (Math.Abs(cross) < Epsilon)
					if ((x0 - x1).Length < Epsilon)
						return true;
					else
						continue;

				double t1 = n0.Dot(x0 - x1) / n0.Dot(v1);
				double t0 = v0.Dot(x1 + v1 * t1 - x0) / v0magSquare;

				if (t0 > 0 && t0 < 1 && t1 > 0 && t1 < 1)
					return true;
			}

			return false;
		}
	}
}
