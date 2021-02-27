using TMPro;
using UnityEngine;

namespace PentoPuzzle
{
    public class PieceManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject[] pieces = null;
        [SerializeField] private TextMeshProUGUI winText = null;

        private const int boardWidth = 15;
        private const int boardHeight = 5;

        private bool[,] board = new bool[boardWidth, boardHeight];

        public static PieceManager instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            GeneratePieces();
        }

        public void InitializePiece(Vector2Int position, Vector2Int[] tiles)
        {
            foreach (Vector2Int tile in tiles)
            {
                // Set board
                Vector2Int pos = position + tile;
                board[pos.x, pos.y] = true;
            }
        }

        // Generates an initial piece layout
        private void GeneratePieces()
        {
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    GameObject piece = pieces[Random.Range(0, pieces.Length)];
                    Piece pieceComponent = piece.GetComponent<Piece>();
                    bool halfPivot = pieceComponent.HalfPivot;
                    Vector2Int[] tiles = pieceComponent.Tiles;
                    Vector2Int position = new Vector2Int(x, y);
                    Vector2 piecePos = halfPivot ? position + new Vector2(0.5f, 0.5f) : position;
                    // If valid piece position
                    if (ValidPiecePosition(position, tiles))
                    {
                        // Instantiate piece
                        Instantiate(piece, piecePos, Quaternion.identity, transform);
                        // Update board
                        foreach (Vector2Int tile in tiles)
                        {
                            Vector2Int pos = position + tile;
                            board[pos.x, pos.y] = true;
                        }
                    }
                }
            }
        }

        // Returns whether piece with given tiles is valid at given position
        private bool ValidPiecePosition(Vector2Int position, Vector2Int[] tiles)
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

            // If valid, return true
            return true;
        }

        // Returns whether piece with given tiles can be moved to given position
        public bool MovePiece(Vector2Int startPosition, Vector2Int[] startTiles, Vector2Int position, Vector2Int[] tiles)
        {
            // Debug.Log($"Start position: {startPosition}");
            // foreach (Vector2Int tile in startTiles) Debug.Log(tile);
            // Debug.Log($"End position: {position}");
            // foreach (Vector2Int tile in tiles) Debug.Log(tile);

            foreach (Vector2Int tile in startTiles)
            {
                // Clear start position
                Vector2Int startPos = startPosition + tile;
                board[startPos.x, startPos.y] = false;
            }

            // If not valid piece position, reset start position and return false
            if (!ValidPiecePosition(position, tiles))
            {
                foreach (Vector2Int tile in startTiles)
                {
                    // Reset start position
                    Vector2Int startPos = startPosition + tile;
                    board[startPos.x, startPos.y] = true;
                }

                return false;
            }

            foreach (Vector2Int tile in tiles)
            {
                // Populate new position
                Vector2Int pos = position + tile;
                board[pos.x, pos.y] = true;
            }

            // Check for win and return true
            CheckWin();
            return true;
        }

        // Checks whether the player has won
        private void CheckWin()
        {
            // For entire board
            for (int x = boardWidth / 3; x < (2 * boardWidth) / 3; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    // If board not filled, return
                    if (!board[x, y]) return;
                }
            }

            winText.text = "Win";
        }
    }
}
