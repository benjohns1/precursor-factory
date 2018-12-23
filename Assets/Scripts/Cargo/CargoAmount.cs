namespace Cargo
{
    [System.Serializable]
    public class CargoAmount
    {
        public CargoID ID = CargoID.None;
        public int Amount = 0;

        public virtual bool Combinable(CargoAmount cargoAmount)
        {
            return (cargoAmount.ID == ID);
        }

        public virtual bool TryCombine(CargoAmount cargoAmount)
        {
            if (!Combinable(cargoAmount))
            {
                return false;
            }

            Amount += cargoAmount.Amount;
            return true;
        }
    }
}
