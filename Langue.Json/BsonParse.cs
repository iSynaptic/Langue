using System;
using System.Collections.Generic;
using System.Linq;

namespace Langue
{
    //public static class BsonParse
    //{
    //    private static BsonValue NavigateTo(this BsonValue value, string path)
    //    {
    //        string[] parts = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

    //        foreach (string part in parts)
    //        {
    //            if (!value.IsBsonDocument)
    //                return null;

    //            var doc = value.AsBsonDocument;
    //            if (doc.Contains(part))
    //                value = value.AsBsonDocument[part];
    //            else
    //                return null;
    //        }

    //        return value;
    //    }

    //    private static BsonParser<T> SelectValue<T>(string path, Func<BsonValue, T> selector)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if (!value.HasValue)
    //                return Result.WithoutValue<T, BsonValue>(ctx);

    //            return Result.WithValue(selector(value.Value), ctx);
    //        };
    //    }

    //    public static BsonParser<T> Defer<T>(Func<T> valueSelector)
    //    {
    //        return ctx => Result.WithValue(valueSelector(), ctx);
    //    }

    //    public static BsonParser<T> Or<T>(this BsonParser<T> @this, BsonParser<T> other)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
                
    //            return result.HasValue 
    //                ? result 
    //                : other(ctx);
    //        };
    //    }

    //    public static BsonParser<ObjectId> ObjectId()
    //    {
    //        return SelectValue("", x => x.AsObjectId);
    //    }
        
    //    public static BsonParser<ObjectId> ObjectId(string path)
    //    {
    //        return SelectValue(path, x => x.AsObjectId);
    //    }

    //    public static BsonParser<String> String()
    //    {
    //        return SelectValue("", x => x.AsString);
    //    }

    //    public static BsonParser<String> String(string path)
    //    {
    //        return SelectValue(path, x => x.AsString);
    //    }

    //    public static BsonParser<DateTime> Date()
    //    {
    //        return SelectValue("", x => x.ToUniversalTime());
    //    }

    //    public static BsonParser<DateTime> Date(string path)
    //    {
    //        return SelectValue(path, x => x.ToUniversalTime());
    //    }

    //    public static BsonParser<Int32> Int32(string path)
    //    {
    //        return SelectValue(path, x => x.AsInt32);
    //    }

    //    public static BsonParser<Int64> Int64(string path)
    //    {
    //        return SelectValue(path, x => x.AsInt64);
    //    }

    //    public static BsonParser<IEnumerable<T>> OneOrMore<T>(string path, BsonParser<T> itemParser)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if (!value.HasValue)
    //                return Result.WithoutValue<IEnumerable<T>, BsonValue>(ctx);

    //            var val = value.Value;

    //            if (val.IsBsonArray)
    //            {
    //                var array = val.AsBsonArray;
    //                var results = new List<T>();

    //                foreach (var element in array)
    //                {
    //                    var item = itemParser(element);
    //                    if (item.HasValue)
    //                        results.Add(item.Value);
    //                    else
    //                        return Result.WithoutValue<IEnumerable<T>, BsonValue>(ctx);
    //                }

    //                return Result.WithValue(results.ToArray(), ctx);
    //            }

    //            return itemParser.Select(x => new[] {x})(val);
    //        };
    //    }

    //    public static BsonParser<T> Object<T>(string path, BsonParser<T> body)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if(!value.HasValue || !value.Value.IsBsonDocument)
    //                return Result.WithoutValue<T, BsonValue>(ctx);

    //            var doc = value.Value.AsBsonDocument;
    //            return body(doc);
    //        };
    //    }

    //    public static BsonParser<IEnumerable<T>> Array<T>(string path, BsonParser<T> itemParser)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if (!value.HasValue || !value.Value.IsBsonArray)
    //                return Result.WithoutValue<IEnumerable<T>, BsonValue>(ctx);

    //            var array = value.Value.AsBsonArray;

    //            var results = new List<T>();

    //            foreach (var element in array)
    //            {
    //                var item = itemParser(element);
    //                if (item.HasValue)
    //                    results.Add(item.Value);
    //                else
    //                    return Result.WithoutValue<IEnumerable<T>, BsonValue>(ctx);
    //            }

    //            return Result.WithValue(results.ToArray(), ctx);
    //        };
    //    }


    //    public static BsonParser<T> Where<T>(this BsonParser<T> @this, Func<T, bool> predicate)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            if (!result.HasValue)
    //                return result;

    //            return predicate(result.Value)
    //                ? result
    //                : Result.WithoutValue<T, BsonValue>(ctx);
    //        };
    //    }

    //    public static BsonParser<T> Unless<T>(this BsonParser<T> @this, Func<T, bool> predicate)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            if (!result.HasValue)
    //                return result;

    //            return !predicate(result.Value)
    //                ? result
    //                : Result.WithoutValue<T, BsonValue>(ctx);
    //        };
    //    }

    //    public static BsonParser<Maybe<T>> Optional<T>(this BsonParser<T> @this)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return Result.WithValue(result.HasValue
    //                ? result.Value.ToMaybe()
    //                : Maybe<T>.NoValue, ctx);
    //        };
    //    }

    //    public static BsonParser<IEnumerable<T>> Optional<T>(this BsonParser<IEnumerable<T>> @this)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return Result.WithValue(result.HasValue
    //                ? result.Value
    //                : Enumerable.Empty<T>(), ctx);
    //        };
    //    }

    //    public static BsonParser<TResult> Select<T, TResult>(this BsonParser<T> @this, Func<T, TResult> selector)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return result.HasValue
    //                ? Result.WithValue(selector(result.Value), ctx)
    //                : Result.WithoutValue<TResult, BsonValue>(ctx);
    //        };
    //    }

    //    public static BsonParser<TResult> SelectMany<T, TResult>(this BsonParser<T> @this, Func<T, BsonParser<TResult>> selector)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return result.HasValue
    //                ? selector(result.Value)(result.Context)
    //                : Result.WithoutValue<TResult, BsonValue>(ctx);
    //        };
    //    }

    //    public static BsonParser<TResult> SelectMany<T, TIntermediate, TResult>(this BsonParser<T> @this,
    //        Func<T, BsonParser<TIntermediate>> selector,
    //        Func<T, TIntermediate, TResult> combiner)
    //    {
    //        return SelectMany(@this, x => selector(x).Select(y => combiner(x, y)));
    //    }
    //}
}