using PlayerCamera;
using GameEvents;
using System.Collections.Generic;
using UnityEngine;
using static UserInput.InputMappings;

namespace UserInput
{
    internal enum Action
    {
        MultiModifier,
        FastModifier,
        PanUp,
        PanRight,
        PanDown,
        PanLeft,
        Select,
        MoveTo
    };

    internal class MapInput
    {
        internal bool MultiModifier;
        internal bool FastModifier;

        internal InputMappings mappings;

        internal MapInput(InputMappings mappings)
        {
            this.mappings = mappings;
            GameManager.EventSystem.Subscribe(new Topic("InputCapture"), HandleInputCaptured);
        }

        protected void HandleInputCaptured(IEvent @event)
        {
            if (!(@event is KeyCaptured))
            {
                return;
            }

            if (@event is MouseCaptured)
            {
                MouseCaptured mouseEvent = @event as MouseCaptured;
                MouseInput(mouseEvent);
                return;
            }

            KeyCaptured keyEvent = @event as KeyCaptured;
            KeyboardInput(keyEvent);
        }

        protected void KeyboardInput(KeyCaptured keyEvent)
        {
            List<InputMap> keys = mappings.ModifierKeys.FindAll(key => key.Key == keyEvent.Key);
            keys.AddRange(mappings.StandardKeys.FindAll(key => key.Key == keyEvent.Key));
            foreach (InputMap input in keys)
            {
                switch (input.Action)
                {
                    case Action.MultiModifier:
                        MultiModifier = keyEvent.Action == KeyCaptured.ActionType.Pressed ? true : false;
                        break;
                    case Action.FastModifier:
                        FastModifier = keyEvent.Action == KeyCaptured.ActionType.Pressed ? true : false;
                        break;
                    case Action.PanUp:
                        GameManager.EventSystem.Publish(new CameraPanRequested(CameraPanRequested.Direction.Up, keyEvent.Action == KeyCaptured.ActionType.Pressed ? CameraPanRequested.RequestType.Start : CameraPanRequested.RequestType.Stop, FastModifier));
                        break;
                    case Action.PanRight:
                        GameManager.EventSystem.Publish(new CameraPanRequested(CameraPanRequested.Direction.Right, keyEvent.Action == KeyCaptured.ActionType.Pressed ? CameraPanRequested.RequestType.Start : CameraPanRequested.RequestType.Stop, FastModifier));
                        break;
                    case Action.PanLeft:
                        GameManager.EventSystem.Publish(new CameraPanRequested(CameraPanRequested.Direction.Left, keyEvent.Action == KeyCaptured.ActionType.Pressed ? CameraPanRequested.RequestType.Start : CameraPanRequested.RequestType.Stop, FastModifier));
                        break;
                    case Action.PanDown:
                        GameManager.EventSystem.Publish(new CameraPanRequested(CameraPanRequested.Direction.Down, keyEvent.Action == KeyCaptured.ActionType.Pressed ? CameraPanRequested.RequestType.Start : CameraPanRequested.RequestType.Stop, FastModifier));
                        break;
                    default:
                        Debug.LogWarning("Key not mapped: " + keyEvent.Key.ToString());
                        break;
                }
            }
        }

        protected void MouseInput(MouseCaptured mouseEvent)
        {
            foreach (InputMap input in mappings.MouseActions.FindAll(input => input.Key == mouseEvent.Key))
            {
                switch (input.Action)
                {
                    case Action.Select:
                        if (mouseEvent.Action == KeyCaptured.ActionType.Pressed)
                        {
                            GameManager.EventSystem.Publish(new InputAction(mouseEvent.Position, MultiModifier ? InputAction.ActionType.MultiSelectAtPosition : InputAction.ActionType.SelectAtPosition));
                        }
                        break;
                    case Action.MoveTo:
                        if (mouseEvent.Action == KeyCaptured.ActionType.Pressed)
                        {
                            GameManager.EventSystem.Publish(new InputAction(mouseEvent.Position, MultiModifier ? InputAction.ActionType.EnqueueMoveToPosition : InputAction.ActionType.MoveToPosition));
                        }
                        break;
                    default:
                        Debug.LogWarning("Mouse action not mapped: " + mouseEvent.Key.ToString());
                        break;
                }
            }
        }
    }
}
