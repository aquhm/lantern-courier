using System;

namespace LanternCourier.Save
{
    [Serializable]
    public sealed class SaveData
    {
        public int coins;
        public int deliveriesCompleted;
        public int tankTier = 1;
    }
}
