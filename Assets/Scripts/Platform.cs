using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float currentSpeed;
	public float deltaX;
	private float xPosT0;
	private float xPosT1;
    void Start()
    {
       xPosT0 = transform.position.x; 
    }

    // Update is called once per frame
    void Update()
    {
        xPosT1 = transform.position.x;
		deltaX = (xPosT1 - xPosT0);
		currentSpeed = deltaX/Time.deltaTime;
		xPosT0 = transform.position.x;
    }
}
