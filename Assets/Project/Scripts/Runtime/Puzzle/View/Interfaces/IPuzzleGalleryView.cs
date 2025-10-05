using System;
using System.Collections.Generic;
using OMG.Models;
using UnityEngine;

namespace OMG.Views
{
    public interface IPuzzleGalleryView
    {
        void ShowItems(IReadOnlyList<(PuzzleMeta meta, Sprite sprite)> items);
   
        IObservable<PuzzleMeta> OnPreviewClicked { get; }
      
        void ShowLoading(bool show);
    }
}