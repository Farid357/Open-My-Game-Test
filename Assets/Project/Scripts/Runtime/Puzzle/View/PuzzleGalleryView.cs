using System;
using System.Collections.Generic;
using OMG.Models;
using UniRx;
using UnityEngine;

namespace OMG.Views
{
    public sealed class PuzzleGalleryView : MonoBehaviour, IPuzzleGalleryView
    {
        [SerializeField] private RectTransform _itemsParent;
        [SerializeField] private PuzzleGalleryItemView _itemPrefab;
        [SerializeField] private GameObject _loadingPanel;

        private readonly Subject<PuzzleMeta> _onClick = new();
        private readonly List<PuzzleGalleryItemView> _instantiated = new();

        public IObservable<PuzzleMeta> OnPreviewClicked => _onClick;

        public void ShowItems(IReadOnlyList<(PuzzleMeta meta, Sprite sprite)> items)
        {
            foreach (var previousItemView in _instantiated)
            {
                Destroy(previousItemView);
            }
            
            _instantiated.Clear();

            foreach ((PuzzleMeta meta, Sprite sprite) in items)
            {
                PuzzleGalleryItemView item = Instantiate(_itemPrefab, _itemsParent);
                _instantiated.Add(item);

                item.Initialize(meta.Title, sprite, () => _onClick.OnNext(meta));
            }
        }

        public void ShowLoading(bool show)
        {
            _loadingPanel.SetActive(show);
        }

        private void OnDestroy()
        {
            _onClick?.OnCompleted();
        }
    }
}