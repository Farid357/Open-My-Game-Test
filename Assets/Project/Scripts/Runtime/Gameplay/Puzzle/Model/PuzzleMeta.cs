using System;

namespace OMG.Models
{
    public sealed class PuzzleMeta
    {
        public string PuzzleId { get; }
        public string Title { get; }
        public string PreviewUrl { get; }

        public PuzzleMeta(string puzzleId, string title, string previewUrl)
        {
            PuzzleId = puzzleId ?? throw new ArgumentNullException(nameof(puzzleId));
            Title = title ?? string.Empty;
            PreviewUrl = previewUrl ?? string.Empty;
        }
    }
}