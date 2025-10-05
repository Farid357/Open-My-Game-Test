using System.Threading;
using Cysharp.Threading.Tasks;

namespace OMG.Services
{
    public interface IAdsService
    {
        UniTask<bool> ShowRewardedAdAsync(CancellationToken cancellationToken = default);
    }
}