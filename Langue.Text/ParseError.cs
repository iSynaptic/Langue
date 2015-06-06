using System.Collections.Generic;
using System.Linq;

namespace Langue
{
    public class ParseError : MatchObservation
    {
        private static readonly IEnumerable<MatchObservation> Empty = new MatchObservation[0];

        public ParseError(string message, LocationRange position)
            : this(message, position, null)
        {
        }

        public ParseError(string message, LocationRange position, IEnumerable<MatchObservation> observations) 
            : base(message)
        {
            Position = position;
            Observations = observations?.ToArray() ?? Empty;
        }

        public LocationRange Position { get; }
        public IEnumerable<MatchObservation> Observations { get; }
    }
}