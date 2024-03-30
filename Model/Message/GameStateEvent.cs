using System;

namespace u1w_2024_3.Src.Model.Message
{
    public readonly struct GameStateEvent : IEquatable<GameStateEvent>
    {
        public readonly GameState Value;
        
        public GameStateEvent(GameState value)
        {
            Value = value;
        }

        public bool Equals(GameStateEvent other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is GameStateEvent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)Value;
        }
    }
}