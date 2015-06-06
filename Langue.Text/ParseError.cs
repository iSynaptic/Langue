using System.Collections.Generic;
using System.Linq;

namespace Langue
{
    public class ParseError : MatchObservation
    {
        private static readonly IEnumerable<MatchObservation> Empty = new MatchObservation[0];

        public ParseError(string message, PositionRange position)
            : this(message, position, null)
        {
        }

        public ParseError(string message, PositionRange position, IEnumerable<MatchObservation> observations) 
            : base(message)
        {
            Position = position;
            Observations = observations?.ToArray() ?? Empty;
        }

        public PositionRange Position { get; }
        public IEnumerable<MatchObservation> Observations { get; }
    }
}