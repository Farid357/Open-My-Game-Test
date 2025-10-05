using OMG.Models;

namespace OMG.Configs
{
    public sealed class PuzzleStartPolicyAdapter : IStartCostPolicy
    {
        private readonly PuzzleStartPolicy _config;

        public PuzzleStartPolicyAdapter(PuzzleStartPolicy config)
        {
            _config = config;
        }

        public StartMode GetStartModeForPieces(int pieces)
        {
            if (pieces <= _config.MaxFreePieces)
                return StartMode.Free;

            if (pieces <= _config.MaxCoinsPieces)
                return StartMode.Coins;

            return StartMode.Ads;
        }

        public int GetPuzzleCost() => _config.PuzzleCost;
    }
}