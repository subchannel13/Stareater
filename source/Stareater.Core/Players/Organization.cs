using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Stareater.Utils;
using Ikadn.Ikon.Types;
using Ikadn.Ikon;

namespace Stareater.Players
{
	public class Organization
	{
		public string Name { get; private set; }
		public string Description { get; private set; }

		private Organization(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}

		#region Loading related
		private const string DataFilePath = "./data/organizations.txt";

		#region Attribute keys
		const string NameKey = "name";
		const string DescriptionKey = "description";
		#endregion

		public static Organization[] List { get; private set; }

		public static IEnumerable<double> Loader()
		{
			var list = new List<Organization>();
			using (var parser = new IkonParser(new StreamReader(DataFilePath))) {
				var data = parser.ParseAll();
				yield return 0.5;

				foreach (double p in Methods.ProgressReportHelper(0.5, 0.5, data.Count)) {
					list.Add(load(data.Dequeue().To<IkonComposite>()));
					yield return p;
				}

				list.Sort((a, b) => a.Name.CompareTo(b.Name));
			}
			
			List = list.ToArray();
			yield return 1;
		}

		public static bool IsLoaded
		{
			get { return List != null; }
		}

		private static Organization load(IkonComposite data)
		{
			return new Organization(
				data[NameKey].To<string>(),
				data[DescriptionKey].To<string>()
				);
		}
		#endregion
	}
}
