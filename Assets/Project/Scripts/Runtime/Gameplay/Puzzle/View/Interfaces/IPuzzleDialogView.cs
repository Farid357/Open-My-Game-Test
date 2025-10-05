using OMG.Configs;
using OMG.Models;
using UnityEngine;

namespace OMG.Views
{
    public interface IPuzzleDialogView : IReadOnlyPuzzleDialogView
    {
        void Show(PuzzleMeta meta, Sprite previewSmall);
       
        void Close();

        void UpdateStartMode(PuzzleStartMode mode, int coinsNeeded);
     
        void UpdateProgress(float progress);
        
        void SetInteractable(bool interactable);
        
        void SetStatus(string text);
        void ResetSelectedPieces();
    }
}