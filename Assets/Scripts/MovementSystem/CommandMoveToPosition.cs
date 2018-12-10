using CommandSystem;
using System;
using UnityEngine;

namespace MovementSystem
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
            //trajectoryY = new Trajectory1D(movementComponent.maxAcceleration, Time.time, initialPosition3D.y, movementComponent.currentVelocity.y, targetPosition.y);
            Debug.Log(trajectoryX.GetInfo());

            if (trajectoryX.InvalidTrajectory)
            {
                Debug.Log("No X trajectory solutions found");
            }

            return !(trajectoryX.InvalidTrajectory);
        }

        public override bool Update(CommandQueueComponent commandQueueComponent)
        {
            Vector3 newPosition = commandQueueComponent.gameObject.transform.position;
            bool @continue = true;
            if (trajectoryX == null)
            {
                return false;
            }

            TrajectoryPoint positionX = trajectoryX.GetTrajectoryPoint(Time.time);
            newPosition.x = positionX.Position;
            if (positionX.Complete)
            {
                trajectoryX = null;
            }
            else if (positionX.Invalid)
            {
                @continue = false;
            }

            commandQueueComponent.gameObject.transform.position = newPosition;
            movementComponent.currentVelocity = new Vector2(positionX.Velocity, 0);
            return @continue;
        }
    }
}
