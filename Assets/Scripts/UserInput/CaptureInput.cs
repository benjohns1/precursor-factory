using GameEvents;
using GameEvents.UserInput;
using UnityEngine;
using static UserInput.InputMappings;

namespace UserInput
{
    internal class CaptureInput
    {
        internal InputMappings mappings;

        private bool gameMouseMode = true;

        internal CaptureInput(InputMappings mappings)
        {
            this.mappings = mappings;

            GameManager.EventSystem.Subscribe(typeof(GameEvents.UI.MouseCaptured), HandleMouseCapturedEvent);
            GameManager.EventSystem.Subscribe(typeof(GameEvents.UI.MouseReleased), HandleMouseReleasedEvent);

            // Send initial modifier key values
            foreach (InputMap input in mappings.ModifierKeys)
            {
                GameManager.EventSystem.Publish(new KeyCaptured(input.Key, Input.GetKey(input.Key) ? KeyCaptured.ActionType.Pressed : KeyCaptured.ActionType.Released));
            }
        }

        private void HandleMouseReleasedEvent(IEvent @event)
        {
            gameMouseMode = true;
        }

        private void HandleMouseCapturedEvent(IEvent @event)
        {
            gameMouseMode = false;
        }

        internal void CaptureLoop()
        {
            // Mouse button clicks
            foreach (InputMap input in mappings.MouseActions)
            {
                if (Input.GetKeyDown(input.Key))
                {
                    GameManager.EventSystem.Publish(new MouseEventCaptured(input.Key, KeyCaptured.ActionType.Pressed, Input.mousePosition, gameMouseMode));
                }
            }

            // Modifier keys
            foreach (InputMap input in mappings.ModifierKeys)
            {
                if (Input.GetKeyDown(input.Key))
                {
                    GameManager.EventSystem.Publish(new KeyCaptured(input.Key, KeyCaptured.ActionType.Pressed));
                }
                if (Input.GetKeyUp(input.Key))
                {
                    GameManager.EventSystem.Publish(new KeyCaptured(input.Key, KeyCaptured.ActionType.Released));
                }
            }

            // Standard keys
            foreach (InputMap input in mappings.StandardKeys)
            {
                if (Input.GetKeyDown(input.Key))
                {
                    GameManager.EventSystem.Publish(new KeyCaptured(input.Key, KeyCaptured.ActionType.Pressed));
                }
                if (Input.GetKeyUp(input.Key))
                {
                    GameManager.EventSystem.Publish(new KeyCaptured(input.Key, KeyCaptured.ActionType.Released));
                }
            }
        }
    }
}
