using GameEvents.Actions;
using System.Collections.Generic;
using UnitTask.Build;
using UnityEngine;

namespace UnitTask
{
    [RequireComponent(typeof(Cargo.CargoComponent))]
    class AvailableBuildingsComponent : MonoBehaviour
    {
        public BuildingType[] BuildingTypes = new BuildingType[0];

        public BuildingType[] GetBuildings(Vector2 mousePosition)
        {
            List<BuildingType> buildings = new List<BuildingType>();
            foreach (BuildingType building in BuildingTypes)
            {
                switch (building.Building)
                {
                    case BuildActionRequested.BuildingType.Drill:
                        bool found = Drill.GetDrillableAsteroid(Camera.main.ScreenToWorldPoint(mousePosition)) != default(AsteroidComponent);
                        if (found)
                        {
                            buildings.Add(building);
                        }
                        break;
                    default:
                        buildings.Add(building);
                        break;
                }
            }
            return buildings.ToArray();
        }
    }
}
