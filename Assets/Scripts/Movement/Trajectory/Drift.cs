using Movement.Trajectory;
using UnityEngine;

namespace Movement
{
    class Drift : Trajectory2D
    {
        public override bool IsValid => true;

        private readonly float initialTime;
        private readonly Vector2 initialPosition;
        private readonly Vector2 velocity;

        public Drift(float initialTime, Vector2 initialPosition, Vector2 velocity)
        {
            this.initialTime = initialTime;
            this.initialPosition = initialPosition;
            this.velocity = velocity;
        }
        public override Point GetPoint(float time)
        {
            float t = time - initialTime;
            Vector2 position = initialPosition + velocity * t;
            return new Point(position, velocity, false);
        }
    }
}
