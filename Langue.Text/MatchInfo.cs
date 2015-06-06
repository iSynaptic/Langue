using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langue
{
    public struct MatchInfo<T>
    {
        public MatchInfo(T value, string description, LocationRange location)
        {
            Value = value;
            Description = description;
            Location = location;
        }

        public T Value { get; }
        public string Description { get; }
        public LocationRange Location { get; }
    }
}
