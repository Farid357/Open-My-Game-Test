using System;
using OMG.Configs;
using OMG.Models;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace OMG.Views
{
    public sealed class PuzzleDialogView : MonoBehaviour, IPuzzleDialogView
    {
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private Image _previewImage;
        [SerializeField] private TMP_Dropdown _piecesDropdown;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private TMP_Text _startButtonText;
        [SerializeField] private Slider _progressSlider;
        
        private readonly Subject<int> _onPieces = new();
        private readonly Subject<Unit> _onStart = new();
        private readonly Subject<Unit> _onClose = new();

        public IObservable<int> PiecesSelected => _onPieces;
        public IObservable<Unit> StartClicked => _onStart;
        public IObservable<Unit> CloseClicked => _onClose;

        private void Awake()
        {
            _piecesDropdown.onValueChanged.AsObservable().Subscribe(i => _onPieces.OnNext(ParseDropdown(_piecesDropdown.options[i].text))).AddTo(this);
            _startButton.onClick.AsObservable().Subscribe(_ => _onStart.OnNext(Unit.Default)).AddTo(this);
            _closeButton.onClick.AsObservable().Subscribe(_ => _onClose.OnNext(Unit.Default)).AddTo(this);

            HideImmediate();
        }

        private int ParseDropdown(string text) => int.TryParse(text, out int v) ? v : 9;

        public void Show(PuzzleMeta meta, Sprite previewSmall)
        {
            _root.gameObject.SetActive(true);
            _previewImage.sprite = previewSmall;
            _previewImage.preserveAspect = true;
            _statusText.text = meta.Title;
            _progressSlider.value = 0f;
            SetInteractable(true);
        }

        public void Close() => _root.gameObject.SetActive(false);

        public void UpdateStartMode(PuzzleStartMode mode, int coinsNeeded)
        {
            _startButtonText.text = mode switch
            {
                PuzzleStartMode.Free => "Start (Free)",
                PuzzleStartMode.Coins => $"Start ({coinsNeeded} coins)",
                PuzzleStartMode.Ads => "Watch Ad to Start",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
        }

        public void ResetSelectedPieces() => _piecesDropdown.SetValueWithoutNotify(0);
        
        public void UpdateProgress(float progress) => _progressSlider.value = Mathf.Clamp01(progress);

        public void SetInteractable(bool interactable) => _root.interactable = interactable;

        public void SetStatus(string text) => _statusText.text = text;

        private void HideImmediate() => _root.gameObject.SetActive(false);

        private void OnDestroy()
        {
            _onPieces?.OnCompleted();
            _onStart?.OnCompleted();
            _onClose?.OnCompleted();
        }
    }
}