using OMG.Models;

namespace OMG.Presenters
{
    public sealed class PuzzleStartContext
    {
        public PuzzleMeta Meta { get; }
        public int Pieces { get; }

        public PuzzleStartContext(PuzzleMeta meta, int pieces)
        {
            Meta = meta;
            Pieces = pieces;
        }
    }
}