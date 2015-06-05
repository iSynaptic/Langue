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
        {
            return parser.Delimit(Char(delimiter));
        }

        public static Pattern<IEnumerable<T>, TextContext> Delimit<T>(this Pattern<T, TextContext> parser, String delimiter)
        {
            return parser.Delimit(Literal(delimiter));
        }

        public static Pattern<IEnumerable<T>, TextContext> Delimit<T, TDelimiter>(this Pattern<T, TextContext> parser, Pattern<TDelimiter, TextContext> delimiter)
        {
            return from first in parser
                   from remaining in
                       (
                           from d in delimiter
                           from item in parser
                           select item
                       ).Many()
                   select new[] { first }.Concat(remaining);
        }

        public static Pattern<IEnumerable<T>, TextContext> Many<T>(this Pattern<T, TextContext> pattern)
        {
            if (pattern == null) throw new ArgumentNullException("parser");
            return i =>
            {
                var remainder = i;
                var result = new List<T>();
                var r = pattern(i);
                while (r.WasSuccessful)
                {
                    if (remainder == r.Remainder)
                        break;
                    result.Add(r.Value);
                    remainder = r.Remainder;
                    r = pattern(remainder);
                }
                return Result.Success<IEnumerable<T>>(result, remainder);
            };
        }
    }
}
