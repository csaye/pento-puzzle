using UnityEngine;

namespace PentoPuzzle
{
    public class Piece : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Vector2Int size = new Vector2Int();
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
                transform.eulerAngles = new Vector3(0, 0, 90 * rotation);
            }
        }

        private bool _flip = false;
        private bool flip
        {
            get { return _flip; }
            set
            {
                _flip = value;
                int xScale = flip ? -1 : 1;
                transform.localScale = new Vector3(xScale, 1, 1);
            }
        }

        private Camera mainCamera;

        private bool offsetSet = false;
        private Vector2 mouseOffset;
        private Vector2Int[] startTiles = null;
        private Vector2Int startPosition;
        private int startRotation;
        private bool startFlip;

        private void Start()
        {
            // Cache main camera reference
            mainCamera = Camera.main;
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
                startRotation = rotation;
                startFlip = flip;
                startPosition = Operation.RoundToInt(transform.position);
            }
            // Rotate and flip
            if (Input.GetKeyDown(KeyCode.R)) rotation += 1;
            if (Input.GetKeyDown(KeyCode.F)) flip = !flip;
            // Round mouse position to snap to grid
            float x = Mathf.Round(mousePosition.x + mouseOffset.x);
            float y = Mathf.Round(mousePosition.y + mouseOffset.y);
            // Move to rounded mouse position
            transform.position = new Vector2(x, y);
        }

        private void OnMouseUp()
        {
            // Reset mouse offset on mouse up
            offsetSet = false;

            // If cannot move piece
            Vector2Int position = Operation.RoundToInt(transform.position);
            if (!PieceManager.instance.MovePiece(startPosition, startTiles, position, tiles))
            {
                // Reset position and tiles
                transform.position = (Vector2)startPosition;
                tiles = startTiles;
                // Reset rotation and flip
                rotation = startRotation;
                flip = startFlip;
            }
        }
    }
}
