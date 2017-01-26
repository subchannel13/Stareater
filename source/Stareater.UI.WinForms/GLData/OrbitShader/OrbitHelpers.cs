using System;
using System.Collections.Generic;

namespace Stareater.GLData.OrbitShader
{
	static class OrbitHelpers
	{
		public static IEnumerable<float> PlanetOrbit(float orbitR, float orbitWidth, float orbitPieces)
		{
			//TODO(v0.6) find better heuristic for width multiplyer
			var orbitMin = orbitR - orbitWidth * 3;
			var orbitMax = orbitR + orbitWidth * 3;
				
			for(int i = 0; i < orbitPieces; i++)
			{
				var angle0 = 2 * (float)Math.PI * i / orbitPieces;
				var angle1 = 2 * (float)Math.PI * (i +1) / orbitPieces;
				var data = new List<float>();
				
				data.AddRange(orbitVertex(orbitMin * (float)Math.Cos(angle1), orbitMin * (float)Math.Sin(angle1)));
				data.AddRange(orbitVertex(orbitMax * (float)Math.Cos(angle1), orbitMax * (float)Math.Sin(angle1)));
				data.AddRange(orbitVertex(orbitMax * (float)Math.Cos(angle0), orbitMax * (float)Math.Sin(angle0)));
				
				data.AddRange(orbitVertex(orbitMax * (float)Math.Cos(angle0), orbitMax * (float)Math.Sin(angle0)));
				data.AddRange(orbitVertex(orbitMin * (float)Math.Cos(angle0), orbitMin * (float)Math.Sin(angle0)));
				data.AddRange(orbitVertex(orbitMin * (float)Math.Cos(angle1), orbitMin * (float)Math.Sin(angle1)));
				
				foreach(var x in data)
					yield return x;
			}
		}
		
		private static IEnumerable<float> orbitVertex(float x, float y)
		{
			yield return x; 
			yield return y;
			yield return x;
			yield return y;
		}
	}
}
