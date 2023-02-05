using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Toggle windowed;
    public Toggle Fullscreen;
    private bool timerIsOn;
    private float timer;
    public GameObject credit;

    private void Start()
    {
        timerIsOn = false;
        windowed = GetComponent<Toggle>();
        Fullscreen = GetComponent<Toggle>();
    }

    private void Update() {
        /*if (windowed.isOn) Screen.fullScreenMode = FullScreenMode.Windowed;
        if (Fullscreen.isOn) Screen.fullScreenMode = FullScreenMode.FullScreenWindow;*/
        if (timerIsOn) timer += Time.deltaTime;
        Debug.Log(timer);
    }

    public void Retry() {
        SceneManager.LoadScene("SampleScene");
    }
    

    public void Quit() {
       /* EditorApplication.isPlaying = false;
        Application.Quit();*/
    }

    public void CloseCredit() {
        timerIsOn = true;
        if (timer >= 0.5f)
        {
            timerIsOn = false;
            timer = 0;
            credit.SetActive(false);
        }
    }
    
}
