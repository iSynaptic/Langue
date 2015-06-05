using System;
using System.Collections.Generic;

namespace Langue
{
    public class MatchObservation
    {
        public MatchObservation(string message, MatchObservationSeverity severity)
        {
            Message = message;
            Severity = severity;
        }

        public string Message { get; }
        public MatchObservationSeverity Severity { get; }
    }
}
