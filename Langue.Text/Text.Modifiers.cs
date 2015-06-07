using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langue
{
    public static partial class Text
    {
        public static Pattern<IEnumerable<T>, TextContext> Delimit<T>(this Pattern<T, TextContext> self, Char delimiter) 
            => self.Delimit(Char(delimiter));

        public static Pattern<IEnumerable<T>, TextContext> Delimit<T>(this Pattern<T, TextContext> self, String delimiter)
            => self.Delimit(Literal(delimiter));

        public static Pattern<IEnumerable<T>, TextContext> Delimit<T, TDelimiter>(this Pattern<T, TextContext> self, Pattern<TDelimiter, TextContext> delimiter) =>
            from first in self
            from remaining in
                (
                    from d in delimiter
                    from item in self
                    select item
                ).ZeroOrMore("zero or more")
            select new[] { first }.Concat(remaining);

        public static Pattern<IEnumerable<T>, TextContext> ZeroOrMore<T>(this Pattern<T, TextContext> self, string description)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            return context =>
            {
                var ctx = context;
                var result = new List<T>();
                var m = self(ctx);
                while (m.HasValue)
                {
                    result.Add(m.Value);
                    ctx = m.Context;
                    m = self(ctx);
                }

                return Match.Success(result, ctx, description);
            };
        }
    }
}
