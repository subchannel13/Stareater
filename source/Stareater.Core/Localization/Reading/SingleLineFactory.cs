using Ikadn;

namespace Stareater.Localization.Reading
{
	class SingleLineFactory : IIkadnObjectFactory
	{
		public IkadnBaseObject Parse(IkadnReader reader)
		{
			return new SingleLineText(reader.ReadUntil('\n', '\r', IkadnReader.EndOfStreamResult).Trim());
		}

		public char Sign
		{
			get { return '='; }
		}
	}
}
