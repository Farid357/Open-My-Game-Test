using OMG.Configs;
using OMG.Models;
using UnityEngine;

namespace OMG.Views
{
    public interface IPuzzleDialogView : IReadOnlyPuzzleDialogView
    {
        void Show(PuzzleMeta meta, Sprite previewSmall);
       
        void Close();

        void UpdateStartMode(StartMode mode, int coinsNeeded);
     
        void UpdateProgress(float progress);
        
        void SetInteractable(bool interactable);
        void SetCoins(int coins);
        
        void SetStatus(string text);
    }
}