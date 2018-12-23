using Cargo;

namespace Manufacture
{
    public abstract class CargoProcess
    {
        public struct CargoRetrieval
        {
            public float PreviousRetrievalTime;
            public CargoAmount CargoAmount;
            public bool Complete;

            public CargoRetrieval(float previousRetrievalTime, CargoAmount cargoAmount, bool complete)
            {
                PreviousRetrievalTime = previousRetrievalTime;
                CargoAmount = cargoAmount;
                Complete = complete;
            }
        }

        public abstract CargoRetrieval GetCargo(float time);
        public abstract bool Running { get; }
        public virtual string Info => ToString();
    }
}
