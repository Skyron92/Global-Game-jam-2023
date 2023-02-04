using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Toggle windowed;

    private void Start() {
        windowed = GetComponent<Toggle>();
    }

    private void Update() {
        if (windowed.isOn) Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    

    public void Retry() {
        SceneManager.LoadScene("SampleScene");
    }
    

    public void Quit() {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
    
}
