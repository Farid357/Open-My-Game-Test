using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using OMG.Models;
using UnityEngine;

namespace OMG.Services
{
    public interface IPreviewService
    {
        UniTask<IReadOnlyList<PuzzleMeta>> GetAvailablePreviewsAsync(CancellationToken cancellationToken = default);
        UniTask<Texture2D> LoadPreviewTextureAsync(string url, CancellationToken cancellationToken = default);
    }
}