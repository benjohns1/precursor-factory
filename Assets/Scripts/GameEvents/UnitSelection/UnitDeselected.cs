namespace GameEvents.UnitSelection
{
    public class UnitDeselected : UnitSelectionChanged
    {
        public enum SelectionType { Single, Multi }

        public SelectableComponent Deselected { get; protected set; }
        public SelectionType Type { get; protected set; }

        public UnitDeselected(SelectableComponent deselected, SelectionType selectionType)
        {
            Deselected = deselected;
            Type = selectionType;
        }
    }
}