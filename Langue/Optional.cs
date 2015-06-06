using System;
using System.Collections.Generic;

namespace Langue
{
    public struct Optional<T>
    {
        public static readonly Optional<T> NoValue = new Optional<T>();

        private readonly T _value;

        public Optional(T value) : this()
        {
            if(value != null)
            {
                _value = value;
                HasValue = true;
            }
        }

        public bool HasValue { get; }

        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("Optional object must have a value.");

                return _value;
            }
        }

        public static implicit operator Optional<T>(T value) => new Optional<T>(value);
    }
}
