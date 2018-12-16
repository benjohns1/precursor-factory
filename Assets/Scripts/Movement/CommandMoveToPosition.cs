using UnitCommand;
using System;
using UnityEngine;

namespace Movement
{
    class CommandMoveToPosition : Command
    {
        protected Vector2 targetPosition;

        protected Trajectory1D trajectoryX;
        protected Trajectory1D trajectoryY;

        protected MovementComponent movementComponent;

        public CommandMoveToPosition(Vector2 position) : base()
        {
            targetPosition = position;
        }

        public override bool Start(CommandQueueComponent commandQueueComponent)
        {
            movementComponent = commandQueueComponent.GetComponent<MovementComponent>();
            Vector3 initialPosition3D = movementComponent.gameObject.transform.position;

            trajectoryX = new Trajectory1D(movementComponent.maxAcceleration, Time.time, initialPosition3D.x, movementComponent.currentVelocity.x, targetPosition.x);
            trajectoryY = new Trajectory1D(movementComponent.maxAcceleration, Time.time, initialPosition3D.y, movementComponent.currentVelocity.y, targetPosition.y);

            bool invalid = trajectoryX.InvalidTrajectory || trajectoryY.InvalidTrajectory;
            if (invalid)
            {
                Debug.Log("No trajectory solutions found");
            }

            return !invalid;
        }

        public override bool Update(CommandQueueComponent commandQueueComponent)
        {
            Vector3 newPosition = commandQueueComponent.gameObject.transform.position;
            bool @continue = true;

            Vector2 newVelocity = new Vector2(movementComponent.currentVelocity.x, movementComponent.currentVelocity.y);

            if (trajectoryX == null && trajectoryY == null)
            {
                return false;
            }

            if (trajectoryX != null)
            {
                TrajectoryPoint positionX = trajectoryX.GetTrajectoryPoint(Time.time);
                newVelocity.x = positionX.Velocity;
                newPosition.x = positionX.Position;
                if (positionX.Complete)
                {
                    trajectoryX = null;
                }
                else if (positionX.Invalid)
                {
                    @continue = false;
                }
            }

            if (trajectoryY != null)
            {
                TrajectoryPoint positionY = trajectoryY.GetTrajectoryPoint(Time.time);
                newVelocity.y = positionY.Velocity;
                newPosition.y = positionY.Position;
                if (positionY.Complete)
                {
                    trajectoryY = null;
                }
                else if (positionY.Invalid)
                {
                    @continue = false;
                }
            }

            commandQueueComponent.gameObject.transform.position = newPosition;
            movementComponent.currentVelocity = newVelocity;
            return @continue;
        }
    }
}
