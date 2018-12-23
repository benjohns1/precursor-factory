using GameEvents;
using GameEvents.Actions;
using GameEvents.UI;
using System.Collections.Generic;
using System.Linq;
using UnitTask;
using UnityEngine;
using UI.UnitTask;

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

            ContextMenu = Object.Instantiate(Settings.ContextMenuPrefab, Settings.Canvas).GetComponent<ContextMenuComponent>();
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

        private class TaskData
        {
            public int Count = 1;
            public TaskType Type = null;
        }

        private class BuildData
        {
            public int Count = 1;
            public BuildingType Type = null;
        }

        private class ContextData
        {
            public string Display = null;
            public int Count = 0;
            public List<TaskData> Tasks = new List<TaskData>();
            public List<BuildData> Buildings = new List<BuildData>();

            public void AddTask(TaskType type)
            {
                if (Tasks.Any(c => c.Type.Action == type.Action && c.Type.DisplayName == type.DisplayName))
                {
                    Tasks.First(c => c.Type.Action == type.Action && c.Type.DisplayName == type.DisplayName).Count++;
                }
                else
                {
                    Tasks.Add(new TaskData
                    {
                        Type = type
                    });
                }
            }

            public void AddBuilding(BuildingType type)
            {
                if (Buildings.Any(c => c.Type.Building == type.Building && c.Type.DisplayName == type.DisplayName))
                {
                    Buildings.First(c => c.Type.Building == type.Building && c.Type.DisplayName == type.DisplayName).Count++;
                }
                else
                {
                    Buildings.Add(new BuildData
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
            ClearActionList();

            foreach (TaskData task in context.Tasks)
            {
                ContextMenuItemComponent menuItem = Object.Instantiate(Settings.ContextMenuItemPrefab, ContextMenu.Actions.transform).GetComponent<ContextMenuItemComponent>();
                menuItem.Text.text = task.Type.DisplayName + (task.Count != context.Count ? " (" + task.Count + ")" : "");
                menuItem.Button.onClick.AddListener(() =>
                {
                    GameManager.EventSystem.Publish(new InputActionRequested(position, task.Type.Action, multi));
                    HideContextMenu();
                });
            }
            foreach (BuildData building in context.Buildings)
            {
                ContextMenuItemComponent menuItem = Object.Instantiate(Settings.ContextMenuItemPrefab, ContextMenu.Actions.transform).GetComponent<ContextMenuItemComponent>();
                menuItem.Text.text = "Build " + building.Type.DisplayName + (building.Count != context.Count ? " (" + building.Count + ")" : "");
                menuItem.Button.onClick.AddListener(() =>
                {
                    GameManager.EventSystem.Publish(new BuildActionRequested(position, building.Type.Building, multi));
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
                if (selection == null)
                {
                    continue;
                }
                context.Count++;

                if (name)
                {
                    if (context.Display == null)
                    {
                        context.Display = selection.DisplayName;
                        displayType = selection.DisplayType;
                    }
                    else if (context.Display != selection.DisplayName)
                    {
                        name = false;
                    }
                }

                if (!name && type)
                {
                    if (displayType == selection.DisplayType)
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

                AvailableTasksComponent availableTasks = selection.GetComponent<AvailableTasksComponent>();
                foreach (TaskType task in availableTasks.GetTasks(mousePosition))
                {
                    context.AddTask(task);
                }

                AvailableBuildingsComponent availableBuildings = selection.GetComponent<AvailableBuildingsComponent>();
                foreach (BuildingType building in availableBuildings.GetBuildings(mousePosition))
                {
                    context.AddBuilding(building);
                }
            }

            return context;
        }

        private void HideContextMenu()
        {
            ContextMenu.gameObject.SetActive(false);
            ClearActionList();
            GameManager.EventSystem.Publish(new MouseReleased());
        }

        private void ClearActionList()
        {
            foreach (Transform child in ContextMenu.Actions.transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}
