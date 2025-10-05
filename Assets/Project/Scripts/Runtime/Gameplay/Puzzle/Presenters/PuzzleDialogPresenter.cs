using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using OMG.Models;
using OMG.Services;
using OMG.Views;
using OMG.Configs;

namespace OMG.Presenters
{
    public sealed class PuzzleDialogPresenter : IDisposable
    {
        private readonly IPuzzleDialogView _view;
        private readonly IPuzzleStartCostPolicy _policy;
        private readonly IPuzzleStartStrategy[] _strategies;
        private readonly PuzzleMeta _meta;

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private int _selectedPieces = 9;
        private PuzzleStartMode _currentMode = PuzzleStartMode.Free;

        public PuzzleDialogPresenter(IPuzzleDialogView view, IPuzzleStartCostPolicy policy, IPuzzleStartStrategy[] strategies, PuzzleMeta meta)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
            _meta = meta ?? throw new ArgumentNullException(nameof(meta));

            _view.PiecesSelected.Subscribe(OnPiecesSelected).AddTo(_compositeDisposable);
            _view.StartClicked.Subscribe(async _ => await HandleStartAsync()).AddTo(_compositeDisposable);
            _view.CloseClicked.Subscribe(_ => Close()).AddTo(_compositeDisposable);

            OnPiecesSelected(_selectedPieces);
            _view.UpdateStartMode(_policy.GetStartModeForPieces(_selectedPieces), _policy.GetPuzzleCost());
        }

        private void OnPiecesSelected(int pieces)
        {
            _selectedPieces = pieces;
            _currentMode = _policy.GetStartModeForPieces(pieces);
            _view.UpdateStartMode(_currentMode, _policy.GetPuzzleCost());
        }

        private async UniTask HandleStartAsync()
        {
            _view.SetInteractable(false);
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var strategy = _strategies.FirstOrDefault(s => s.Mode == _currentMode);
                if (strategy == null)
                {
                    Debug.LogError($"No strategy for mode {_currentMode}");
                    _view.SetStatus("Internal error");
                    return;
                }

                var context = new PuzzleStartContext(_meta, _selectedPieces);
                bool success = await strategy.TryStartAsync(context, _cancellationTokenSource.Token);

                if (success)
                {
                    _view.SetStatus("Started");
                    _view.Close();
                    Dispose();
                }
                
                OnPiecesSelected(9);
                _view.ResetSelectedPieces();
            }
            catch (OperationCanceledException)
            {
                _view.SetStatus("Canceled");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[PuzzleDialogPresenter] HandleStartAsync error: {ex}");
                _view.SetStatus("Error");
            }
            finally
            {
                _view.SetInteractable(true);
            }
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

        public class Factory : PlaceholderFactory<PuzzleMeta, PuzzleDialogPresenter> { }

        public class FactoryImpl : IFactory<PuzzleMeta, PuzzleDialogPresenter>
        {
            private readonly DiContainer _container;

            public FactoryImpl(DiContainer container)
            {
                _container = container ?? throw new ArgumentNullException(nameof(container));
            }

            public PuzzleDialogPresenter Create(PuzzleMeta meta)
            {
                IPuzzleDialogView view = _container.Resolve<IPuzzleDialogView>();
                IWallet wallet = _container.Resolve<IWallet>();
                IAdsService ads = _container.Resolve<IAdsService>();
                ILoadingService loader = _container.Resolve<ILoadingService>();
                IPuzzleStartCostPolicy policy = _container.Resolve<IPuzzleStartCostPolicy>();

                IPuzzleStartStrategy[] strategies = _container.ResolveAll<IPuzzleStartStrategy>().ToArray();

                return new PuzzleDialogPresenter(view, policy, strategies, meta);
            }
        }
    }
}
