﻿using System;
using System.Linq;

namespace Langue
{
    public static partial class Text
    {
        public static Pattern<ResultInfo<T>, TextContext> WithInfo<T>(this Pattern<T, TextContext> self) => context =>
        {
            var result = self(context);
            if (result.HasValue)
            {
                var location = new PositionRange(context.ConsumedTo, result.Context.ConsumedTo);

                var info = new ResultInfo<T>(result.Value, result.Description, location);
                return Match.Success(info, result.Description, result.Context, result.Observations);
            }

            return Match<ResultInfo<T>>.Failure(result.Description, result.Context, result.Observations);
        };

        public static Pattern<T, TextContext> InterleaveWith<T>(this Pattern<T, TextContext> @this, Pattern<Object, TextContext> interleaving) => context =>
        {
            interleaving = context.Interleaving != null
                                ? context.Interleaving.Or(interleaving)
                                : interleaving;

            var newContext = context.WithInterleave(interleaving);

            return @this(newContext);
        };

        public static Pattern<T, TextContext> Interleave<T>(this Pattern<T, TextContext> @this) => context =>
        {
            if (context.Interleaving != null)
            {
                var result = context.Interleaving(context);
                while (result.HasValue)
                {
                    context = result.Context;
                    result = context.Interleaving(context);
                }
            }

            return @this(context);
        };

        public static Pattern<T, TextContext> Or<T>(this Pattern<T, TextContext> first, Pattern<T, TextContext> second) => context =>
        {
            var firstResult = first(context);
            if (firstResult.HasValue)
                return firstResult;

            var secondResult = second(context);
            if (secondResult.HasValue)
                return secondResult;

            return Match<T>.Failure("or",
                context,
                new ParseError(
                    $"Expected: {firstResult.Description} or {secondResult.Description}",
                    context.ReadTo.ToRange(),
                    firstResult.Observations.Concat(secondResult.Observations)));
        };

        public static Pattern<U, TextContext> Select<T, U>(this Pattern<T, TextContext> @this, Func<T, U> selector) => context =>
        {
            var result = @this(context);
            if (result.HasValue)
                return Match.Success(selector(result.Value), result.Description, result.Context, result.Observations);

            return Match<U>.Failure(result.Description, result.Context, result.Observations);
        };

        public static Pattern<U, TextContext> SelectMany<T, U>(this Pattern<T, TextContext> @this, Func<T, Pattern<U, TextContext>> selector) => context =>
        {
            var result = @this.Interleave()(context);
            if (result.HasValue)
            {
                var nextResult = selector(result.Value).Interleave()(result.Context);
                return nextResult;
            }

            return Match<U>.Failure(result.Description, result.Context, result.Observations);
        };

        public static Pattern<TResult, TextContext> SelectMany<T, TIntermediate, TResult>(this Pattern<T, TextContext> @this,
                                                                            Func<T, Pattern<TIntermediate, TextContext>> selector,
                                                                            Func<T, TIntermediate, TResult> combiner) 
            => SelectMany(@this, x => selector(x).Select(y => combiner(x, y)));

        public static Pattern<T, TextContext> DescribeAs<T>(this Pattern<T, TextContext> @this, string description) => context =>
        {
            var result = @this(context);
            return result.HasValue
                ? Match.Success(result.Value, description, result.Context, result.Observations)
                : Match<T>.Failure(description, result.Context, result.Observations);
        };
    }

}