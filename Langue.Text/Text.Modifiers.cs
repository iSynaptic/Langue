using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langue
{
    public static partial class Text
    {
        public static Pattern<IEnumerable<T>, TextContext> Delimit<T>(this Pattern<T, TextContext> parser, Char delimiter) 
            => parser.Delimit(Char(delimiter));

        public static Pattern<IEnumerable<T>, TextContext> Delimit<T>(this Pattern<T, TextContext> parser, String delimiter)
            => parser.Delimit(Literal(delimiter));

        public static Pattern<IEnumerable<T>, TextContext> Delimit<T, TDelimiter>(this Pattern<T, TextContext> parser, Pattern<TDelimiter, TextContext> delimiter)
        {
            return from first in parser
                   from remaining in
                       (
                           from d in delimiter
                           from item in parser
                           select item
                       ).ZeroOrMore("many")
                   select new[] { first }.Concat(remaining);
        }

        public static Pattern<IEnumerable<T>, TextContext> ZeroOrMore<T>(this Pattern<T, TextContext> parser, string description)
        {
            if (parser == null) throw new ArgumentNullException(nameof(parser));

            return context =>
            {
                var ctx = context;
                var result = new List<T>();
                var m = parser(ctx);
                while (m.HasValue)
                {
                    result.Add(m.Value);
                    ctx = m.Context;
                    m = parser(ctx);
                }

                return Match.Success(result, description, ctx);
            };
        }
    }
}
