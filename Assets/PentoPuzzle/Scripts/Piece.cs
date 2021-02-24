using UnityEngine;

namespace PentoPuzzle
{
    public class Piece : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private Vector2Int[] tiles = null;

        private Camera mainCamera;

        private bool offsetSet = false;
        private Vector2 mouseOffset;

        public Vector2Int[] Tiles
        {
            get { return tiles; }
        }

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
            }
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
        }
    }
}
