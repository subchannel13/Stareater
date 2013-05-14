﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;
using Stareater.AppData;
using System.IO;
using Ikadn;
using Ikadn.Ikon.Values;
using NGenerics.DataStructures.Mathematical;
using Stareater.Utils;
using NGenerics.DataStructures.Queues;

namespace Stareater.Maps.ProximityLanes
{
	public class ProximityLanesBuilder : IStarConnector
	{
		const string ParametersFile = "proximity_lanes.txt";

		const string LanguageContext = "ProximityLanes";
		const string DegreeKey = "Degree";
		const double Epsilon = 1e-9;

		private SelectorParameter degreesParameter;
		private ParameterList parameters;
		private DegreeOption[] degreeOptions;

		public ProximityLanesBuilder()
		{
			ValueQueue data;
			using (var parser = new Ikadn.Ikon.Parser(new StreamReader(MapAssets.MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			degreesParameter = loadDegrees(data);
			this.parameters = new ParameterList(new ParameterBase[]{
				degreesParameter,
			});
		}

		private SelectorParameter loadDegrees(ValueQueue data)
		{
			this.degreeOptions = new DegreeOption[data.CountOf(DegreeKey)];
			var parameterOptions = new Dictionary<int, string>();
			for (int i = 0; i < degreeOptions.Length; i++) {
				degreeOptions[i] = new DegreeOption(data.Dequeue(DegreeKey).To<ObjectValue>());
				parameterOptions.Add(i, degreeOptions[i].Name);
			}

			return new SelectorParameter(LanguageContext, DegreeKey, parameterOptions, (int)Math.Ceiling(parameterOptions.Count / 2.0));
		}

		public string Name
		{
			get { return Settings.Get.Language[LanguageContext]["name"].Text(); }
		}

		public string Description
		{
			get
			{
				return Settings.Get.Language[LanguageContext]["description"].Text(new Dictionary<string, double>(){
				{"minDegree", degreeOptions[degreesParameter.Value].Min},
				{"maxDegree", degreeOptions[degreesParameter.Value].Max}
				});
			}
		}

		public ParameterList Parameters
		{
			get { return parameters; }
		}


		public IEnumerable<Tuple<int, int>> Generate(Random rng, StarPositions starPositions)
		{
			var starGroups = new Dictionary<int, int>();
			for (int star = 0; star < starPositions.Stars.Length; star++) {
				
				int group = 0;
				for (int i = 1; i < starPositions.HomeSystems.Length; i++)
					if ((starPositions.Stars[star] - starPositions.Stars[starPositions.HomeSystems[group]]).Magnitude() >
						(starPositions.Stars[star] - starPositions.Stars[starPositions.HomeSystems[i]]).Magnitude())
						group = i;

				starGroups.Add(star, group);
			}

			List<Tuple<int, int>> lanes = new List<Tuple<int, int>>();
			for (int group = 0; group < starPositions.HomeSystems.Length; group++) {
				lanes.AddRange(connectGroup(
					starPositions.Stars, 
					Methods.SelectIndices(starGroups, x => x.Value == group).ToArray(),
					starPositions.HomeSystems[group]));
			}

			return lanes;
		}

		private IEnumerable<Tuple<int, int>> connectGroup(Vector2D[] positions, int[] indices, int root)
		{
			var closed = new HashSet<int>();
			var possibleEdges = new PriorityQueue<Tuple<int, int>, double>(PriorityQueueType.Minimum);

			closeVertex(possibleEdges, root, positions, indices, closed);
			var usedEdges = new List<Tuple<Vector2D, Vector2D>>();

			while (closed.Count < indices.Length) {
				var edge = possibleEdges.Dequeue();
				int openVertex = -1;

				if (closed.Contains(edge.Item1))
					if (closed.Contains(edge.Item2))
						continue;
					else
						openVertex = edge.Item2;
				else
					openVertex = edge.Item1;

				var line = new Tuple<Vector2D, Vector2D>(positions[edge.Item1], positions[edge.Item2]);
				if (!Methods.LineIntersects(line, usedEdges, Epsilon)) {
					usedEdges.Add(line);
					yield return edge;
				}

				closeVertex(possibleEdges, openVertex, positions, indices, closed);
			}
		}

		private static void closeVertex(PriorityQueue<Tuple<int, int>, double> freeEdges, int vertexToClose, Vector2D[] positions, int[] vertexIndices, HashSet<int> closed)
		{
			closed.Add(vertexToClose);
			foreach (var vertex in vertexIndices)
				if (!closed.Contains(vertex)) {
					double weight = (positions[vertexToClose] - positions[vertex]).Magnitude();
					freeEdges.Add(new Tuple<int, int>(vertexToClose, vertex), weight);
				}
		}
	}
}
