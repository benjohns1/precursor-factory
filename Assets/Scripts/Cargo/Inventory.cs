using UnityEngine;

namespace Cargo
{
    [System.Serializable]
    public class Inventory
    {
        [SerializeField]
        private int MaxCargoVolume = 100;

        [SerializeField]
        private CargoAmount[] Cargo = new CargoAmount[0];

        private int availableVolume;
        public int AvailableVolume => availableVolume;

        public Inventory()
        {
            availableVolume = MaxCargoVolume;
        }

        public bool Add(CargoAmount cargoAmount)
        {
            CargoType cargoType = GameManager.CargoRepo.Get(cargoAmount.ID);
            int volume = cargoType.VolumePerUnit * cargoAmount.Amount;
            if (availableVolume < volume)
            {
                return false;
            }

            bool added = false;
            CargoAmount[] newCargo = new CargoAmount[Cargo.Length + 1];
            for (int i = 0; i < Cargo.Length; i++)
            {
                if (!added)
                {
                    added = Cargo[i].TryCombine(cargoAmount);
                }
                newCargo[i] = Cargo[i];
            }
            if (!added)
            {
                newCargo[Cargo.Length] = cargoAmount;
                Cargo = newCargo;
            }
            availableVolume -= volume;
            return true;
        }
    }
}
