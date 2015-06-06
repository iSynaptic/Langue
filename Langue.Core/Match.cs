using System;
using System.Collections.Generic;

namespace Langue
{
    public static class Match
    {
        public static readonly IEnumerable<MatchObservation> NoObservations = new MatchObservation[0];

        public static Match<T, TContext> Success<T, TContext>(T value, TContext context, params MatchObservation[] observations)
            => new Match<T, TContext>(value, context, observations);

        public static Match<T, TContext> Success<T, TContext>(T value, TContext context, string description, params MatchObservation[] observations)
            => new Match<T, TContext>(value, context, description, observations);

        public static Match<T, TContext> Success<T, TContext>(T value, TContext context, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(value, context, observations);

        public static Match<T, TContext> Success<T, TContext>(T value, TContext context, string description, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(value, context, description, observations);
    }

    public static class Match<T>
    {
        public static Match<T, TContext> Failure<TContext>(TContext context, params MatchObservation[] observations)
            => new Match<T, TContext>(Optional<T>.NoValue, context, observations);

        public static Match<T, TContext> Failure<TContext>(TContext context, string description, params MatchObservation[] observations)
            => new Match<T, TContext>(Optional<T>.NoValue, context, description, observations);

        public static Match<T, TContext> Failure<TContext>(TContext context, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(Optional<T>.NoValue, context, observations);

        public static Match<T, TContext> Failure<TContext>(TContext context, string description, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(Optional<T>.NoValue, context, description, observations);

        public static Match<T, TContext> Success<TContext>(T value, TContext context, params MatchObservation[] observations)
           => new Match<T, TContext>(value, context, observations);

        public static Match<T, TContext> Success<TContext>(T value, TContext context, string description, params MatchObservation[] observations)
            => new Match<T, TContext>(value, context, description, observations);

        public static Match<T, TContext> Success<TContext>(T value, TContext context, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(value, context, observations);

        public static Match<T, TContext> Success<TContext>(T value, TContext context, string description, IEnumerable<MatchObservation> observations)
            => new Match<T, TContext>(value, context, description, observations);
    }

    public class Match<T, TContext> : IMatch<T, TContext>
    {
        private readonly Optional<T> _value;

        public Match(Optional<T> value, TContext context, IEnumerable<MatchObservation> observations)
            : this(value, context, string.Empty, observations) { }

        public Match(Optional<T> value, TContext context, string description, IEnumerable<MatchObservation> observations)
        {
            _value = value;
            Context = context;
            Description = description ?? string.Empty;
            Observations = observations ?? Match.NoObservations;
        }

        public T Value => _value.Value;
        public bool HasValue => _value.HasValue;

        public TContext Context { get; }

        public string Description { get; }
        public IEnumerable<MatchObservation> Observations { get; }
    }
}
