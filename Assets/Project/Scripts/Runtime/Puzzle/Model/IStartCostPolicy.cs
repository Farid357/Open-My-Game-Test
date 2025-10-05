using OMG.Configs;

namespace OMG.Models
{
    public interface IStartCostPolicy
    {
        StartMode GetStartModeForPieces(int pieces);
        
        int GetPuzzleCost();
    }
}