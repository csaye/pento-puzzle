using UnityEngine;
using UnityEngine.SceneManagement;

namespace PentoPuzzle
{
    public class ResetButton : MonoBehaviour
    {
        // Reloads current scene
        public void ReloadScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
