using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private List<Transform> backgroundObjects;
    [SerializeField]
    private float intensityParallax = 0.5f;

    private Vector3 originalPositionCamera;

    void Start()
    {
        originalPositionCamera = Camera.main.transform.position;
    }

    void Update()
    {
        for (int i = 0; i < backgroundObjects.Count; i++)
        {
            float parallax = (originalPositionCamera.x - Camera.main.transform.position.x) * (i * intensityParallax);

            float newPosBackgroundX = backgroundObjects[i].position.x + parallax;

            Vector3 newPos = new Vector3(newPosBackgroundX, backgroundObjects[i].position.y, backgroundObjects[i].position.z);

            backgroundObjects[i].position = newPos;
        }

        originalPositionCamera = Camera.main.transform.position;
    }
}
