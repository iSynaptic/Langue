using System;

namespace Langue
{
    public class TextContext
    {
        public readonly Position ConsumedTo;
        public readonly Position ReadTo;

        public readonly string Input;
        public readonly string Name;

        public bool AtEnd => Input.Length == ReadTo.Index;
        public char Current => !AtEnd ? Input[ReadTo.Index] : default(char);

        public readonly Parser<object> Interleaving;

        public TextContext(string input)
            : this(input, null)
        {
        }

        public TextContext(string input, string name)
            : this(input, name, null)
        {
        }

        public TextContext(string input, string name, Parser<object> interleaving)
            : this(input, name, interleaving, default(Position), default(Position))
        {
        }

        private TextContext(string input, string name, Parser<object> interleaving, Position consumedTo, Position readTo)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            Input = input;
            Name = name ?? "";
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

            return new TextContext(Input, Name, Interleaving, consume ? newReadTo : ConsumedTo, newReadTo);
        }

        public TextContext WithInterleave(Parser<Object> interleaving)
            => new TextContext(Input, Name, interleaving, ConsumedTo, ReadTo);

        public static implicit operator TextContext(string content) 
            => new TextContext(content, null);
    }
}