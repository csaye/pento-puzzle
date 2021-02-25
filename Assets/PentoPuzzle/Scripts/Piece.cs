using UnityEngine;

namespace PentoPuzzle
{
    public class Piece : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Vector2 pivot = new Vector2();
        [SerializeField] private Vector2Int[] tiles = null;

        public Vector2Int[] Tiles
        {
            get { return tiles; }
        }

        private int _rotation = 0;
        private int rotation
        {
            get { return _rotation; }
            set
            {
                if (value < 0 || value >= 4) _rotation = 0;
                else _rotation = value;
                // Rotate
                int zRot = 90 * rotation;
                transform.eulerAngles = new Vector3(0, 0, zRot);
            }
        }

        private bool _flip = false;
        private bool flip
        {
            get { return _flip; }
            set
            {
                if (_flip == value) return;
                else _flip = value;
                // Flip scale
                int xScale = flip ? -1 : 1;
                transform.localScale = new Vector3(xScale, 1, 1);
                // Flip tiles
                // for (int i = 0; i < tiles.Length; i++)
                // {
                //     Vector2Int tile = tiles[i];
                //     tiles[i] = 
                // }
            }
        }

        private Camera mainCamera;

        private bool offsetSet = false;
        private Vector2 mouseOffset;

        private Vector2Int[] startTiles = null;
        private Vector2 startPosition;
        private int startRotation;
        private bool startFlip;

        private void Start()
        {
            // Cache main camera reference
            mainCamera = Camera.main;

            // Register piece with manager
            Vector2Int position = Operation.RoundToInt((Vector2)transform.position - pivot);
            PieceManager.instance.InitializePiece(position, tiles);
        }

        // Move to mouse position on drag
        private void OnMouseDrag()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Set mouse offset if not set
            if (!offsetSet)
            {
                offsetSet = true;
                mouseOffset = (Vector2)transform.position - mousePosition;
                startTiles = tiles;
                startPosition = transform.position;
                // Set start rotation and flip
                startRotation = rotation;
                startFlip = flip;
            }
            // Rotate and flip
            if (Input.GetKeyDown(KeyCode.R)) rotation += 1;
            if (Input.GetKeyDown(KeyCode.F)) flip = !flip;
            // Round mouse position to snap to grid
            float xRaw = mousePosition.x + mouseOffset.x;
            float x = (pivot.x % 1 == 0.5f) ? Operation.RoundToHalf(xRaw) : Mathf.Round(xRaw);
            float yRaw = mousePosition.y + mouseOffset.y;
            float y = (pivot.y % 1 == 0.5f) ? Operation.RoundToHalf(yRaw) : Mathf.Round(yRaw);
            // Move to rounded mouse position
            transform.position = new Vector2(x, y);
        }

        private void OnMouseUp()
        {
            // Reset mouse offset on mouse up
            offsetSet = false;

            // If cannot move piece
            Vector2Int position = Operation.RoundToInt((Vector2)transform.position - pivot);
            Vector2Int startPos = Operation.RoundToInt(startPosition - pivot);
            if (!PieceManager.instance.MovePiece(startPos, startTiles, position, tiles))
            {
                // Reset position and tiles
                transform.position = startPosition;
                tiles = startTiles;
                // Reset rotation and flip
                // rotation = startRotation;
                flip = startFlip;
            }
        }
    }
}
