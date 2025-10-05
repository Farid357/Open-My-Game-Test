using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using OMG.Models;
using UnityEngine;

namespace OMG.Services
{
    public sealed class MockPreviewService : IPreviewService
    {
        public async UniTask<IReadOnlyList<PuzzleMeta>> GetAvailablePreviewsAsync(CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(250, cancellationToken: cancellationToken);
         
            var puzzleMetas = new List<PuzzleMeta>
            {
                new PuzzleMeta("pzl_001", "Cat", "mock://cat"),
                new PuzzleMeta("pzl_002", "Mountain", "mock://mountain"),
                new PuzzleMeta("pzl_003", "Space", "mock://space"),
                new PuzzleMeta("pzl_004", "Forest", "mock://forest"),
                new PuzzleMeta("pzl_005", "Tiger", "mock://tiger"),
                new PuzzleMeta("pzl_006", "Lion", "mock://lion")
            };
            
            return puzzleMetas;
        }

        public async UniTask<Texture2D> LoadPreviewTextureAsync(string url, CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(150, cancellationToken: cancellationToken);

            var texture = new Texture2D(64, 64);
            Color color = Color.gray;
            if (url.Contains("cat"))
            {
                color = Color.magenta;
            }
            else if (url.Contains("mountain"))
            {
                color = Color.cyan;
            }
            else if (url.Contains("space"))
            {
                color = Color.black;
            }
            else if (url.Contains("forest"))
            {
                color = Color.green;
            }

            else if (url.Contains("tiger"))
            {
                color = Color.yellow;
            }
            else if(url.Contains("lion"))
            {
                color = Color.red;
            }
            
            var pixels = new Color[64 * 64];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}