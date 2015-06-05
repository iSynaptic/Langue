using System;
using System.Collections.Generic;

namespace Langue
{
    public interface IMatch<out T, TContext>
    {
        T Value { get; }
        MatchOutcome Outcome { get; }

        string Description { get; }

        TContext Context { get; }
        IEnumerable<MatchObservation> Observations { get; }
    }
}
