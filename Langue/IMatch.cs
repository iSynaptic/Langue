using System;
using System.Collections.Generic;

namespace Langue
{
    public interface IMatch<out T, out TContext>
    {
        T Value { get; }
        bool HasValue { get; }

        string Description { get; }

        TContext Context { get; }
        IEnumerable<MatchObservation> Observations { get; }
    }
}
