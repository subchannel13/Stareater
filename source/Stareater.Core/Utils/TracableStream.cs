using System;
using System.IO;

namespace Stareater.Utils
{
	public class TracableStream
	{
		public TextReader Stream { get; private set; }
		public string SourceInfo { get; private set; }

		public TracableStream(TextReader stream, string sourceInfo)
		{
			this.Stream = stream;
			this.SourceInfo = sourceInfo;
		}
	}
}
