using UnityEngine;

namespace OMG.Configs
{
    [CreateAssetMenu(fileName = "PuzzleStartPolicy", menuName = "OMG/Configs/PuzzleStartPolicy")]
    public sealed class PuzzleStartPolicy : ScriptableObject
    {
        [Header("Piece thresholds")]
        [Tooltip("Max pieces count for free start (inclusive).")]
        [SerializeField] private int _maxFreePieces = 16;

        [Tooltip("Max pieces count for coin-based start (inclusive).")]
        [SerializeField] private int _maxCoinsPieces = 49;

        [Header("Costs")]
        [Tooltip("Coin cost when coin-based start is selected.")]
        [SerializeField] private int _puzzleCost = 25;

        public int MaxFreePieces => _maxFreePieces;
    
        public int MaxCoinsPieces => _maxCoinsPieces;
     
        public int PuzzleCost => _puzzleCost;
    }
}