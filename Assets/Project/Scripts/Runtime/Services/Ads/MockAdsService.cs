using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace OMG.Services
{
    public sealed class MockAdsService : IAdsService
    {
        public async UniTask<bool> ShowRewardedAdAsync(CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(1200, cancellationToken: cancellationToken);
            Debug.Log("[MockAdsService] Ad shown (mock).");
            return true;
        }
    }
}