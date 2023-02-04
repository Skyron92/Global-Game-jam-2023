using UnityEngine;
using UnityEngine.UI;

public class HydratationManager : MonoBehaviour
{
    public Image LoadingBar;
    public static float currentValue = 100f;
    private int speed = 5;

    void Update () {
		if (currentValue > 0) currentValue -= speed * Time.deltaTime;
		if (currentValue > 100) currentValue = 100;
		LoadingBar.fillAmount = currentValue / 100;
	}
}
