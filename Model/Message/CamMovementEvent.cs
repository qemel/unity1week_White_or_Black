using System;
using UnityEngine;

namespace u1w_2024_3.Src.Model.Message
{
    public sealed class CamMovementEvent : IEquatable<CamMovementEvent>
    {
        public EventType Type { get; }
        public Vector2Int Move { get; }

        public CamMovementEvent(EventType type , Vector2Int move)
        {
            Type = type;
            Move = move;
        }

        public enum EventType
        {
            MoveStart,
            StageFieldMoved,
            MoveEnd
        }

        public bool Equals(CamMovementEvent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && Move.Equals(other.Move);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is CamMovementEvent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Type, Move);
        }
    }
}