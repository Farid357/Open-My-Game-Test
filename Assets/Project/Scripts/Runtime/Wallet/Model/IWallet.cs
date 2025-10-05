namespace OMG.Models
{
    public interface IWallet : IReadOnlyWallet
    {
        void Spend(int amount);
       
        void Put(int amount);
    }
}