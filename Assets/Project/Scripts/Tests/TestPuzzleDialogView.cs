using System;
using UniRx;
using OMG.Models;
using OMG.Views;
using UnityEngine;

namespace OMG.Tests.TestHelpers
{
    public sealed class TestPuzzleDialogView : IPuzzleDialogView
    {
        private readonly Subject<int> _pieces = new();
        private readonly Subject<Unit> _start = new();
        private readonly Subject<Unit> _close = new();

        public IObservable<int> PiecesSelected => _pieces;
        public IObservable<Unit> StartClicked => _start;
        public IObservable<Unit> CloseClicked => _close;

        // Observability for assertions
        public string LastStatus { get; private set; }
        public int LastCoins { get; private set; }
        public float LastProgress { get; private set; }
        public bool LastInteractable { get; private set; }
        public bool CloseCalled { get; private set; }
        public bool ResetCalled { get; private set; }
        public bool UpdateStartModeCalled { get; private set; }

        public void TriggerPieces(int p) => _pieces.OnNext(p);
        public void TriggerStart() => _start.OnNext(Unit.Default);
        public void TriggerClose() => _close.OnNext(Unit.Default);

        public void Show(PuzzleMeta meta, Sprite previewSmall) { /* noop for tests */ }
        public void Close()
        {
            CloseCalled = true;
        }

        public void UpdateStartMode(OMG.Configs.PuzzleStartMode mode, int coinsNeeded)
        {
            UpdateStartModeCalled = true;
        }

        public void UpdateProgress(float progress)
        {
            LastProgress = progress;
        }

        public void SetInteractable(bool interactable)
        {
            LastInteractable = interactable;
        }

        public void SetStatus(string text)
        {
            LastStatus = text;
        }

        public void ResetSelectedPieces()
        {
            ResetCalled = true;
        }
    }
}
