using GameEvents;
using System.Collections.Generic;
using UnityEngine;
using UserInput;

namespace UnitSelection
{
    public class SelectionSystem
    {
        private readonly Selections Selections = new Selections();

        public SelectionSystem()
        {
            GameManager.EventSystem.Subscribe(new Topic("InputAction"), HandleInputEvent);
        }

        public bool IsSelected(SelectableComponent selectable)
        {
            return Selections.IsSelected(selectable);
        }

        protected void HandleInputEvent(IEvent @event)
        {
            if (!(@event is InputAction))
            {
                return;
            }

            InputAction inputEvent = @event as InputAction;
            switch (inputEvent.Action)
            {
                case InputAction.ActionType.SelectAtPosition:
                    SelectActionAtPosition(inputEvent.Position, false);
                    break;
                case InputAction.ActionType.MultiSelectAtPosition:
                    SelectActionAtPosition(inputEvent.Position, true);
                    break;
            }
        }

        protected void SelectActionAtPosition(Vector2 position, bool multi)
        {
            SelectableComponent selection = Physics2D.Raycast(position, Vector2.zero).transform?.gameObject.GetComponent<SelectableComponent>();
            if (IsSelected(selection))
            {
                Selections.Deselect(selection, multi);
            }
            else
            {
                Selections.Select(selection, multi);
            }
        }
    }
}
