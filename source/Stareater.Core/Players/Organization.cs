using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ikon.Ston.Values;

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

		private const string DataFilePath = "./data/organizations.txt";

		#region Attribute keys
		const string NameKey = "name";
		const string DescriptionKey = "description";
		#endregion

		public static Organization[] List;

		public static IEnumerable<double> Loader()
		{
			List<Organization> list = new List<Organization>();
			using (Ikon.Ston.Parser parser = new Ikon.Ston.Parser(new StreamReader(DataFilePath))) {
				var data = parser.ParseAll();
				yield return 0.5;

				double count = data.Count;
				for (int i = 0; i < count; i++) {
					list.Add(load(data.Dequeue() as ObjectValue));
					yield return 0.5 + 0.5 * i / count;
				}
			}

			List = list.ToArray();
			yield return 1;
		}

		private static Organization load(ObjectValue data)
		{
			return new Organization(
				(data[NameKey] as TextValue).GetText,
				(data[DescriptionKey] as TextValue).GetText
				);
		}
	}
}
