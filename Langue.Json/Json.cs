﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace Langue
{
    public static class Json
    {
        private static JToken NavigateTo(this JToken self, string path)
        {
            string[] parts = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                if (self.Type != JTokenType.Object)
                    return null;

                var obj = (JObject)self;

                if (!obj.TryGetValue(part, out self))
                    return null;
            }

            return self;
        }

        private static Pattern<T, JToken> SelectValue<T>(string path, Func<JToken, T> selector) => ctx =>
        {
            var value = ctx.NavigateTo(path);
            if (value == null)
                return Match<T>.Failure(ctx);

            return Match.Success(selector(value), ctx);
        };

        public static Pattern<string, JToken> String() => String("");

        public static Pattern<string, JToken> String(string path)
            => SelectValue(path, x => x.ToObject<string>());

        public static Pattern<DateTime, JToken> Date() => Date("");

        public static Pattern<DateTime, JToken> Date(string path) 
            => SelectValue(path, x => DateTime.Parse(x.ToObject<string>()));

        public static Pattern<Guid, JToken> Guid() => Guid("");

        public static Pattern<Guid, JToken> Guid(string path) 
            => SelectValue(path, x => System.Guid.Parse(x.ToObject<string>()));

        public static Pattern<decimal, JToken> Decimal() => Decimal("");

        public static Pattern<decimal, JToken> Decimal(string path) 
            => SelectValue(path, x => decimal.Parse(x.ToObject<string>()));

        public static Pattern<bool, JToken> Boolean() => Boolean("");

        public static Pattern<bool, JToken> Boolean(string path) 
            => SelectValue(path, x => System.Boolean.Parse(x.ToObject<string>()));

        public static Pattern<int, JToken> Int32() => Int32("");

        public static Pattern<int, JToken> Int32(string path) 
            => SelectValue(path, x => x.ToObject<int>());

        public static Pattern<long, JToken> Int64() => Int64("");

        public static Pattern<long, JToken> Int64(string path)
            => SelectValue(path, x => x.ToObject<long>());

        public static Pattern<T, JToken> Enum<T>() => Enum<T>("");

        public static Pattern<T, JToken> Enum<T>(string path)
            => SelectValue(path, x => (T)System.Enum.Parse(typeof(T), x.ToObject<string>()));

        public static Pattern<IEnumerable<T>, JToken> AsArray<T>(this Pattern<T, JToken> self)
            => Array("", self);

        public static Pattern<IEnumerable<T>, JToken> AsArray<T>(this Pattern<T, JToken> self, string path)
            => Array(path, self);

        public static Pattern<IEnumerable<T>, JToken> Array<T>(string path, Pattern<T, JToken> pattern) => ctx =>
        {
            var value = ctx.NavigateTo(path);
            if (value == null || value.Type != JTokenType.Array)
                return Match<IEnumerable<T>>.Failure(ctx);

            var array = (JArray)value;

            var results = new List<T>();

            foreach (var element in array)
            {
                var item = pattern(element);
                if (item.HasValue)
                    results.Add(item.Value);
                else
                    return Match<IEnumerable<T>>.Failure(ctx);
            }

            return Match.Success(results.ToArray(), ctx);
        };

        public static Pattern<IEnumerable<T>, JToken> OneOrMore<T>(string path, Pattern<T, JToken> itemParser) => ctx =>
        {
            var value = ctx.NavigateTo(path);
            if (value == null)
                return Match<IEnumerable<T>>.Failure(ctx);

            if (value.Type == JTokenType.Array)
            {
                var array = (JArray)value;
                var results = new List<T>();

                foreach (var element in array)
                {
                    var item = itemParser(element);
                    if (item.HasValue)
                        results.Add(item.Value);
                    else
                        return Match<IEnumerable<T>>.Failure(ctx);
                }

                return Match.Success(results.ToArray(), ctx);
            }

            return itemParser.Select(x => new[] { x })(value);
        };

        public static Pattern<T, JToken> Object<T>(string path, Pattern<T, JToken> body) => ctx =>
        {
            var value = ctx.NavigateTo(path);
            if (value == null || value.Type != JTokenType.Object)
                return Match<T>.Failure(ctx);

            var doc = (JObject)value;
            return body(doc).WithContext(ctx);
        };

        public static Pattern<IEnumerable<T>, JToken> HashArray<T>(string path, Func<string, Pattern<T, JToken>> selector) => ctx =>
        {
            var value = ctx.NavigateTo(path);
            if (value == null || value.Type != JTokenType.Object)
                return Match<IEnumerable<T>>.Failure(ctx);

            var results = new List<T>();

            var doc = (JObject)value;
            foreach (var property in doc.Properties())
            {
                var itemPattern = selector(property.Name);
                var item = itemPattern(property.Value);

                if (item.HasValue)
                    results.Add(item.Value);
                else
                    return Match<IEnumerable<T>>.Failure(ctx);
            }

            return Match.Success(results.ToArray(), ctx);
        };
    }
}
