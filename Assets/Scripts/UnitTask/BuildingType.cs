using GameEvents.Actions;

namespace UnitTask
{
    [System.Serializable]
    class BuildingType
    {
        public BuildActionRequested.BuildingType Building = BuildActionRequested.BuildingType.None;
        public string DisplayName = null;
    }
}
