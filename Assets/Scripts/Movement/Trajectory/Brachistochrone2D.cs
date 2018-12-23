using UnityEngine;

namespace Movement.Trajectory
{
    class Brachistochrone2D : Trajectory2D
    {
        public override bool IsValid => trajectoryX.IsValid && trajectoryY.IsValid;
        public override string Info => string.Format("X:  {0}\nY:  {1}\n", trajectoryX.Info, trajectoryY.Info);

        private Vector2 targetPosition;
        private Brachistochrone1D trajectoryX;
        private Brachistochrone1D trajectoryY;

        public Brachistochrone2D(float accelerationMagnitude, float initialTime, Vector2 initialPosition, Vector2 initialVelocity, Vector2 targetPosition, Vector2? targetVelocity = null)
        {
            Vector2 targetVelocityWithDefault = targetVelocity ?? Vector2.zero;
            trajectoryX = new Brachistochrone1D(accelerationMagnitude, initialTime, initialPosition.x, initialVelocity.x, targetPosition.x, targetVelocityWithDefault.x);
            trajectoryY = new Brachistochrone1D(accelerationMagnitude, initialTime, initialPosition.y, initialVelocity.y, targetPosition.y, targetVelocityWithDefault.y);
        }
        public override Point GetPoint(float time)
        {
            if (!IsValid)
            {
                return new Point(Vector2.zero, Vector2.zero, true, true);
            }

            Brachistochrone1D.Point x = trajectoryX.GetPoint(time);
            Brachistochrone1D.Point y = trajectoryY.GetPoint(time);

            return new Point(new Vector2(x.Position, y.Position), new Vector2(x.Velocity, y.Velocity), x.Complete && y.Complete);
        }

    }
}
