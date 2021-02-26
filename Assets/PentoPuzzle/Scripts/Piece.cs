using UnityEngine;

namespace PentoPuzzle
{
    public class Piece : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Vector2Int size = new Vector2Int();
        // [SerializeField] private Vector2 pivot = new Vector2();
        [SerializeField] private bool halfPivot = false;
        [SerializeField] private Vector2Int[] tiles = null;

        public bool HalfPivot
        {
            get { return halfPivot; }
        }

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
                if (_rotation == value) return;
                else if (value < 0) _rotation = 3;
                else _rotation = value;
                // Rotate
                int zRot = 90 * rotation;
                transform.eulerAngles = new Vector3(0, 0, zRot);
                Rotate();
            }
        }

        // Rotates tiles clockwise around pivot
        private void Rotate()
        {
            // Rotate tiles
            for (int i = 0; i < tiles.Length; i++)
            {
                Vector2Int tile = tiles[i];
                int tileX = tile.y;
                int tileY = halfPivot ? -tile.x : -tileX - 1;
                tiles[i] = new Vector2Int(tileX, tileY);
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
                for (int i = 0; i < tiles.Length; i++)
                {
                    Vector2Int tile = tiles[i];
                    tiles[i] = new Vector2Int(size.x - tile.x - 1, tile.y);
                }
            }
        }

        private Camera mainCamera;

        private bool offsetSet = false;
        private Vector2 mouseOffset;

        private Vector2Int[] startTiles = new Vector2Int[5];
        private Vector2 startPosition;
        private int startRotation;
        private bool startFlip;

        private void Start()
        {
            // Cache main camera reference
            mainCamera = Camera.main;

            // Register piece with manager
            Vector2Int position = halfPivot ? Operation.FloorToInt(transform.position) : Operation.RoundToInt(transform.position);
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
                // Shallow clone tiles
                for (int i = 0; i < tiles.Length; i++) startTiles[i] = tiles[i];
                startPosition = transform.position;
                // Set start rotation and flip
                startRotation = rotation;
                startFlip = flip;
            }
            // Rotate and flip
            if (Input.GetKeyDown(KeyCode.R)) rotation -= 1;
            if (Input.GetKeyDown(KeyCode.F)) flip = !flip;
            // Round mouse position to snap to grid
            float xRaw = mousePosition.x + mouseOffset.x;
            float x = halfPivot ? Operation.RoundToHalf(xRaw) : Mathf.Round(xRaw);
            float yRaw = mousePosition.y + mouseOffset.y;
            float y = halfPivot ? Operation.RoundToHalf(yRaw) : Mathf.Round(yRaw);
            // Move to rounded mouse position
            transform.position = new Vector2(x, y);
        }

        private void OnMouseUp()
        {
            // Reset mouse offset on mouse up
            offsetSet = false;

            // If cannot move piece
            Vector2Int position = halfPivot ? Operation.FloorToInt(transform.position) : Operation.RoundToInt(transform.position);
            Vector2Int startPos = halfPivot ? Operation.FloorToInt(startPosition) : Operation.RoundToInt(startPosition);
            if (!PieceManager.instance.MovePiece(startPos, startTiles, position, tiles))
            {
                // Reset position and tiles
                transform.position = startPosition;
                // Shallow clone tiles
                for (int i = 0; i < tiles.Length; i++) tiles[i] = startTiles[i];
                // Reset rotation and flip
                rotation = startRotation;
                flip = startFlip;
            }
        }
    }
}
