using UnityEngine;

namespace Movement.Trajectory
{
    class AllStop : Trajectory2D
    {
        public override bool IsValid => true;

        private readonly Vector2 acceleration;
        private readonly float initialGameTime;
        private readonly Vector2 initialPosition;
        private readonly Vector2 initialVelocity;
        private readonly float finalTimeDelta;
        private readonly Vector2 finalPosition;

        public AllStop(float accelerationMagnitude, float initialTime, Vector2 initialPosition, Vector2 initialVelocity)
        {
            acceleration = initialVelocity.normalized * -accelerationMagnitude;
            initialGameTime = initialTime;
            this.initialPosition = initialPosition;
            this.initialVelocity = initialVelocity;
            finalTimeDelta = CalculateFinalTime(initialVelocity, Vector2.zero, acceleration);
            finalPosition = CalculateDisplacement(finalTimeDelta, initialVelocity, Vector2.zero);
        }
        public override Point GetPoint(float time)
        {
            float t = time - initialGameTime;

            if (t >= finalTimeDelta)
            {
                return new Point(Vector2.zero, finalPosition, true);
            }

            Vector2 velocity = CalculateVelocity(t, initialVelocity, acceleration);
            Vector2 position = initialPosition + CalculateDisplacement(t, initialVelocity, velocity);
            return new Point(position, velocity, false);
        }

        protected static float CalculateFinalTime(Vector2 vi, Vector2 vf, Vector2 a)
        {
            float tx = (vf.x - vi.x) / a.x;
            float ty = (vf.y - vi.y) / a.y;
            return Mathf.Max(tx, ty);
        }

        protected static Vector2 CalculateVelocity(float t, Vector2 vi, Vector2 a)
        {
            return vi + a * t;
        }

        protected static Vector2 CalculateDisplacement(float t, Vector2 vi, Vector2 vf)
        {
            return (vf + vi) * 0.5f * t;
        }
    }
}
