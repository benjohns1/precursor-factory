using GameEvents;
using GameEvents.UnitTask;
using Manufacture.Process;
using System.Collections.Generic;
using System.Linq;
using UnitTask;
using UnitTask.Manufacture;
using UnityEngine;
using static Manufacture.CargoProcess;

namespace Manufacture
{
    public class ManufactureSystem
    {
        internal class Manufacturer
        {
            public CargoProcess CargoProcess;
            public ITask InitialTask;

            public Manufacturer(CargoProcess cargoProcess, ITask initialTask = null)
            {
                CargoProcess = cargoProcess;
                InitialTask = initialTask;
            }
        }

        internal Dictionary<ManufactureComponent, Manufacturer> manufacturers = new Dictionary<ManufactureComponent, Manufacturer>();

        public ManufactureSystem()
        {
            GameManager.EventSystem.Subscribe(typeof(UnitTaskEvent), HandleUnitTaskEvent);
        }

        private void HandleUnitTaskEvent(IEvent @event)
        {
            UnitTaskEvent taskEvent = @event as UnitTaskEvent;
            if (taskEvent.Task is Drill)
            {
                Drill drillTask = taskEvent.Task as Drill;
                ManufactureComponent manufactureComponent = GetManufactureComponent(drillTask);
                if (manufactureComponent == null)
                {
                    return;
                }

                if (@event is UnitTaskStarted)
                {
                    StartDrilling(manufactureComponent, drillTask);
                }
                else if (@event is UnitTaskCancelled)
                {
                    StopAll(manufactureComponent, drillTask);
                }
            }
        }

        private void StartDrilling(ManufactureComponent manufactureComponent, Drill drillTask)
        {
            AsteroidComponent asteroid = Drill.GetDrillableAsteroid(manufactureComponent.transform.position);
            if (asteroid == default(AsteroidComponent))
            {
                StopAll(manufactureComponent, drillTask);
                return;
            }
            manufacturers[manufactureComponent] = new Manufacturer(new DrillOre(asteroid, manufactureComponent, Time.time), drillTask);
        }

        private void StopAll(ManufactureComponent manufactureComponent, Drill drillTask)
        {
            manufacturers[manufactureComponent] = new Manufacturer(new Stopped(), drillTask);
        }

        private ManufactureComponent GetManufactureComponent(Drill drillTask)
        {
            ManufactureComponent manufactureComponent = drillTask.TaskQueue.TaskQueueComponent.GetComponent<ManufactureComponent>();
            if (!manufacturers.ContainsKey(manufactureComponent))
            {
                Debug.LogWarning(typeof(ManufactureComponent).ToString() + " " + manufactureComponent.name + " is not registered with " + this.GetType().ToString());
                return null;
            }
            return manufactureComponent;
        }

        public void Register(ManufactureComponent manufacturer)
        {
            if (!manufacturers.ContainsKey(manufacturer))
            {
                manufacturers.Add(manufacturer, null);
            }
        }

        public void Unregister(ManufactureComponent manufacturer)
        {
            if (manufacturers.ContainsKey(manufacturer))
            {
                manufacturers.Remove(manufacturer);
            }
        }

        public void Update()
        {
            List<KeyValuePair<ManufactureComponent, Manufacturer>> complete = new List<KeyValuePair<ManufactureComponent, Manufacturer>>();
            foreach (KeyValuePair<ManufactureComponent, Manufacturer> manufacturer in manufacturers)
            {
                if (manufacturer.Value == null)
                {
                    continue;
                }

                if (!UpdateManufacturer(manufacturer.Key, manufacturer.Value))
                {
                    complete.Add(manufacturer);
                }
            }

            foreach (KeyValuePair<ManufactureComponent, Manufacturer> manufacturer in complete)
            {
                GameManager.EventSystem.Publish(new UnitTaskCompleted(manufacturer.Value.InitialTask));
                manufacturers[manufacturer.Key] = null;
            }
        }

        private bool UpdateManufacturer(ManufactureComponent manufactureComponent, Manufacturer manufacturer)
        {
            if (!manufacturer.CargoProcess.Running)
            {
                return true;
            }

            CargoRetrieval cargo = manufacturer.CargoProcess.GetCargo(Time.time);
            if (cargo.CargoAmount.Amount > 0)
            {
                Cargo.Inventory inventory = manufactureComponent.GetComponent<Cargo.CargoComponent>().Inventory;
                if (!inventory.Add(cargo.CargoAmount))
                {
                    return false;
                }
                if (inventory.AvailableVolume <= 0)
                {
                    return false;
                }
            }

            return !cargo.Complete;
        }
    }
}
