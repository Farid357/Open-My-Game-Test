using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace OMG.Services
{
    public sealed class MockLoadingService : ILoadingService
    {
        private readonly float _durationSeconds;

        public MockLoadingService(float durationSeconds = 1.5f)
        {
            _durationSeconds = Mathf.Max(0.1f, durationSeconds);
        }

        public async UniTask LoadAsync(System.IProgress<float> onProgress, CancellationToken cancellationToken = default)
        {
            const int steps = 20;
            for (int i = 0; i <= steps; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                float p = (float)i / steps;
                onProgress?.Report(p);
                await UniTask.Delay(TimeSpan.FromSeconds(_durationSeconds / steps), cancellationToken: cancellationToken);
            }
            onProgress?.Report(1f);
        }
    }
}