using Ikon;

namespace Stareater.Localization
{
	class SingleLineText : Value, IText
	{
		string text;

		internal SingleLineText(string line)
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

		public string Get(params double[] variables) {
			return text;
		}
	}
}
