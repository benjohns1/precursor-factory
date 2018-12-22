using Behavior;
using GameEvents;
using GameEvents.Actions;
using GameEvents.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UISystem
    {
        [System.Serializable]
        public class UISettings
        {
            public Transform Canvas;
            public GameObject ContextMenuPrefab;
            public GameObject ContextMenuItemPrefab;
        }

        private readonly UISettings Settings;
        private readonly ContextMenuComponent ContextMenu;

        public UISystem(UISettings settings)
        {
            Settings = settings;

            ContextMenu = UnityEngine.Object.Instantiate(Settings.ContextMenuPrefab, Settings.Canvas).GetComponent<ContextMenuComponent>();
            ContextMenu.gameObject.SetActive(false);

            GameManager.EventSystem.Subscribe(typeof(InputActionRequested), HandleInputEvent);
            GameManager.EventSystem.Subscribe(typeof(MouseEventCaptured), HandleUIMouseEvent);
        }

        private void HandleUIMouseEvent(IEvent @event)
        {
            MouseEventCaptured mouseEvent = @event as MouseEventCaptured;
            CheckHideContextMenu(mouseEvent.Position);
        }

        private void HandleInputEvent(IEvent @event)
        {
            InputActionRequested inputEvent = @event as InputActionRequested;
            if (inputEvent.Action == InputActionRequested.ActionType.ChooseAction)
            {
                List<SelectableComponent> selections = GameManager.SelectionSystem.GetSelections();
                if (selections.Count > 0)
                {
                    ShowContextMenu(selections, inputEvent.Position, inputEvent.MultiModifier);
                }
                else
                {
                    CheckHideContextMenu(inputEvent.Position);
                }
            }
            else
            {
                CheckHideContextMenu(inputEvent.Position);
            }
        }

        private void CheckHideContextMenu(Vector2 mousePosition)
        {
            if (!ContextMenu.gameObject.activeSelf)
            {
                return;
            }

            if (!RectTransformUtility.RectangleContainsScreenPoint(ContextMenu.GetComponent<RectTransform>(), mousePosition))
            {
                HideContextMenu();
            }
        }

        private class CommandData
        {
            public int Count = 1;
            public CommandType Type = null;
        }

        private class ContextData
        {
            public string Display = null;
            public int Count = 0;
            public List<CommandData> Commands = new List<CommandData>();

            public void AddCommand(CommandType type)
            {
                if (Commands.Any(c => c.Type.Action == type.Action && c.Type.DisplayName == type.DisplayName))
                {
                    Commands.First(c => c.Type.Action == type.Action && c.Type.DisplayName == type.DisplayName).Count++;
                }
                else
                {
                    Commands.Add(new CommandData
                    {
                        Type = type
                    });
                }
            }
        }

        private void ShowContextMenu(List<SelectableComponent> selections, Vector2 position, bool multi)
        {
            GameManager.EventSystem.Publish(new MouseCaptured());
            ContextMenu.transform.position = position;

            ContextData context = GetContextData(selections, position);
            ContextMenu.Title.text = context.Display + (context.Count > 1 ? " (" + context.Count + ")" : "");
            ClearCommandList();
            foreach (CommandData command in context.Commands)
            {
                ContextMenuItemComponent menuItem = UnityEngine.Object.Instantiate(Settings.ContextMenuItemPrefab, ContextMenu.Commands.transform).GetComponent<ContextMenuItemComponent>();
                menuItem.Text.text = command.Type.DisplayName + (command.Count != context.Count ? " (" + command.Count + ")" : "");
                menuItem.Button.onClick.AddListener(() =>
                {
                    GameManager.EventSystem.Publish(new InputActionRequested(position, command.Type.Action, multi));
                    HideContextMenu();
                });
            }

            ContextMenu.gameObject.SetActive(true);
        }

        private ContextData GetContextData(List<SelectableComponent> selections, Vector2 mousePosition)
        {
            ContextData context = new ContextData();
            bool name = true;
            bool type = true;
            string displayType = null;
            foreach (SelectableComponent selection in selections)
            {
                BehaviorComponent behavior = selection.GetComponent<BehaviorComponent>();
                if (behavior == null)
                {
                    continue;
                }
                context.Count++;

                if (name)
                {
                    if (context.Display == null)
                    {
                        context.Display = behavior.DisplayName;
                        displayType = behavior.DisplayType;
                    }
                    else if (context.Display != behavior.DisplayName)
                    {
                        name = false;
                    }
                }

                if (!name && type)
                {
                    if (displayType == behavior.DisplayType)
                    {
                        context.Display = displayType;
                    }
                    else
                    {
                        type = false;
                    }
                }

                if (!name && !type)
                {
                    context.Display = null;
                }

                foreach (CommandType command in behavior.GetCommands(mousePosition))
                {
                    context.AddCommand(command);
                }
            }

            return context;
        }

        private void HideContextMenu()
        {
            ContextMenu.gameObject.SetActive(false);
            ClearCommandList();
            GameManager.EventSystem.Publish(new MouseReleased());
        }

        private void ClearCommandList()
        {
            foreach (Transform child in ContextMenu.Commands.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }
    }
}
