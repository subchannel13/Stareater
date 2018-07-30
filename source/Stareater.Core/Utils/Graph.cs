using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Utils
{
	public class Graph<T>
	{
		private readonly HashSet<Vertex<T>> vertices = new HashSet<Vertex<T>>();
		private readonly Dictionary<Pair<Vertex<T>>, double> weights = new Dictionary<Pair<Vertex<T>>, double>();
		private readonly Dictionary<Vertex<T>, HashSet<Vertex<T>>> adjacency = new Dictionary<Vertex<T>, HashSet<Vertex<T>>>();

		public Vertex<T> MakeVertex(T data)
		{
			var vertex = new Vertex<T>(data);
			this.vertices.Add(vertex);
			this.adjacency.Add(vertex, new HashSet<Vertex<T>>());

			return vertex;
		}

		public IEnumerable<Vertex<T>> Vertices
		{
			get { return this.vertices; }
		}

		public int VertexCount
		{
			get { return this.vertices.Count; }
		}

		public IEnumerable<Edge<T>> Edges
		{
			get { return this.weights.Select(x => new Edge<T>(x.Key.First, x.Key.Second, x.Value)); }
		}

		public int EdgeCount
		{
			get { return this.weights.Count; }
		}

		public void AddEdge(Edge<T> edge)
		{
			this.weights.Add(new Pair<Vertex<T>>(edge.FirstEnd, edge.SecondEnd), edge.Weight);
			this.adjacency[edge.FirstEnd].Add(edge.SecondEnd);
			this.adjacency[edge.SecondEnd].Add(edge.FirstEnd);
		}

		public void RemoveEdge(Edge<T> edge)
		{
			this.weights.Remove(new Pair<Vertex<T>>(edge.FirstEnd, edge.SecondEnd));
			this.adjacency[edge.FirstEnd].Remove(edge.SecondEnd);
			this.adjacency[edge.SecondEnd].Remove(edge.FirstEnd);
		}

		public Edge<T> GetEdge(Vertex<T> fromNode, Vertex<T> toNode)
		{
			return new Edge<T>(fromNode, toNode, this.weights[new Pair<Vertex<T>>(fromNode, toNode)]);
		}

		public IEnumerable<Edge<T>> GetEdges(Vertex<T> fromNode)
		{
			return this.adjacency[fromNode].Select(toNode => this.GetEdge(fromNode, toNode));
		}
	}
}
