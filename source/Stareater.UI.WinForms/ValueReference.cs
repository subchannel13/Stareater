namespace Stareater
{
	class ValueReference<T>
	{
		public ValueReference()
		{
			this.Value = default;
		}

		public ValueReference(T value)
		{
			this.Value = value;
		}

		public T Value { get; set; }
	}
}
