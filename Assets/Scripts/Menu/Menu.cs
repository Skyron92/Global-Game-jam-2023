using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Toggle windowed;
    public Toggle Fullscreen;

    private void Start() {
        windowed = GetComponent<Toggle>();
        Fullscreen = GetComponent<Toggle>();
    }

    private void Update() {
        if (windowed.isOn) Screen.fullScreenMode = FullScreenMode.Windowed;
        if (Fullscreen.isOn) Screen.fullScreen = true;
    }
    

    public void Retry() {
        SceneManager.LoadScene("SampleScene");
    }
    

    public void Quit() {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
    
}
