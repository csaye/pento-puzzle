using UnityEngine;

namespace PentoPuzzle
{
    public class Operation
    {
        public static Vector2Int RoundToInt(Vector2 v2)
        {
            int x = Mathf.RoundToInt(v2.x);
            int y = Mathf.RoundToInt(v2.y);
            return new Vector2Int(x, y);
        }
    }
}
