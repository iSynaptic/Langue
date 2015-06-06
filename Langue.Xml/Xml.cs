using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Langue
{
    public static class Xml
    {
        public static Pattern<string, XElement> Attribute(string name)
        {
            return ctx =>
            {
                var attribute = ctx.Attribute(name);
                return attribute != null
                    ? Match.Success(attribute.Value, ctx, "")
                    : Match<String>.Failure(ctx, "");
            };
        }

        public static Pattern<string, XElement> Attribute(string xpath, string name)
        {
            return ctx =>
            {
                var el = ctx.XPathSelectElement(xpath);
                if (el == null)
                    return Match<String>.Failure(ctx, "");

                var attribute = el.Attribute(name);
                return attribute != null
                    ? Match.Success(attribute.Value, ctx, "")
                    : Match<String>.Failure(ctx, "");
            };
        }

        public static Pattern<IEnumerable<string>, XElement> Attributes(string name)
        {
            return ctx =>
            {
                var results = ctx.Attributes(name)
                    .Select(x => x.Value)
                    .ToArray();

                return Match.Success(results, ctx, "");
            };
        }

        public static Pattern<IEnumerable<String>, XElement> Attributes(string xpath, string name)
        {
            return ctx =>
            {
                var results = ctx.XPathSelectElements(xpath)
                    .Attributes(name)
                    .Select(x => x.Value);

                return Match.Success(results, ctx, "");
            };
        }

        public static Pattern<String, XElement> Content(string xpath)
        {
            return ctx =>
            {
                var el = ctx.XPathSelectElement(xpath);
                return el != null
                    ? Match.Success(el.Value, ctx, "")
                    : Match<String>.Failure(ctx, "");
            };
        }

        public static Pattern<IEnumerable<String>, XElement> Contents(string xpath)
        {
            return ctx =>
            {
                var els = ctx.XPathSelectElements(xpath);
                return Match.Success(els.Select(x => x.Value).ToArray(), ctx, "");
            };
        }

        public static Pattern<T, XElement> Element<T>(string xpath, Pattern<T, XElement> body)
        {
            return ctx =>
            {
                var el = ctx.XPathSelectElement(xpath);
                if (el == null)
                    return Match<T>.Failure(ctx, "");

                var result = body(el);
                if (result.HasValue)
                    return Match.Success(result.Value, ctx, "");

                return Match<T>.Failure(ctx, "");
            };
        }

        public static Pattern<IEnumerable<T>, XElement> Elements<T>(string xpath, Pattern<T, XElement> body)
        {
            return ctx =>
            {
                var els = ctx.XPathSelectElements(xpath);

                var results = new List<T>();

                foreach (var el in els)
                {
                    var result = body(el);
                    if (result.HasValue)
                        results.Add(result.Value);
                    else
                        return Match<IEnumerable<T>>.Failure(ctx, "");
                }

                return Match.Success(results.ToArray(), ctx, "");
            };
        }
    }
}
