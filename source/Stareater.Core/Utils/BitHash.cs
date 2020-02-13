using Stareater.Utils.StateEngine;
using System;

namespace Stareater.Utils
{
	[StateTypeAttribute(true)]
	public class BitHash
	{
		private readonly uint[] contents;
		private readonly int hash;

		public BitHash(uint[] contents)
		{
			if (contents == null)
				throw new ArgumentNullException(nameof(contents));

			this.contents = contents;
			
			int hash = 0;

			foreach (uint i in contents) {
				hash *= 31;
				hash += i.GetHashCode() & 0xff;
				hash &= 0x003fffff;
			}
			this.hash = hash;
		}

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as BitHash;
			if (other == null)
				return false;
			
			if (other.contents.Length != contents.Length) 
				return false;
			for (int i = 0; i < contents.Length; i++)
				if (other.contents[i] != contents[i])
					return false;

			return true;
		}

		public override int GetHashCode()
		{
			return hash;
		}
		
		public static bool operator ==(BitHash lhs, BitHash rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(BitHash lhs, BitHash rhs) {
			return !(lhs == rhs);
		}

		#endregion
	}
}
