using UnityEngine;

namespace PentoPuzzle
{
    public class PieceManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject[] pieces = null;

        private const int boardWidth = 15;
        private const int boardHeight = 5;

        private bool[,] board = new bool[boardWidth, boardHeight];

        public static PieceManager instance;

        private void Awake()
        {
            instance = this;
        }

        // Returns whether piece with given tiles can be moved to given position
        public bool MovePiece(Vector2Int startPosition, Vector2Int position, Vector2Int[] tiles)
        {
            // If any position out of bounds, return false
            foreach (Vector2Int tile in tiles)
            {
                Vector2Int pos = position + tile;
                if (pos.x < 0 || pos.x >= boardWidth) return false;
                if (pos.y < 0 || pos.y >= boardHeight) return false;
            }

            // For each tile in piece
            foreach (Vector2Int tile in tiles)
            {
                // Get position of tile in board
                Vector2Int pos = position + tile;
                // If board filled at position, return false
                if (board[pos.x, pos.y]) return false;
            }

            // If all board positions empty, update board
            foreach (Vector2Int tile in tiles)
            {
                // Clear start position
                Vector2Int startPos = startPosition + tile;
                board[startPos.x, startPos.y] = false;
                // Populate new position
                Vector2Int pos = position + tile;
                board[pos.x, pos.y] = true;
            }

            // Return true
            return true;
        }
    }
}
