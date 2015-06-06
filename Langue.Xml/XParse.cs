using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
//using System.Xml.XPath;

namespace Langue
{
    //public static class XParse
    //{
    //    public static XParser<string> Attribute(string name)
    //    {
    //        return ctx =>
    //        {
    //            var attribute = ctx.Input.Attribute(name);
    //            return attribute != null 
    //                ? Result.WithValue(attribute.Value, ctx) 
    //                : Result.WithoutValue<String, XElement>(ctx);
    //        };
    //    }

    //    public static XParser<String> Attribute(string xpath, string name)
    //    {
    //        return ctx =>
    //        {
    //            var el = ctx.Input.XPathSelectElement(xpath);
    //            if (el == null)
    //                return Result.WithoutValue<String, XElement>(ctx);

    //            var attribute = el.Attribute(name);
    //            return attribute != null
    //                ? Result.WithValue(attribute.Value, ctx)
    //                : Result.WithoutValue<String, XElement>(ctx);
    //        };
    //    }

    //    public static XParser<IEnumerable<string>> Attributes(string name)
    //    {
    //        return ctx =>
    //        {
    //            var results = ctx.Input.Attributes(name)
    //                .Select(x => x.Value)
    //                .ToArray();

    //            return Result.WithValue(results, ctx);
    //        };
    //    }

    //    public static XParser<IEnumerable<String>> Attributes(string xpath, string name)
    //    {
    //        return ctx =>
    //        {
    //            var results = ctx.Input.XPathSelectElements(xpath)
    //                .Attributes(name)
    //                .Select(x => x.Value);

    //            return Result.WithValue(results, ctx);
    //        };
    //    }

    //    public static XParser<String> Content(string xpath)
    //    {
    //        return ctx =>
    //        {
    //            var el = ctx.Input.XPathSelectElement(xpath);
    //            return el != null 
    //                ? Result.WithValue(el.Value, ctx) 
    //                : Result.WithoutValue<String, XElement>(ctx);
    //        };
    //    }

    //    public static XParser<IEnumerable<String>> Contents(string xpath)
    //    {
    //        return ctx =>
    //        {
    //            var els = ctx.Input.XPathSelectElements(xpath);
    //            return Result.WithValue(els.Select(x => x.Value).ToArray(), ctx);
    //        };
    //    }

    //    public static XParser<T> Element<T>(string xpath, XParser<T> body)
    //    {
    //        return ctx =>
    //        {
    //            var el = ctx.Input.XPathSelectElement(xpath);
    //            if(el == null)
    //                return Result.WithoutValue<T, XElement>(ctx);

    //            var result = body(new Context<XElement>(el));
    //            if (result.HasValue)
    //                return Result.WithValue(result.Value, ctx);
                
    //            return Result.WithoutValue<T, XElement>(new Context<XElement>(ctx.Input));
    //        };
    //    }

    //    public static XParser<IEnumerable<T>> Elements<T>(string xpath, XParser<T> body)
    //    {
    //        return ctx =>
    //        {
    //            var els = ctx.Input.XPathSelectElements(xpath);

    //            var results = new List<T>();

    //            foreach (var el in els)
    //            {
    //                var result = body(new Context<XElement>(el));
    //                if (result.HasValue)
    //                    results.Add(result.Value);
    //                else
    //                    return Result.WithoutValue<IEnumerable<T>, XElement>(ctx);
    //            }

    //            return Result.WithValue(results.ToArray(), ctx);
    //        };
    //    }

    //    public static XParser<T> Where<T>(this XParser<T> @this, Func<T, bool> predicate)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            if (!result.HasValue)
    //                return result;

    //            return predicate(result.Value) 
    //                ? result
    //                : Result.WithoutValue<T, XElement>(ctx);
    //        };
    //    }

    //    public static XParser<IEnumerable<T>> Where<T>(this XParser<IEnumerable<T>> @this, Func<T, bool> predicate)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            if (!result.HasValue)
    //                return result;

    //            return Result.WithValue(result.Value.Where(predicate), ctx);
    //        };
    //    }

    //    public static XParser<T> Unless<T>(this XParser<T> @this, Func<T, bool> predicate)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            if (!result.HasValue)
    //                return result;

    //            return !predicate(result.Value)
    //                ? result
    //                : Result.WithoutValue<T, XElement>(ctx);
    //        };
    //    }

    //    public static XParser<Maybe<T>> Optional<T>(this XParser<T> @this)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return Result.WithValue(result.HasValue 
    //                ? result.Value.ToMaybe() 
    //                : Maybe<T>.NoValue, ctx);
    //        };
    //    }

    //    public static XParser<TResult> Select<T, TResult>(this XParser<T> @this, Func<T, TResult> selector)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return result.HasValue 
    //                ? Result.WithValue(selector(result.Value), ctx) 
    //                : Result.WithoutValue<TResult, XElement>(ctx);
    //        };
    //    }

    //    public static XParser<TResult> SelectMany<T, TResult>(this XParser<T> @this, Func<T, XParser<TResult>> selector)
    //    {
    //        return ctx =>
    //        {
    //            var result = @this(ctx);
    //            return result.HasValue
    //                ? selector(result.Value)(result.Context) 
    //                : Result.WithoutValue<TResult, XElement>(ctx);
    //        };
    //    }

    //    public static XParser<TResult> SelectMany<T, TIntermediate, TResult>(this XParser<T> @this,
    //                                                                        Func<T, XParser<TIntermediate>> selector,
    //                                                                        Func<T, TIntermediate, TResult> combiner)
    //    {
    //        return SelectMany(@this, x => selector(x).Select(y => combiner(x, y)));
    //    }
    //}
}
