using System.Threading;
using Cysharp.Threading.Tasks;
using OMG.Configs;

namespace OMG.Presenters
{
    public interface IPuzzleStartStrategy
    {
        PuzzleStartMode Mode { get; }
       
        UniTask<bool> TryStartAsync(PuzzleStartContext context, CancellationToken cancellationToken);
    }
}