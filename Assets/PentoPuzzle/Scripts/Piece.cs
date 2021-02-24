using UnityEngine;

namespace PentoPuzzle
{
    public class Piece : MonoBehaviour
    {
        private Camera mainCamera;

        private void Start()
        {
            // Cache main camera reference
            mainCamera = Camera.main;
        }

        // Move to mouse position on drag
        private void OnMouseDrag()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Round mouse position to snap to grid
            float x = Mathf.Round(mousePosition.x);
            float y = Mathf.Round(mousePosition.y);
            // Move to rounded mouse position
            transform.position = new Vector2(x, y);
        }
    }
}
