using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flutuation : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float range = 0.1f;
    [SerializeField]
    private float frequency = 1.5f;

    private Vector3 originalPosition;
    
    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        float flutuation = Mathf.Sin(Time.time * frequency) * range;
        transform.position = originalPosition + new Vector3(0, flutuation, 0);
    }
}
