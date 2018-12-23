using System.Collections.Generic;
using System.Linq;

namespace Cargo
{
    [System.Serializable]
    public class OreAmount : CargoAmount
    {
        public Matter[] MatterList = new Matter[0];
        public int[] MatterRatioList = new int[0];

        public Dictionary<Matter, int> MatterRatio
        {
            get
            {
                Dictionary<Matter, int> matterRatio = new Dictionary<Matter, int>();
                for (int i = 0; i < MatterList.Length; i++)
                {
                    if (MatterRatioList.Length <= i)
                    {
                        break;
                    }
                    matterRatio.Add(MatterList[i], MatterRatioList[i]);
                }
                return matterRatio;
            }
        }

        public OreAmount()
        {
            ID = CargoID.OreChunk;
        }

        public OreAmount(int amount, Dictionary<Matter, int> matterRatio) : this()
        {
            Amount = amount;
            MatterList = matterRatio.Keys.ToArray();
            MatterRatioList = matterRatio.Values.ToArray();
        }

        public override bool Combinable(CargoAmount cargoAmount)
        {
            if (!(cargoAmount is OreAmount))
            {
                return false;
            }
            if (!base.Combinable(cargoAmount))
            {
                return false;
            }
            OreAmount oreAmount = cargoAmount as OreAmount;
            if (oreAmount.MatterRatio.Count != MatterRatio.Count)
            {
                return false;
            }
            foreach (KeyValuePair<Matter, int> ratio in MatterRatio)
            {
                if (!oreAmount.MatterRatio.ContainsKey(ratio.Key)
                    || !oreAmount.MatterRatio[ratio.Key].Equals(ratio.Value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
