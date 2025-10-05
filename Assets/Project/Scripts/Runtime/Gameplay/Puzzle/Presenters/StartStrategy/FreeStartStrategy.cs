using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using OMG.Configs;
using OMG.Services;
using OMG.Views;

namespace OMG.Presenters
{
    public sealed class FreeStartStrategy : IPuzzleStartStrategy
    {
        private readonly ILoadingService _loadingService;
        private readonly IPuzzleDialogView _dialogView;

        public FreeStartStrategy(ILoadingService loadingService, IPuzzleDialogView dialogView)
        {
            _loadingService = loadingService ?? throw new ArgumentNullException(nameof(loadingService));
            _dialogView = dialogView ?? throw new ArgumentNullException(nameof(dialogView));
        }
        
        public PuzzleStartMode Mode => PuzzleStartMode.Free;

        public async UniTask<bool> TryStartAsync(PuzzleStartContext context, CancellationToken cancellationToken)
        {
            await _loadingService.LoadAsync(new Progress<float>(_dialogView.UpdateProgress), cancellationToken);

            return true;
        }
    }
}