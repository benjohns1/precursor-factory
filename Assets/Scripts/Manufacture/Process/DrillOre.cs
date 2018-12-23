using Movement.Trajectory;
using UnityEngine;

namespace Manufacture.Process
{
    class DrillOre : CargoProcess
    {
        public override bool Running => running;

        private readonly AsteroidComponent Asteroid;
        private readonly ManufactureComponent Drill;
        private readonly float InitialTime;

        private bool running;
        private float PreviousRetrievalTime;
        private int PreviousRetrievedAmount;

        public DrillOre(AsteroidComponent asteroid, ManufactureComponent drill, float initialTime)
        {
            Asteroid = asteroid;
            Drill = drill;
            InitialTime = initialTime;
            PreviousRetrievalTime = initialTime;
            PreviousRetrievedAmount = 0;
            running = true;
        }

        public override CargoRetrieval GetCargo(float time)
        {
            // @TODO: check movement trajectories to determine if/when asteroid/drill moved out of range
            // @TODO: listen for event when drill's OrePerHour changes
            // @TODO: remove from asteroid's available ore

            int total = CalculateTotalDrilledAmount(InitialTime, time, Drill.DrillOrePerHour);
            if (total <= 0)
            {
                return None();
            }

            int max = total - PreviousRetrievedAmount;
            if (max <= 0)
            {
                return None();
            }

            return FromOreAmount(time, max, Asteroid.OreAmount);
        }

        private static int CalculateTotalDrilledAmount(float initialTime, float now, int orePerHour)
        {
            float t = now - initialTime;
            return Mathf.FloorToInt(t * orePerHour / 3600);
        }

        private CargoRetrieval None()
        {
            return new CargoRetrieval(PreviousRetrievalTime, new Cargo.OreAmount(), false);
        }

        private CargoRetrieval FromOreAmount(float time, int max, Cargo.OreAmount oreAmount)
        {
            bool complete = oreAmount.Amount <= max;
            int amount = complete ? oreAmount.Amount : max;
            PreviousRetrievalTime = time;
            PreviousRetrievedAmount += amount;
            running = !complete;
            return new CargoRetrieval(PreviousRetrievalTime, new Cargo.OreAmount(amount, oreAmount.MatterRatio), complete);
        }
    }
}
