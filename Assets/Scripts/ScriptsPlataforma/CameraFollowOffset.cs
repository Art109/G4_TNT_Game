using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollowOffset : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCam;
    [SerializeField]
    private float offsetX = 2f;
    [SerializeField]
    private PlayerScript playerScript;

    private CinemachineFramingTransposer transposer;

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
    }
}
