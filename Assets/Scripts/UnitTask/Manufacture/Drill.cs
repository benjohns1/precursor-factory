using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitTask.Manufacture
{
    public class Drill : ITask
    {
        public TaskQueue TaskQueue { get; protected set; }

        public Drill(TaskQueue taskQueue)
        {
            TaskQueue = taskQueue;
        }

        public static AsteroidComponent GetDrillableAsteroid(Vector2 worldPosition)
        {
            Vector3 start = new Vector3(worldPosition.x, worldPosition.y, Camera.main.transform.position.z);
            RaycastHit[] hits = Physics.RaycastAll(start, new Vector3(0, 0, 1), Mathf.Infinity);
            IEnumerable<AsteroidComponent> asteroids = hits.Select(h => h.transform?.gameObject.GetComponent<AsteroidComponent>());
            return asteroids.FirstOrDefault(a => a != null && a.OreAmount.Amount > 0);
        }
    }
}
