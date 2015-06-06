using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langue
{
    //public delegate IResult<T, JToken> JParser<out T>(Context<JToken> context);

    //public static class JParse
    //{
    //    public static JToken NavigateTo(this JToken value, string path)
    //    {
    //        string[] parts = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

    //        foreach (string part in parts)
    //        {
    //            if (value.Type != JTokenType.Object)
    //                return null;

    //            var obj = (JObject)value;
                
    //            if (!obj.TryGetValue(part, out value))
    //                return null;
    //        }

    //        return value;
    //    }

    //    private static JParser<T> SelectValue<T>(string path, Func<JToken, T> selector)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if (value == null)
    //                return Result.WithoutValue<T, JToken>(ctx);

    //            return Result.WithValue(selector(value), ctx);
    //        };
    //    }

    //    public static JParser<String> String()
    //    {
    //        return SelectValue("", x => x.ToObject<string>());
    //    }

    //    public static JParser<String> String(string path)
    //    {
    //        return SelectValue(path, x => x.ToObject<string>());
    //    }

    //    public static JParser<DateTime> Date()
    //    {
    //        return SelectValue("", x => DateTime.Parse(x.ToObject<string>()));
    //    }

    //    public static JParser<DateTime> Date(string path)
    //    {
    //        return SelectValue(path, x => DateTime.Parse(x.ToObject<string>()));
    //    }

    //    public static JParser<Guid> Guid()
    //    {
    //        return SelectValue("", x => System.Guid.Parse(x.ToObject<string>()));
    //    }

    //    public static JParser<Guid> Guid(string path)
    //    {
    //        return SelectValue(path, x => System.Guid.Parse(x.ToObject<string>()));
    //    }

    //    public static JParser<Decimal> Decimal()
    //    {
    //        return SelectValue("", x => System.Decimal.Parse(x.ToObject<string>()));
    //    }

    //    public static JParser<Decimal> Decimal(string path)
    //    {
    //        return SelectValue(path, x => System.Decimal.Parse(x.ToObject<string>()));
    //    }

    //    public static JParser<Boolean> Boolean()
    //    {
    //        return SelectValue("", x => System.Boolean.Parse(x.ToObject<string>()));
    //    }

    //    public static JParser<Boolean> Boolean(string path)
    //    {
    //        return SelectValue(path, x => System.Boolean.Parse(x.ToObject<string>()));
    //    }

    //    public static JParser<Int32> Int32(string path)
    //    {
    //        return SelectValue(path, x => x.ToObject<int>());
    //    }

    //    public static JParser<Int64> Int64(string path)
    //    {
    //        return SelectValue(path, x => x.ToObject<long>());
    //    }

    //    public static JParser<IEnumerable<T>> OneOrMore<T>(string path, JParser<T> itemParser)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if (value == null)
    //                return Result.WithoutValue<IEnumerable<T>, JToken>(ctx);

    //            if (value.Type == JTokenType.Array)
    //            {
    //                var array = (JArray)value;
    //                var results = new List<T>();

    //                foreach (var element in array)
    //                {
    //                    var item = itemParser(element);
    //                    if (item.HasValue)
    //                        results.Add(item.Value);
    //                    else
    //                        return Result.WithoutValue<IEnumerable<T>, JToken>(ctx);
    //                }

    //                return Result.WithValue(results.ToArray(), ctx);
    //            }

    //            return itemParser.Select(x => new[] { x })(value);
    //        };
    //    }

    //    public static JParser<T> Object<T>(string path, JParser<T> body)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if (value == null || value.Type != JTokenType.Object)
    //                return Result.WithoutValue<T, JToken>(ctx);

    //            var doc = (JObject)value;
    //            return body(doc);
    //        };
    //    }

    //    public static JParser<IEnumerable<T>> HashArray<T>(string path, Func<string, JParser<T>> itemParser)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if (value == null || value.Type != JTokenType.Object)
    //                return Result.WithoutValue<IEnumerable<T>, JToken>(ctx);

    //            var results = new List<T>();

    //            var doc = (JObject)value;
    //            foreach (var property in doc.Properties())
    //            {
    //                var parser = itemParser(property.Name);
    //                var item = parser(property.Value);

    //                if (item.HasValue)
    //                    results.Add(item.Value);
    //                else
    //                    return Result.WithoutValue<IEnumerable<T>, JToken>(ctx);
    //            }

    //            return Result.WithValue(results.ToArray(), ctx);
    //        };
    //    }

    //    public static JParser<IEnumerable<T>> Array<T>(string path, JParser<T> itemParser)
    //    {
    //        return ctx =>
    //        {
    //            var value = ctx.Input.NavigateTo(path);
    //            if (value == null || value.Type != JTokenType.Array)
    //                return Result.WithoutValue<IEnumerable<T>, JToken>(ctx);

    //            var array = (JArray)value;

    //            var results = new List<T>();

    //            foreach (var element in array)
    //            {
    //                var item = itemParser(element);
    //                if (item.HasValue)
    //                    results.Add(item.Value);
    //                else
    //                    return Result.WithoutValue<IEnumerable<T>, JToken>(ctx);
    //            }

    //            return Result.WithValue(results.ToArray(), ctx);
    //        };
    //    }

    //    public static JParser<IEnumerable<T>> Flatten<T>(this JParser<IEnumerable<IEnumerable<T>>> parser)
    //    {
    //        return ctx =>
    //        {
    //            var items = parser(ctx.Input);
    //            if (items.HasValue)
    //                return Result.WithValue(items.Value.SelectMany(x => x), ctx);
    //            else
    //                return Result.WithoutValue<IEnumerable<T>, JToken>(ctx);
    //        };
    //    }


    //    public static JParser<T> Where<T>(this JParser<T> @this, Func<T, bool> predicate)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            if (!result.HasValue)
    //                return result;

    //            return predicate(result.Value)
    //                ? result
    //                : Result.WithoutValue<T, JToken>(ctx);
    //        };
    //    }

    //    public static JParser<T> Unless<T>(this JParser<T> @this, Func<T, bool> predicate)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            if (!result.HasValue)
    //                return result;

    //            return !predicate(result.Value)
    //                ? result
    //                : Result.WithoutValue<T, JToken>(ctx);
    //        };
    //    }

    //    public static JParser<IEnumerable<T>> Optional<T>(this JParser<IEnumerable<T>> @this)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return Result.WithValue(result.HasValue
    //                ? result.Value
    //                : Enumerable.Empty<T>(), ctx);
    //        };
    //    }

    //    public static JParser<Optional<T>> Optional<T>(this JParser<T> @this)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return Result.WithValue(result.HasValue
    //                ? result.Value
    //                : Optional<T>.NoValue, ctx);
    //        };
    //    }

    //    public static JParser<TResult> Select<T, TResult>(this JParser<T> @this, Func<T, TResult> selector)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return result.HasValue
    //                ? Result.WithValue(selector(result.Value), ctx)
    //                : Result.WithoutValue<TResult, JToken>(ctx);
    //        };
    //    }

    //    public static JParser<TResult> SelectMany<T, TResult>(this JParser<T> @this, Func<T, JParser<TResult>> selector)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return result.HasValue
    //                ? selector(result.Value)(result.Context)
    //                : Result.WithoutValue<TResult, JToken>(ctx);
    //        };
    //    }

    //    public static JParser<TResult> SelectMany<T, TIntermediate, TResult>(this JParser<T> @this,
    //        Func<T, JParser<TIntermediate>> selector,
    //        Func<T, TIntermediate, TResult> combiner)
    //    {
    //        return SelectMany(@this, x => selector(x).Select(y => combiner(x, y)));
    //    }
    //}

    //public static class Result
    //{
    //    public static IResult<T, TInput> WithValue<T, TInput>(T value, Context<TInput> context)
    //    {
    //        return new R<T, TInput>(value, true, context);
    //    }

    //    public static IResult<T, TInput> WithoutValue<T, TInput>(Context<TInput> context)
    //    {
    //        return new R<T, TInput>(default(T), false, context);
    //    }

    //    private class R<T, TInput> : IResult<T, TInput>
    //    {
    //        private readonly T _value;

    //        public R(T value, bool hasValue, Context<TInput> context)
    //        {
    //            _value = value;
    //            HasValue = hasValue;
    //            Context = context;
    //        }

    //        public T Value
    //        {
    //            get
    //            {
    //                if (!HasValue)
    //                    throw new InvalidOperationException("No value can be computed.");

    //                return _value;
    //            }
    //        }
    //        public bool HasValue { get; private set; }

    //        public Context<TInput> Context { get; private set; }
    //    }
    //}

    //public interface IResult<out T, TInput>
    //{
    //    T Value { get; }
    //    Boolean HasValue { get; }

    //    Context<TInput> Context { get; }
    //}

    //public class Context<T>
    //{
    //    public Context(T input)
    //    {
    //        Input = input;
    //    }

    //    public static implicit operator Context<T>(T input)
    //    {
    //        return new Context<T>(input);
    //    }

    //    public T Input { get; private set; }
    //}
}
