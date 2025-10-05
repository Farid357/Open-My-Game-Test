using System;
using UniRx;

namespace OMG.Views
{
    public interface IReadOnlyPuzzleDialogView
    {
        IObservable<int> PiecesSelected { get; }
      
        IObservable<Unit> StartClicked { get; }
    
        IObservable<Unit> CloseClicked { get; }
    }
}