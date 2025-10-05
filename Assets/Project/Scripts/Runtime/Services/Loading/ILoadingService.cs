using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace OMG.Services
{
    public interface ILoadingService
    {
        UniTask LoadAsync(IProgress<float> onProgress, CancellationToken cancellationToken = default);
    }
}