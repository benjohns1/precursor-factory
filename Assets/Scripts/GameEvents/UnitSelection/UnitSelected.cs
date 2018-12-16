namespace GameEvents.UnitSelection
{
    public class UnitSelected : UnitSelectionChanged
    {
        public enum SelectionType { Single, Multi }

        public SelectableComponent Selection { get; protected set; }
        public SelectionType Type { get; protected set; }

        public UnitSelected(SelectableComponent selection, SelectionType selectionType)
        {
            Selection = selection;
            Type = selectionType;
        }
    }
}