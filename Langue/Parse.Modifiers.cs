using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langue
{
    public static partial class Parse
    {
        public static Parser<IEnumerable<T>> Delimit<T>(this Parser<T> parser, Char delimiter)
        {
            return parser.Delimit(Char(delimiter));
        }

        public static Parser<IEnumerable<T>> Delimit<T>(this Parser<T> parser, String delimiter)
        {
            return parser.Delimit(Literal(delimiter));
        }

        public static Parser<IEnumerable<T>> Delimit<T, TDelimiter>(this Parser<T> parser, Parser<TDelimiter> delimiter)
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

    }
}
