using IKON;

namespace Stareater.Localization
{
	public class Text : Value
	{
		string text;

		internal Text(string line)
		{
			this.text = line;
		}

		public override void Compose(Composer composer)
		{
			//NoOP
		}

		public override string TypeName
		{
			get { return "Text"; }
		}
	}
}
