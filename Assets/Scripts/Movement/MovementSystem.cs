using GameEvents;
using GameEvents.UnitCommand;
using System.Collections.Generic;
using UnitCommand;
using UnitCommand.Movement;
using UnityEngine;
using static Movement.Trajectory2D;

namespace Movement
{
    public class MovementSystem
    {
        internal class Mover
        {
            public Trajectory2D Trajectory;
            public ICommand InitialCommand;

            public Mover(Trajectory2D trajectory, ICommand initialCommand = null)
            {
                Trajectory = trajectory;
                InitialCommand = initialCommand;
            }
        }

        internal Dictionary<MovementComponent, Mover> movers = new Dictionary<MovementComponent, Mover>();

        public MovementSystem()
        {
            GameManager.EventSystem.Subscribe(typeof(UnitCommandEvent), HandleMoveToPosition);
        }

        public void Register(MovementComponent mover)
        {
            if (!movers.ContainsKey(mover))
            {
                movers.Add(mover, new Mover(new Drift(Time.time, mover.transform.position, mover.currentVelocity)));
            }
        }

        public void Unregister(MovementComponent mover)
        {
            if (movers.ContainsKey(mover))
            {
                movers.Remove(mover);
            }
        }

        private void HandleMoveToPosition(IEvent @event)
        {
            if (!(@event is UnitCommandEvent))
            {
                return;
            }

            UnitCommandEvent commandEvent = @event as UnitCommandEvent;
            if (commandEvent.Command is MoveToPosition)
            {
                MoveToPosition moveToPosition = commandEvent.Command as MoveToPosition;
                MovementComponent movementComponent = GetMovementComponent(moveToPosition);
                if (movementComponent == null)
                {
                    return;
                }

                if (@event is UnitCommandStarted)
                {
                    MoveToPosition(movementComponent, moveToPosition);
                }
                else if (@event is UnitCommandCancelled)
                {
                    Drift(movementComponent);
                }
            }
        }

        private void MoveToPosition(MovementComponent movementComponent, MoveToPosition moveToPosition)
        {
            Brachistochrone2D trajectory = new Brachistochrone2D(movementComponent.maxAcceleration, Time.time, movementComponent.gameObject.transform.position, movementComponent.currentVelocity, moveToPosition.TargetPosition);

            if (!trajectory.IsValid)
            {
                Debug.LogWarning("No Brachistochrone2D solutions found");
                AllStop(movementComponent);
                return;
            }

            movers[movementComponent] = new Mover(trajectory, moveToPosition);
        }

        private void Drift(MovementComponent movementComponent)
        {
            Drift trajectory = new Drift(Time.time, movementComponent.gameObject.transform.position, movementComponent.currentVelocity);

            if (!trajectory.IsValid)
            {
                Debug.LogError("Drift trajectory is invalid!");
                AllStop(movementComponent);
                return;
            }

            movers[movementComponent] = new Mover(trajectory);
        }

        private void AllStop(MovementComponent movementComponent)
        {
            AllStop trajectory = new AllStop(movementComponent.maxAcceleration, Time.time, movementComponent.gameObject.transform.position, movementComponent.currentVelocity);

            if (!trajectory.IsValid)
            {
                Debug.LogError("AllStop trajectory is invalid!");
                return;
            }

            movers[movementComponent] = new Mover(trajectory);
        }

        private MovementComponent GetMovementComponent(MoveToPosition moveToPosition)
        {
            MovementComponent movementComponent = moveToPosition.CommandQueue.CommandQueueComponent.GetComponent<MovementComponent>();
            if (!movers.ContainsKey(movementComponent))
            {
                Debug.LogWarning("MovementComponent " + movementComponent.name + " is not registered with MovementSystem");
                return null;
            }
            return movementComponent;
        }

        public void Update()
        {
            List<KeyValuePair<MovementComponent, Mover>> trajectoryComplete = new List<KeyValuePair<MovementComponent, Mover>>();
            foreach (KeyValuePair<MovementComponent, Mover> mover in movers)
            {
                if (!UpdateMover(mover.Key, mover.Value.Trajectory))
                {
                    trajectoryComplete.Add(mover);
                }
            }

            // Default to drifting when any trajectories are completed
            foreach (KeyValuePair<MovementComponent, Mover> mover in trajectoryComplete)
            {
                GameManager.EventSystem.Publish(new UnitCommandCompleted(mover.Value.InitialCommand));
                Drift(mover.Key);
            }
        }

        private bool UpdateMover(MovementComponent mover, Trajectory2D trajectory)
        {
            if (trajectory == null || !trajectory.IsValid)
            {
                return false;
            }

            Point point = trajectory.GetPoint(Time.time);
            if (point.Complete)
            {
                trajectory = null;
            }
            if (point.Invalid)
            {
                return false;
            }

            mover.gameObject.transform.position = point.Position;
            mover.currentVelocity = point.Velocity;
            return !point.Complete;
        }
    }
}
