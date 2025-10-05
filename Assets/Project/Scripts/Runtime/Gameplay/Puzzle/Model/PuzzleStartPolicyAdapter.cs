using OMG.Models;

namespace OMG.Configs
{
    public sealed class PuzzleStartPolicyAdapter : IPuzzleStartCostPolicy
    {
        private readonly PuzzleStartPolicy _config;

        public PuzzleStartPolicyAdapter(PuzzleStartPolicy config)
        {
            _config = config;
        }

        public PuzzleStartMode GetStartModeForPieces(int pieces)
        {
            if (pieces <= _config.MaxFreePieces)
                return PuzzleStartMode.Free;

            if (pieces <= _config.MaxCoinsPieces)
                return PuzzleStartMode.Coins;

            return PuzzleStartMode.Ads;
        }

        public int GetPuzzleCost() => _config.PuzzleCost;
    }
}