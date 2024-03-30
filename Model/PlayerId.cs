using System;

namespace u1w_2024_3.Src.Model
{
    public class PlayerId : IEquatable<PlayerId>
    {
        public Guid Value { get; }

        public PlayerId(Guid value)
        {
            Value = value;
        }

        public PlayerId()
        {
            Value = Guid.NewGuid();
        }

        public bool Equals(PlayerId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}