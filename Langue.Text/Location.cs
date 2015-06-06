using System;

namespace Langue
{
    public struct Location : IEquatable<Location>, IComparable<Location>
    {
        public static readonly Location Beginning = new Location(0, 1, 1);

        private readonly int _index;
        private readonly int _line;
        private readonly int _column;

        public Location(int index, int line, int column)
            : this()
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Index must not be negative.");
            if (line <= 0) throw new ArgumentOutOfRangeException(nameof(line), "Line must be greater than or equal to one.");
            if (column <= 0) throw new ArgumentOutOfRangeException(nameof(column), "Column must be greater than or equal to one.");

            _index = index;
            _line = line;
            _column = column;
        }

        public Location Advance() 
            => new Location(Index + 1, Line, Column + 1);

        public Location AdvanceWithNewLine()
            => new Location(Index + 1, Line + 1, 1);

        public bool Equals(Location other)
            => Index == other.Index &&
               Line == other.Line &&
               Column == other.Column;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (GetType() != obj.GetType()) return false;

            return Equals((Location)obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;

            hash = Hashing.MixJenkins32(hash + Index);
            hash = Hashing.MixJenkins32(hash + Line);
            hash = Hashing.MixJenkins32(hash + Column);

            return hash;
        }

        public LocationRange ToRange() => new LocationRange(this, this);

        public int CompareTo(Location other) => Index.CompareTo(other.Index);

        public static Boolean operator ==(Location left, Location right) 
            => left.Equals(right);

        public static Boolean operator !=(Location left, Location right) 
            => !(left.Equals(right));

        public static Boolean operator <(Location left, Location right)
            => left.Index < right.Index;

        public static Boolean operator >(Location left, Location right)
            => left.Index < right.Index;

        public static Boolean operator <=(Location left, Location right)
            => left.Index <= right.Index;

        public static Boolean operator >=(Location left, Location right)
            => left.Index <= right.Index;

        public int Index => _index;
        public int Line => _line == 0 ? 1 : _line;
        public int Column => _column == 0 ? 1 : _column;
    }
}