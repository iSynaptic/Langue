using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langue
{
    public static class CoreOperators
    {
        public static Pattern<U, TContext> Select<T, U, TContext>(this Pattern<T, TContext> @this, Func<T, U> selector) => context =>
        {
            var result = @this(context);
            if (result.HasValue)
                return Match.Success(selector(result.Value), result.Description, result.Context, result.Observations);

            return Match<U>.Failure(result.Description, result.Context, result.Observations);
        };

        public static Pattern<U, TContext> SelectMany<T, U, TContext>(this Pattern<T, TContext> @this, Func<T, Pattern<U, TContext>> selector) => context =>
        {
            var result = @this(context);
            if (result.HasValue)
            {
                var nextResult = selector(result.Value)(result.Context);
                return nextResult;
            }

            return Match<U>.Failure(result.Description, result.Context, result.Observations);
        };

        public static Pattern<TResult, TContext> SelectMany<T, TIntermediate, TResult, TContext>(this Pattern<T, TContext> @this,
                                                                            Func<T, Pattern<TIntermediate, TContext>> selector,
                                                                            Func<T, TIntermediate, TResult> combiner)
            => SelectMany(@this, x => selector(x).Select(y => combiner(x, y)));

        public static Pattern<T, TContext> DescribeAs<T, TContext>(this Pattern<T, TContext> @this, string description) => context =>
        {
            var result = @this(context);
            return result.HasValue
                ? Match.Success(result.Value, description, result.Context, result.Observations)
                : Match<T>.Failure(description, result.Context, result.Observations);
        };

        public static Pattern<IEnumerable<T>, TContext> Optional<T, TContext>(this Pattern<IEnumerable<T>, TContext> pattern)
        {
            return ctx =>
            {
                var result = pattern(ctx);
                if (result.HasValue)
                    return result;

                return Match.Success(Enumerable.Empty<T>(), result.Description, ctx, result.Observations);
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

                return Match.Success(val, result.Description, newCtx, result.Observations);
            };
        }

        public static Pattern<IEnumerable<T>, TContext> Flatten<T, TContext>(this Pattern<IEnumerable<IEnumerable<T>>, TContext> parser)
        {
            return ctx =>
            {
                var items = parser(ctx);
                if (items.HasValue)
                    return Match.Success(items.Value.SelectMany(x => x), "", ctx);
                else
                    return Match<IEnumerable<T>>.Failure("", ctx);
            };
        }

        public static Pattern<T, TContext> Where<T, TContext>(this Pattern<T, TContext> @this, Func<T, bool> predicate)
        {
            return ctx =>
            {
                var result = @this(ctx);
                if (!result.HasValue)
                    return result;

                return predicate(result.Value)
                    ? result
                    : Match<T>.Failure("", ctx);
            };
        }

        public static Pattern<T, TContext> Unless<T, TContext>(this Pattern<T, TContext> @this, Func<T, bool> predicate)
        {
            return ctx =>
            {
                var result = @this(ctx);
                if (!result.HasValue)
                    return result;

                return !predicate(result.Value)
                    ? result
                    : Match<T>.Failure("", ctx);
            };
        }
    }
}
