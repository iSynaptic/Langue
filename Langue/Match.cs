using System;
using System.Collections.Generic;

namespace Langue
{
    public static class Match
    {
        public static readonly IEnumerable<MatchObservation> NoObservations = new MatchObservation[0];

        public static Match<T, TContext> Success<T, TContext>(T value, string description, TContext context, params MatchObservation[] observations)
            => new Match<T, TContext>(value, description, context, observations);

        public static Match<T, TContext> Success<T, TContext>(T value, string description, TContext context, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(value, description, context, observations);
    }

    public static class Match<T>
    {
        public static Match<T, TContext> Failure<TContext>(string description, TContext context, params MatchObservation[] observations)
            => new Match<T, TContext>(Optional<T>.NoValue, description, context, observations);

        public static Match<T, TContext> Failure<TContext>(string description, TContext context, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(Optional<T>.NoValue, description, context, observations);

        public static Match<T, TContext> Success<TContext>(T value, string description, TContext context, params MatchObservation[] observations)
            => new Match<T, TContext>(value, description, context, observations);

        public static Match<T, TContext> Success<TContext>(T value, string description, TContext context, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(value, description, context, observations);
    }

    public class Match<T, TContext> : IMatch<T, TContext>
    {
        private readonly Optional<T> _value;

        public Match(Optional<T> value, string description, TContext context, IEnumerable<MatchObservation> observations)
        {
            _value = value;
            Description = description ?? string.Empty;
            Context = context;
            Observations = observations ?? Match.NoObservations;
        }

        public T Value => _value.Value;
        public bool HasValue => _value.HasValue;

        public string Description { get; }

        public TContext Context { get; }
        public IEnumerable<MatchObservation> Observations { get; }
    }
}
