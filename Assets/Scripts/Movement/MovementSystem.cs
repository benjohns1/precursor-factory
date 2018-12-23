using GameEvents;
using GameEvents.UnitTask;
using Movement.Trajectory;
using System.Collections.Generic;
using UnitTask;
using UnitTask.Movement;
using UnityEngine;
using static Movement.Trajectory2D;

namespace Movement
{
    public class MovementSystem
    {
        internal class Mover
        {
            public Trajectory2D Trajectory;
            public ITask InitialTask;

            public Mover(Trajectory2D trajectory, ITask initialTask = null)
            {
                Trajectory = trajectory;
                InitialTask = initialTask;
            }
        }

        internal Dictionary<MovementComponent, Mover> movers = new Dictionary<MovementComponent, Mover>();

        public MovementSystem()
        {
            GameManager.EventSystem.Subscribe(typeof(UnitTaskEvent), HandleUnitTaskEvent);
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

        private void HandleUnitTaskEvent(IEvent @event)
        {
            UnitTaskEvent taskEvent = @event as UnitTaskEvent;
            if (taskEvent.Task is MoveToPosition)
            {
                MoveToPosition moveToPosition = taskEvent.Task as MoveToPosition;
                MovementComponent movementComponent = GetMovementComponent(moveToPosition);
                if (movementComponent == null)
                {
                    return;
                }

                if (@event is UnitTaskStarted)
                {
                    MoveToPosition(movementComponent, moveToPosition);
                }
                else if (@event is UnitTaskCancelled)
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
            MovementComponent movementComponent = moveToPosition.TaskQueue.TaskQueueComponent.GetComponent<MovementComponent>();
            if (!movers.ContainsKey(movementComponent))
            {
                Debug.LogWarning(typeof(MovementComponent).ToString() + " " + movementComponent.name + " is not registered with " + this.GetType().ToString());
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
                GameManager.EventSystem.Publish(new UnitTaskCompleted(mover.Value.InitialTask));
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
