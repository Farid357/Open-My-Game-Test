using System;
using UniRx;

namespace OMG.Models
{
    public sealed class Wallet : IWallet
    {
        private readonly ReactiveProperty<int> _money = new(100);

        public IReactiveProperty<int> Money => _money;

        public bool CanSpend(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
          
            return _money.Value - amount >= 0;
        }

        public void Spend(int amount)
        {
            if (CanSpend(amount) == false)
                throw new ArgumentOutOfRangeException("Insufficient funds.");
            
            _money.Value -= amount;
        }

        public void Put(int amount)
        {
            if (amount < 0) 
                throw new ArgumentOutOfRangeException(nameof(amount));

            _money.Value += amount;
        }
    }
}