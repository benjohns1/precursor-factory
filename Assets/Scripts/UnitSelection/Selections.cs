using GameEvents.UnitSelection;
using System.Collections.Generic;
using UnityEngine;

namespace UnitSelection
{
    internal class Selections
    {
        private readonly List<SelectableComponent> Selected = new List<SelectableComponent>();

        public void HandleSelection(SelectableComponent selection, bool multi)
        {
            if (IsSelected(selection) && (Selected.Count == 1 || multi))
            {
                Deselect(selection, multi);
            }
            else
            {
                Select(selection, multi);
            }
        }

        private void Select(SelectableComponent selection, bool multi)
        {
            if (!multi)
            {
                Clear(Selected.Count == 1 ? selection : null);
            }
            if (selection == null)
            {
                return;
            }
            Selected.Add(selection);
            selection.Select();
            GameManager.EventSystem.Publish(new UnitSelected(selection, multi ? UnitSelected.SelectionType.Multi : UnitSelected.SelectionType.Single));
        }

        private void Deselect(SelectableComponent selection, bool multi)
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

        private void Clear(SelectableComponent ignore = null)
        {
            if (Selected.Count <= 0)
            {
                return;
            }
            foreach (SelectableComponent selected in Selected)
            {
                if (selected != ignore)
                {
                    selected.Deselect();
                }
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
