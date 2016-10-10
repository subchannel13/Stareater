using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
		public static Organization[] List { get; private set; }

		public static void Loader(IEnumerable<TextReader> dataSources)
		{
			var list = new List<Organization>();
			foreach(var source in dataSources)
			{
				var parser = new IkonParser(source);
				foreach(var item in parser.ParseAll())
					list.Add(load(item.Value.To<IkonComposite>()));
			}
			
			list.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
			List = list.ToArray();
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
		
		const string NameKey = "name";
		const string DescriptionKey = "description";
		#endregion
	}
}
