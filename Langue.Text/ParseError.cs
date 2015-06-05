using System.Collections.Generic;
using System.Linq;

namespace Langue
{
    public class ParseError
    {
        public ParseError(PositionRange position, string message)
            : this(position, message, null)
        {
        }

        public ParseError(PositionRange position, string message, IEnumerable<ParseError> errors)
        {
            Position = position;
            Message = message;
            Errors = (errors ?? new ParseError[0]).ToArray();
        }

        public PositionRange Position { get; }
        public string Message { get; }

        public IEnumerable<ParseError> Errors { get; }
    }
}