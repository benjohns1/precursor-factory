using System.Collections.Generic;
using UnityEngine;

namespace Cargo
{
    public class CargoRepo
    {
        public CargoRepo()
        {
        }

        public CargoType Get(CargoID ID)
        {
            return Types[ID];
        }

        private readonly Dictionary<CargoID, CargoType> Types = new Dictionary<CargoID, CargoType>()
        {
            { CargoID.OreChunk, new CargoType(CargoID.OreChunk, "Ore Chunk", Color.grey, null, 10) },
            { CargoID.Iron, new CargoType(CargoID.Iron, "Iron", Color.red, null, 10) },
            { CargoID.Silicon, new CargoType(CargoID.Silicon, "Silicon", Color.blue, null, 10) },
        };
    }
}
