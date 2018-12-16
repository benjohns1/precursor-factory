using UnityEngine;

namespace Movement
{
    public abstract class Trajectory2D
    {
        public struct Point
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public bool Complete;
            public bool Invalid;

            public Point(Vector2 position, Vector2 velocity, bool complete, bool invalid = false)
            {
                Position = position;
                Velocity = velocity;
                Complete = complete;
                Invalid = invalid;
            }
        }

        public abstract Point GetPoint(float time);
        public abstract bool IsValid { get; }
        public virtual string Info => ToString();
    }
}
