using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydratationManager : MonoBehaviour
{
    public Image LoadingBar;
    private float currentValue = 100f;
    private int speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

  	void Update () {
		if (currentValue > 0) {
			currentValue -= speed * Time.deltaTime;
		}
 
		LoadingBar.fillAmount = currentValue / 100;
	}
}
