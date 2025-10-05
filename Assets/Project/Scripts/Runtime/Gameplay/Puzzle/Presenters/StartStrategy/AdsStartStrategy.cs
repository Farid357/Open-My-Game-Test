using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using OMG.Configs;
using OMG.Services;
using OMG.Views;

namespace OMG.Presenters
{
    public sealed class AdsStartStrategy : IPuzzleStartStrategy
    {
        private readonly IAdsService _adsService;
        private readonly ILoadingService _loadingService;
        private readonly IPuzzleDialogView _dialogView;

        public AdsStartStrategy(IAdsService adsService, ILoadingService loadingService, IPuzzleDialogView dialogView)
        {
            _adsService = adsService ?? throw new ArgumentNullException(nameof(adsService));
            _loadingService = loadingService ?? throw new ArgumentNullException(nameof(loadingService));
            _dialogView = dialogView ?? throw new ArgumentNullException(nameof(dialogView));
        }
        
        public PuzzleStartMode Mode => PuzzleStartMode.Ads;

        public async UniTask<bool> TryStartAsync(PuzzleStartContext context, CancellationToken cancellationToken)
        {
            _dialogView.SetStatus("Loading Reward Ad...");

            bool rewarded = await _adsService.ShowRewardedAdAsync(cancellationToken);
            if (!rewarded)
            {
                _dialogView.SetStatus("Ad not completed");
                return false;
            }

            await _loadingService.LoadAsync(new Progress<float>(_dialogView.UpdateProgress), cancellationToken);
            return true;
        }
    }
}