using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using OMG.Models;
using OMG.Services;
using OMG.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace OMG.Presenters
{
    public sealed class GalleryPresenter : IDisposable
    {
        private readonly IPreviewService _previewService;
        private readonly IPuzzleGalleryView _view;
        private readonly DiContainer _container;
        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly CancellationTokenSource _cancellationToken = new();

        public GalleryPresenter(IPreviewService previewService, IPuzzleGalleryView view, DiContainer container)
        {
            _previewService = previewService ?? throw new ArgumentNullException(nameof(previewService));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _container = container ?? throw new ArgumentNullException(nameof(container));

            InitializeAsync().Forget();
            _view.OnPreviewClicked.Subscribe(OnPreviewClicked).AddTo(_compositeDisposable);
        }

        private async UniTask InitializeAsync()
        {
            _view.ShowLoading(true);
            try
            {
                var metas = await _previewService.GetAvailablePreviewsAsync(_cancellationToken.Token);
                var items = new List<(PuzzleMeta meta, Sprite sprite)>(metas.Count);
                foreach (PuzzleMeta meta in metas)
                {
                    var texture = await _previewService.LoadPreviewTextureAsync(meta.PreviewUrl, _cancellationToken.Token);
                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    items.Add((meta, sprite));
                }
                _view.ShowItems(items);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[GalleryPresenter] Initialize canceled");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GalleryPresenter] Initialize error: {ex}");
            }
            finally
            {
                _view.ShowLoading(false);
            }
        }

        private void OnPreviewClicked(PuzzleMeta meta)
        {
            try
            {
                var dialogView = _container.Resolve<IPuzzleDialogView>();
                if (dialogView == null)
                {
                    Debug.LogWarning("DialogView not bound.");
                    return;
                }
                
                ShowDialogForMetaAsync(meta, dialogView).Forget();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GalleryPresenter] OnPreviewClicked error: {ex}");
            }
        }

        private async UniTaskVoid ShowDialogForMetaAsync(PuzzleMeta meta, IPuzzleDialogView dialog)
        {
            Texture2D texture = null;
            try
            {
                texture = await _previewService.LoadPreviewTextureAsync(meta.PreviewUrl, _cancellationToken.Token);
            }
            catch { /* ignore */ }

            var smallSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            dialog.Show(meta, smallSprite);

            // Create presenter for dialog via DI factory or directly
            var factory = _container.Resolve<PuzzleDialogPresenter.Factory>();
            PuzzleDialogPresenter presenter = factory.Create(meta);
            // Presenter created, lifecycle owned by presenter itself (will Dispose on Close)
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
        }
    }
}