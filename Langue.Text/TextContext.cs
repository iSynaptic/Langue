using System;

namespace Langue
{
    public class TextContext
    {
        public readonly Position ConsumedTo;
        public readonly Position ReadTo;

        public readonly string Input;
        public readonly string Source;

        public bool AtEnd => Input.Length == ReadTo.Index;
        public char Current => !AtEnd ? Input[ReadTo.Index] : default(char);

        public readonly Pattern<object, TextContext> Interleaving;

        public TextContext(string input)
            : this(input, null)
        {
        }

        public TextContext(string input, string source)
            : this(input, source, null)
        {
        }

        public TextContext(string input, string source, Pattern<object, TextContext> interleaving)
            : this(input, source, interleaving, default(Position), default(Position))
        {
        }

        private TextContext(string input, string source, Pattern<object, TextContext> interleaving, Position consumedTo, Position readTo)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            Input = input;
            Source = source ?? "";
            Interleaving = interleaving;

            ConsumedTo = consumedTo;
            ReadTo = readTo;
        }

        public TextContext Read(int count) => Move(count, false);
        public TextContext ReadAndConsume(int count) => Move(count, true);
        public TextContext Consume() => Move(0, true);

        private TextContext Move(int count, bool consume)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0 && consume && ConsumedTo == ReadTo)
                return this;

            int newIndex = ReadTo.Index + count;

            if (AtEnd || newIndex > Input.Length)
                throw new ArgumentException(String.Format("Advancing {0} charaters exceeds the length of the input.", count), nameof(count));

            var newReadTo = ReadTo;
            while (newReadTo.Index < newIndex)
            {
                newReadTo = Input[newReadTo.Index] == '\n'
                    ? newReadTo.AdvanceWithNewLine()
                    : newReadTo.Advance();
            }

            return new TextContext(Input, Source, Interleaving, consume ? newReadTo : ConsumedTo, newReadTo);
        }

        public TextContext WithInterleave(Pattern<Object, TextContext> interleaving)
            => new TextContext(Input, Source, interleaving, ConsumedTo, ReadTo);

        public static implicit operator TextContext(string content) 
            => new TextContext(content, null);
    }
}