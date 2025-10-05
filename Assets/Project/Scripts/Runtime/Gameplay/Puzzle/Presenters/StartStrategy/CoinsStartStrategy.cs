using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using OMG.Configs;
using OMG.Models;
using OMG.Services;
using OMG.Views;

namespace OMG.Presenters
{
    public sealed class CoinsStartStrategy : IPuzzleStartStrategy
    {
        private readonly ILoadingService _loadingService;
        private readonly IWallet _wallet;
        private readonly IPuzzleDialogView _dialogView;
        private readonly IPuzzleStartCostPolicy _costPolicy;

        public CoinsStartStrategy(ILoadingService loadingService, IWallet wallet, IPuzzleDialogView dialogView, IPuzzleStartCostPolicy costPolicy)
        {
            _loadingService = loadingService ?? throw new ArgumentNullException(nameof(loadingService));
            _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            _dialogView = dialogView ?? throw new ArgumentNullException(nameof(dialogView));
            _costPolicy = costPolicy ?? throw new ArgumentNullException(nameof(costPolicy));
        }

        public PuzzleStartMode Mode => PuzzleStartMode.Coins;

        public async UniTask<bool> TryStartAsync(PuzzleStartContext context, CancellationToken cancellationToken)
        {
            int cost = _costPolicy.GetPuzzleCost();
            
            if (!_wallet.CanSpend(cost))
            {
                _dialogView.SetStatus("Not enough coins");
                return false;
            }

            _wallet.Spend(cost);
            await _loadingService.LoadAsync(new Progress<float>(_dialogView.UpdateProgress), cancellationToken);
            return true;
        }
    }
}