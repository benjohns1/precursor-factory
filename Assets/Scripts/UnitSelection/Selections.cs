using GameEvents.UnitSelection;
using System.Collections.Generic;

namespace UnitSelection
{
    internal class Selections
    {
        private readonly List<SelectableComponent> Selected = new List<SelectableComponent>();

        public void Select(SelectableComponent selection, bool multi)
        {
            if (!multi)
            {
                Clear();
            }
            if (selection == null)
            {
                return;
            }
            Selected.Add(selection);
            selection.Select();
            GameManager.EventSystem.Publish(new UnitSelected(selection, multi ? UnitSelected.SelectionType.Multi : UnitSelected.SelectionType.Single));
        }

        public void Deselect(SelectableComponent selection, bool multi)
        {
            if (!multi)
            {
                Clear();
                return;
            }
            if (Selected.Contains(selection))
            {
                Selected.Remove(selection);
                selection.Deselect();
                GameManager.EventSystem.Publish(new UnitDeselected(selection, multi ? UnitDeselected.SelectionType.Multi : UnitDeselected.SelectionType.Single));
            }
        }

        public void Clear()
        {
            if (Selected.Count <= 0)
            {
                return;
            }
            foreach (SelectableComponent selected in Selected)
            {
                selected.Deselect();
            }
            Selected.Clear();
            GameManager.EventSystem.Publish(new UnitSelectionCleared());
        }

        public bool IsSelected(SelectableComponent selectable)
        {
            return selectable == null ? false : Selected.Contains(selectable);
        }

        public List<SelectableComponent> GetSelections()
        {
            return Selected;
        }
    }
}
