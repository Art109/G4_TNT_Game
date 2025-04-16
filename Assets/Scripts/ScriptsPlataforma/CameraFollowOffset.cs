using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollowOffset : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private CinemachineVirtualCamera virtualCam;
    [SerializeField]
    private PlayerScript playerScript;
    private CinemachineFramingTransposer transposer;

    [Header("Offset")]
    public float offsetX = 3f;

    [Header("Zoom")]
    [SerializeField]
    public float targetZoom = 4f;
    [SerializeField]
    private float zoomSpeed = 3f;
    public bool zooming = false;

    void Start()
    {
        transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        if (playerScript.isLookingCamera())
        {
            transposer.m_TrackedObjectOffset = new Vector3(offsetX, 0f, 0f);
        }
        else
        {
            transposer.m_TrackedObjectOffset = new Vector3(-offsetX, 0f, 0f);
        }

        if (zooming)
        {
            Zoom();
        }
    }

    void Zoom()
    {
        float currentZoom = virtualCam.m_Lens.OrthographicSize;
        float newZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        virtualCam.m_Lens.OrthographicSize = newZoom;
    }
}
