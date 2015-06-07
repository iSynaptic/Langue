using System;
using System.Linq;

namespace Langue
{
    public static partial class Text
    {
        public static Pattern<T, TContext> To<T, TContext>(this Pattern<string, TContext> self, Pattern<T, TextContext> pattern) => ctx =>
        {
            var result = self(ctx);
            if (!result.HasValue)
                return Match<T>.Failure(ctx, "");

            var tResult = pattern(result.Value);

            return tResult.HasValue
                ? Match.Success(tResult.Value, ctx, tResult.Description, tResult.Observations)
                : Match<T>.Failure(ctx, "");
        };

        public static Pattern<MatchInfo<T>, TextContext> WithInfo<T>(this Pattern<T, TextContext> self) => ctx =>
        {
            var result = self(ctx);
            if (result.HasValue)
            {
                var location = new LocationRange(ctx.ConsumedTo, result.Context.ConsumedTo);

                var info = new MatchInfo<T>(result.Value, result.Description, location);
                return Match.Success(info, result.Context, result.Description, result.Observations);
            }

            return Match<MatchInfo<T>>.Failure(result.Context, result.Description, result.Observations);
        };

        public static Pattern<T, TextContext> InterleaveWith<T>(this Pattern<T, TextContext> self, Pattern<Object, TextContext> interleaving) => ctx =>
        {
            interleaving = ctx.Interleaving != null
                                ? ctx.Interleaving.Or(interleaving)
                                : interleaving;

            var newContext = ctx.WithInterleave(interleaving);

            return self(newContext);
        };

        public static Pattern<T, TextContext> Interleave<T>(this Pattern<T, TextContext> self) => ctx =>
        {
            if (ctx.Interleaving != null)
            {
                var result = ctx.Interleaving(ctx);
                while (result.HasValue)
                {
                    ctx = result.Context;
                    result = ctx.Interleaving(ctx);
                }
            }

            return self(ctx);
        };

        public static Pattern<T, TextContext> Or<T>(this Pattern<T, TextContext> self, Pattern<T, TextContext> second) => ctx =>
        {
            var firstResult = self(ctx);
            if (firstResult.HasValue)
                return firstResult;

            var secondResult = second(ctx);
            if (secondResult.HasValue)
                return secondResult;

            return Match<T>.Failure(ctx,
                "or",
                new ParseError(
                    $"Expected: {firstResult.Description} or {secondResult.Description}",
                    ctx.ReadTo.ToRange(),
                    firstResult.Observations.Concat(secondResult.Observations)));
        };

        public static Pattern<U, TextContext> SelectMany<T, U>(this Pattern<T, TextContext> self, Func<T, Pattern<U, TextContext>> selector) => ctx =>
        {
            var result = self.Interleave()(ctx);
            if (result.HasValue)
            {
                var nextResult = selector(result.Value).Interleave()(result.Context);
                return nextResult;
            }

            return Match<U>.Failure(result.Context, result.Description, result.Observations);
        };

        public static Pattern<TResult, TextContext> SelectMany<T, TIntermediate, TResult>(this Pattern<T, TextContext> self,
                                                                            Func<T, Pattern<TIntermediate, TextContext>> selector,
                                                                            Func<T, TIntermediate, TResult> combiner) 
            => SelectMany(self, x => selector(x).Select(y => combiner(x, y)));
    }
}