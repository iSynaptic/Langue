using System;
using System.Collections.Generic;
using System.Linq;

namespace Langue
{
    public static class CoreOperators
    {
        public static Pattern<U, TContext> Select<T, U, TContext>(this Pattern<T, TContext> self, Func<T, U> selector) => ctx =>
        {
            var result = self(ctx);
            if (result.HasValue)
                return Match.Success(selector(result.Value), result.Context, result.Description, result.Observations);

            return Match<U>.Failure(result.Context, result.Description, result.Observations);
        };

        public static Pattern<U, TContext> SelectMany<T, U, TContext>(this Pattern<T, TContext> self, Func<T, Pattern<U, TContext>> selector) => ctx =>
        {
            var result = self(ctx);
            if (result.HasValue)
            {
                var nextResult = selector(result.Value)(result.Context);
                var obs = result.Observations.Concat(nextResult.Observations);

                return nextResult.HasValue
                    ? Match.Success(nextResult.Value, nextResult.Context, nextResult.Description, obs)
                    : Match<U>.Failure(nextResult.Context, nextResult.Description, obs);
            }

            return Match<U>.Failure(result.Context, result.Description, result.Observations);
        };

        public static Pattern<TResult, TContext> SelectMany<T, TIntermediate, TResult, TContext>(this Pattern<T, TContext> @this,
                                                                            Func<T, Pattern<TIntermediate, TContext>> selector,
                                                                            Func<T, TIntermediate, TResult> combiner)
            => SelectMany(@this, x => selector(x).Select(y => combiner(x, y)));

        public static Pattern<T, TContext> DescribeAs<T, TContext>(this Pattern<T, TContext> self, string description) => ctx =>
        {
            var result = self(ctx);
            return result.HasValue
                ? Match.Success(result.Value, result.Context, description, result.Observations)
                : Match<T>.Failure(result.Context, description, result.Observations);
        };

        public static Pattern<IEnumerable<T>, TContext> Optional<T, TContext>(this Pattern<IEnumerable<T>, TContext> pattern)
        {
            return ctx =>
            {
                var result = pattern(ctx);
                if (result.HasValue)
                    return result;

                return Match.Success(Enumerable.Empty<T>(), ctx, result.Description, result.Observations);
            };
        }

        public static Pattern<Optional<T>, TContext> Optional<T, TContext>(this Pattern<T, TContext> pattern)
        {
            return ctx =>
            {
                var result = pattern(ctx);

                var val = result.HasValue
                    ? new Optional<T>(result.Value)
                    : Optional<T>.NoValue;

                var newCtx = result.HasValue
                    ? result.Context
                    : ctx;

                return Match.Success(val, newCtx, result.Description, result.Observations);
            };
        }

        public static Pattern<IEnumerable<T>, TContext> Flatten<T, TContext>(this Pattern<IEnumerable<IEnumerable<T>>, TContext> self)
        {
            return ctx =>
            {
                var results = self(ctx);
                if (results.HasValue)
                    return Match.Success(results.Value.SelectMany(x => x), ctx, results.Description, results.Observations);
                else
                    return Match<IEnumerable<T>>.Failure(ctx, results.Description, results.Observations);
            };
        }

        public static Pattern<T, TContext> Where<T, TContext>(this Pattern<T, TContext> self, Func<T, bool> predicate)
        {
            return ctx =>
            {
                var result = self(ctx);
                if (!result.HasValue)
                    return result;

                return predicate(result.Value)
                    ? result
                    : Match<T>.Failure(ctx, result.Description, result.Observations);
            };
        }

        public static Pattern<T, TContext> Unless<T, TContext>(this Pattern<T, TContext> self, Func<T, bool> predicate)
        {
            return ctx =>
            {
                var result = self(ctx);
                if (!result.HasValue)
                    return result;

                return !predicate(result.Value)
                    ? result
                    : Match<T>.Failure(ctx, result.Description, result.Observations);
            };
        }
    }
}
