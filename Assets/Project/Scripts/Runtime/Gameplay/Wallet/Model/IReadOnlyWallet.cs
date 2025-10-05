using UniRx;

namespace OMG.Models
{
    public interface IReadOnlyWallet
    {
        IReactiveProperty<int> Money { get; }
        
        bool CanSpend(int amount);
    }
}