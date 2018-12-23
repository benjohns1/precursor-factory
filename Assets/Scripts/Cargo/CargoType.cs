using System.Collections.Generic;
using UnityEngine;

namespace Cargo
{
    public struct CargoType
    {
        public readonly CargoID ID;
        public readonly string Name;
        public readonly Color Color;
        public readonly Sprite Sprite;
        public readonly int VolumePerUnit;

        public CargoType(CargoID ID, string name, Color color, Sprite sprite, int volumePerUnit)
        {
            this.ID = ID;
            Name = name;
            Color = color;
            Sprite = sprite;
            VolumePerUnit = volumePerUnit;

            if (!Repo.ContainsKey(ID))
            {
                Repo.Add(ID, this);
            }
        }

        public readonly static Dictionary<CargoID, CargoType> Repo = new Dictionary<CargoID, CargoType>();
    }
}
