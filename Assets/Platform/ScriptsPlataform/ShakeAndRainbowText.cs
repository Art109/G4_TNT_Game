using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ShakeAndRainbowText : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI text;

    [Header("Image")]
    [SerializeField]
    private Image image;

    [Header("Rainbow")]
    [SerializeField]
    [Tooltip("Velocidade da alteração das cores")]
    private float speed = 1f;

    [Header("Flutuation and Shake")]
    public float amplitude = 5f; 
    public float frequency = 5f;
    public float rotationAmount = 5f; 
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = text.rectTransform.anchoredPosition;
    }

    void Update()
    {
        ShakeFlutuation();
        Rainbow();
    }

    void Rainbow()
    {
        float time = Time.time * speed;

        float r = Mathf.Sin(time + 0f) * 0.5f + 0.5f;
        float g = Mathf.Sin(time + 2f) * 0.5f + 0.5f;
        float b = Mathf.Sin(time + 4f) * 0.5f + 0.5f;

        text.color = new Color(r, g, b);
    }

    void ShakeFlutuation()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        float rotZ = Mathf.Sin(Time.time * frequency * 1.2f) * rotationAmount;

        text.rectTransform.anchoredPosition = originalPosition + new Vector3(0, offsetY, 0);
        text.rectTransform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
