using UnityEngine;

namespace GameEvents.Actions
{
    public class BuildActionRequested : IEvent
    {
        public enum BuildingType { None, Drill, Refinery }

        public Vector2 Position { get; protected set; }
        public BuildingType Building { get; protected set; }
        public bool MultiModifier { get; protected set; }

        public BuildActionRequested(Vector2 position, BuildingType building, bool multiModifier)
        {
            Position = position;
            Building = building;
            MultiModifier = multiModifier;
        }
    }
}