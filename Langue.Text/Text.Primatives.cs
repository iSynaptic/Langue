using System;

namespace Langue
{
    public static partial class Text
    {
        public static Pattern<char, TextContext> AnyChar = context =>
        {
            return context.AtEnd
                ? Match<char>.Failure("any character", context, new ParseError("Unexpected: end of input", context.ReadTo.ToRange()))
                : Match.Success(context.Current, "any character", context.ReadAndConsume(1));
        };

        public static readonly Pattern<string, TextContext> Empty =
            ctx => Match.Success("", "empty", ctx);

        public static Pattern<char, TextContext> Char(char c)
        {
            return context =>
            {
                if (context.AtEnd)
                    return Match<char>.Failure("character ", context, new ParseError("Unexpected: end of input", context.ReadTo.ToRange()));

                return null;
            };
        }

        public static Pattern<string, TextContext> Literal(string expected) => Literal(expected, "literal");

        public static Pattern<string, TextContext> Literal(string expected, string description)
        {
            if (expected == null) throw new ArgumentNullException(nameof(expected));
            if (expected == string.Empty) return Empty;

            return context =>
            {
                if(context.AtEnd)
                    return Match<string>.Failure(description, context, new ParseError("Unexpected: end of input", context.ReadTo.ToRange()));

                string input = context.Input;
                int offset = context.ConsumedTo.Index;
                int length = expected.Length;

                for (int i = 0; i < length; i++)
                {
                    if (i == input.Length)
                    {
                        var newContext = context.Read(i);
                        return Match<string>.Failure(description, newContext, new ParseError("Unexpected: end of input", newContext.ReadTo.ToRange()));
                    }

                    if (input[offset + i] != expected[i])
                    {
                        var newContext = context.Read(i);
                        return Match<string>.Failure(description, newContext, new ParseError($"Unexpected: character '{input[offset + i]}'", newContext.ReadTo.ToRange()));
                    }
                }

                return Match.Success(expected, description, context.ReadAndConsume(length));
            };
        }
    }
}