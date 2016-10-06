using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using NGenerics.DataStructures.Mathematical;
using NGenerics.DataStructures.Queues;
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

		private SelectorParameter degreesParameter;
		private ParameterList parameters;
		private DegreeOption[] degreeOptions;

		public ProximityLanesBuilder()
		{
			TaggableQueue<object, IkadnBaseObject> data;
			using (var parser = new IkonParser(new StreamReader(MapAssets.MapsFolder + ParametersFile)))
				data = parser.ParseAll();

			degreesParameter = loadDegrees(data);
			this.parameters = new ParameterList(new ParameterBase[]{
				degreesParameter,
			});
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

		public ParameterList Parameters
		{
			get { return parameters; }
		}


		public IEnumerable<WormholeEndpoints> Generate(Random rng, StarPositions starPositions)
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

			var lanes = new List<WormholeEndpoints>();
			for (int group = 0; group < starPositions.HomeSystems.Length; group++) {
				lanes.AddRange(connectGroup(
					starPositions.Stars, 
					Methods.SelectIndices(starGroups, x => x.Value == group).ToArray(),
					starPositions.HomeSystems[group]));
			}

			return lanes;
		}

		private IEnumerable<WormholeEndpoints> connectGroup(Vector2D[] positions, int[] indices, int root)
		{
			var closed = new HashSet<int>();
			var possibleEdges = new PriorityQueue<WormholeEndpoints, double>(PriorityQueueType.Minimum);

			closeVertex(possibleEdges, root, positions, indices, closed);
			var usedEdges = new List<Tuple<Vector2D, Vector2D>>();

			while (closed.Count < indices.Length) {
				var edge = possibleEdges.Dequeue();
				int openVertex = -1;

				if (closed.Contains(edge.FromIndex))
					if (closed.Contains(edge.ToIndex))
						continue;
					else
						openVertex = edge.ToIndex;
				else
					openVertex = edge.FromIndex;

				var line = new Tuple<Vector2D, Vector2D>(positions[edge.FromIndex], positions[edge.ToIndex]);
				if (!Methods.LineIntersects(line, usedEdges, Epsilon)) {
					usedEdges.Add(line);
					yield return edge;
				}

				closeVertex(possibleEdges, openVertex, positions, indices, closed);
			}
		}

		private static void closeVertex(PriorityQueue<WormholeEndpoints, double> freeEdges, int vertexToClose, IList<Vector2D> positions, int[] vertexIndices, ISet<int> closed)
		{
			closed.Add(vertexToClose);
			foreach (var vertex in vertexIndices)
				if (!closed.Contains(vertex)) {
					double weight = (positions[vertexToClose] - positions[vertex]).Magnitude();
					freeEdges.Add(new WormholeEndpoints(vertexToClose, vertex), weight);
				}
		}
	}
}
