using UnityEngine;
using UnityEngine.SceneManagement;

namespace PentoPuzzle
{
    public class MenuButton : MonoBehaviour
    {
        // Loads given scene by name
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        // Quits application
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}