using UnityEngine;

namespace PentoPuzzle
{
    public class Pointer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetState(bool state)
        {
            spriteRenderer.color = state ? new Color(0.5f, 0, 0, 1) : new Color(0, 0.5f, 0, 1);
        }
    }
}
