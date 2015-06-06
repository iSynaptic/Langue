using System;

namespace Langue
{
    public struct LocationRange : IEquatable<LocationRange>
    {
        public LocationRange(Location start, Location end)
        {
            if (end < start)
                throw new ArgumentOutOfRangeException(nameof(end), "End location must not be before the start location.");

            Start = start;
            End = end;
        }

        public bool Equals(LocationRange other) 
            => Start.Equals(other.Start) &&
               End.Equals(other.End);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (GetType() != obj.GetType()) return false;

            return Equals((LocationRange)obj);
        }

        public override int GetHashCode()
        {
            int hash = 1;

            hash = Hashing.MixJenkins32(hash + Start.GetHashCode());
            hash = Hashing.MixJenkins32(hash + End.GetHashCode());

            return hash;
        }

        public static Boolean operator ==(LocationRange left, LocationRange right)
            => left.Equals(right);

        public static Boolean operator !=(LocationRange left, LocationRange right)
            => !(left.Equals(right));

        public Location Start { get; }
        public Location End { get; }
    }
}
