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
                    SelectActionAtScreenPosition(inputEvent.Position, inputEvent.MultiModifier);
                    break;
            }
        }

        protected void SelectActionAtScreenPosition(Vector2 position, bool multi)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
            SelectableComponent selection = Physics2D.Raycast(worldPosition, Vector2.zero).transform?.gameObject.GetComponent<SelectableComponent>();
            Selections.HandleSelection(selection, multi);
        }
    }
}
