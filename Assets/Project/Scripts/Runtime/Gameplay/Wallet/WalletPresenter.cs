using System;
using System.Collections.Generic;
using OMG.Models;
using OMG.Views;
using UniRx;

namespace OMG.Presenters
{
    public class WalletPresenter : IDisposable
    {
        private readonly IWallet _wallet;
        private readonly WalletView _view;
        private readonly CompositeDisposable _compositeDisposable = new();

        public WalletPresenter(IWallet wallet, WalletView view)
        {
            _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _view.Show($"Money: {_wallet.Money.Value}");
            _wallet.Money.Subscribe(money => _view.Show($"Money: {money}")).AddTo(_compositeDisposable);
        }
        
        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}