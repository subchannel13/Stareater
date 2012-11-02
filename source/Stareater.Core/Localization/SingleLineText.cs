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

		protected override void DoCompose(IkonWriter writer)
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
