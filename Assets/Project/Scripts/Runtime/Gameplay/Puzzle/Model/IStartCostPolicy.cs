using OMG.Configs;

namespace OMG.Models
{
    public interface IPuzzleStartCostPolicy
    {
        PuzzleStartMode GetStartModeForPieces(int pieces);
        
        int GetPuzzleCost();
    }
}