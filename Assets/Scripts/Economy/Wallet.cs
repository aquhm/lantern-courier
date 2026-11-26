using System;
using R3;

namespace LanternCourier.Economy
{
    public sealed class Wallet : IDisposable
    {
        public ReactiveProperty<int> Coins { get; } = new(0);

        public void Add(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Coins.Value += amount;
        }

        public bool TrySpend(int amount)
        {
            if (amount < 0 || Coins.Value < amount)
            {
                return false;
            }

            Coins.Value -= amount;
            return true;
        }

        public void Dispose()
        {
            Coins.Dispose();
        }
    }
}
