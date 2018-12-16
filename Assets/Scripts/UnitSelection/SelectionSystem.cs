using GameEvents;
using GameEvents.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace UnitSelection
{
    public class SelectionSystem
    {
        private readonly Selections Selections = new Selections();

        public SelectionSystem()
        {
            GameManager.EventSystem.Subscribe(typeof(InputActionRequested), HandleInputEvent);
        }

        public bool IsSelected(SelectableComponent selectable)
        {
            return Selections.IsSelected(selectable);
        }

        public List<SelectableComponent> GetSelections()
        {
            return Selections.GetSelections();
        }

        protected void HandleInputEvent(IEvent @event)
        {
            if (!(@event is InputActionRequested))
            {
                return;
            }

            InputActionRequested inputEvent = @event as InputActionRequested;
            switch (inputEvent.Action)
            {
                case InputActionRequested.ActionType.SelectAtPosition:
                    SelectActionAtPosition(inputEvent.Position, false);
                    break;
                case InputActionRequested.ActionType.MultiSelectAtPosition:
                    SelectActionAtPosition(inputEvent.Position, true);
                    break;
            }
        }

        protected void SelectActionAtPosition(Vector2 position, bool multi)
        {
            SelectableComponent selection = Physics2D.Raycast(position, Vector2.zero).transform?.gameObject.GetComponent<SelectableComponent>();
            Selections.HandleSelection(selection, multi);
        }
    }
}
