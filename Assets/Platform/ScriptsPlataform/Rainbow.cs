using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rainbow : MonoBehaviour
{
    private Image image;
    private float hue = 0f;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        hue += Time.deltaTime * 0.2f;
        if (hue > 1f)
            hue = 0f;

        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);
        image.color = rainbowColor;
    }
}
