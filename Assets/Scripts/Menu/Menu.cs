using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Retry() {
        SceneManager.LoadScene("SampleScene");
    }
    

    public void Quit() {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
