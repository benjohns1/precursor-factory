using Movement.Trajectory;
using UnityEngine;

namespace Manufacture.Process
{
    class Stopped : CargoProcess
    {
        public override bool Running => false;

        public override CargoRetrieval GetCargo(float time)
        {
            return new CargoRetrieval(0, new Cargo.CargoAmount(), true);
        }
    }
}
