using UnityEngine;

namespace PentoPuzzle
{
    public class Piece : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera = null;

        // Move to mouse position on drag
        public void OnMouseDrag()
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
