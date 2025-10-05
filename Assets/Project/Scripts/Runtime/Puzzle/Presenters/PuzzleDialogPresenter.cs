using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using OMG.Configs;
using OMG.Models;
using OMG.Services;
using OMG.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace OMG.Presenters
{
    public sealed class PuzzleDialogPresenter : IDisposable
    {
        private readonly IPuzzleDialogView _view;
        private readonly ILoadingService _loadingService;
        private readonly IAdsService _adsService;
        private readonly IWallet _wallet;
        private readonly IStartCostPolicy _startPolicy;
        private readonly PuzzleMeta _meta;
        private readonly CompositeDisposable _compositeDisposable = new();
     
        private CancellationTokenSource _cancellationTokenSource = new();
        private StartMode _currentMode = StartMode.Free;
        private int _selectedPieces = 9;
        
        public PuzzleDialogPresenter(IPuzzleDialogView view, ILoadingService loadingService, IAdsService adsService, IWallet wallet, IStartCostPolicy startPolicy, PuzzleMeta meta)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _loadingService = loadingService ?? throw new ArgumentNullException(nameof(loadingService));
            _adsService = adsService ?? throw new ArgumentNullException(nameof(adsService));
            _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            _startPolicy = startPolicy ?? throw new ArgumentNullException(nameof(startPolicy));
            _meta = meta ?? throw new ArgumentNullException(nameof(meta));

            _view.SetCoins(_wallet.Money.Value);
            _wallet.Money.Subscribe(v => _view.SetCoins(v)).AddTo(_compositeDisposable);

            _view.PiecesSelected.Subscribe(OnPiecesSelected).AddTo(_compositeDisposable);
            _view.StartClicked.Subscribe(async _ => await SafeStart()).AddTo(_compositeDisposable);
            _view.CloseClicked.Subscribe(_ => Close()).AddTo(_compositeDisposable);

            OnPiecesSelected(9);
            _view.UpdateStartMode(_startPolicy.GetStartModeForPieces(9), _startPolicy.GetPuzzleCost());
        }

        private void OnPiecesSelected(int pieces)
        {
            _selectedPieces = pieces;
            _currentMode = _startPolicy.GetStartModeForPieces(pieces);
            _view.UpdateStartMode(_currentMode, _startPolicy.GetPuzzleCost());
        }

        private async UniTask SafeStart()
        {
            _view.SetInteractable(false);
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                switch (_currentMode)
                {
                    case StartMode.Free:
                        await DoLoad(_cancellationTokenSource.Token);
                        break;
                    case StartMode.Coins:
                        int cost = _startPolicy.GetPuzzleCost();
                        if (!_wallet.CanSpend(cost))
                        {
                            _view.SetStatus("Not enough coins");
                            break;
                        }
                        _wallet.Spend(cost);
                        _view.SetCoins(_wallet.Money.Value);
                        await DoLoad(_cancellationTokenSource.Token);
                        break;
                    case StartMode.Ads:
                        bool rewarded = await _adsService.ShowRewardedAdAsync(_cancellationTokenSource.Token);
                        if (!rewarded)
                        {
                            _view.SetStatus("Ad not completed");
                            break;
                        }
                        await DoLoad(_cancellationTokenSource.Token);
                        break;
                    default:
                        _view.SetStatus("Unknown start mode");
                        break;
                }
            }
            catch (OperationCanceledException)
            {
                _view.SetStatus("Canceled");
            }
            catch (Exception exception)
            {
                Debug.LogError($"[PuzzleDialogPresenter] SafeStart error: {exception}");
                _view.SetStatus("Error");
            }
            finally
            {
                _view.SetInteractable(true);
            }
        }

        private async UniTask DoLoad(CancellationToken cancellationToken)
        {
            _view.SetStatus("Loading...");
            var progress = 0f;
            var progressReporter = new Progress<float>(p =>
            {
                progress = p;
                _view.UpdateProgress(progress);
            });
            await _loadingService.LoadAsync(progressReporter, cancellationToken);
            _view.SetStatus("Loaded. Starting...");
            await UniTask.Delay(250, cancellationToken: cancellationToken);
            Debug.Log($"[PuzzleDialogPresenter] Start puzzle {_meta.PuzzleId} pieces={_selectedPieces}");
            _view.SetStatus("Started");
            _view.Close();
            Dispose();
        }

        private void Close()
        {
            _cancellationTokenSource.Cancel();
            _view.Close();
            Dispose();
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        // Zenject factory
        public class Factory : PlaceholderFactory<PuzzleMeta, PuzzleDialogPresenter> { }

        public class FactoryImpl : IFactory<PuzzleMeta, PuzzleDialogPresenter>
        {
            private readonly DiContainer _container;

            public FactoryImpl(DiContainer container) { _container = container; }

            public PuzzleDialogPresenter Create(PuzzleMeta meta)
            {
                var view = _container.Resolve<IPuzzleDialogView>();
                var loadingService = _container.Resolve<ILoadingService>();
                var ads = _container.Resolve<IAdsService>();
                var wallet = _container.Resolve<IWallet>();
                var policy = _container.Resolve<IStartCostPolicy>();
                return new PuzzleDialogPresenter(view, loadingService, ads, wallet, policy, meta);
            }
        }
    }
}