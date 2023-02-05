using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public AnimationCurve fade;
    public Image image;

    private void Awake()
    {
        Color imageColor = image.color;
        imageColor.a = fade.Evaluate(Time.deltaTime);
        image.color = imageColor;
    }
}
