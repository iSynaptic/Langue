using System;
using System.Collections.Generic;

namespace Langue
{
    public class MatchObservation
    {
        public MatchObservation(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("You must provide a message.", nameof(message));

            Message = message;
        }

        public string Message { get; }
    }
}
