using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
//using System.Xml.XPath;

namespace Langue
{
    public static class XParse
    {
        public static Pattern<string, XElement> Attribute(string name)
        {
            return ctx =>
            {
                var attribute = ctx.Attribute(name);
                return attribute != null
                    ? Match.Success(attribute.Value, "", ctx)
                    : Match<String>.Failure("", ctx);
            };
        }

        public static Pattern<string, XElement> Attribute(string xpath, string name)
        {
            return ctx =>
            {
                var el = ctx.XPathSelectElement(xpath);
                if (el == null)
                    return Result.WithoutValue<String, XElement>(ctx);

                var attribute = el.Attribute(name);
                return attribute != null
                    ? Result.WithValue(attribute.Value, ctx)
                    : Result.WithoutValue<String, XElement>(ctx);
            };
        }

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
    }
}
