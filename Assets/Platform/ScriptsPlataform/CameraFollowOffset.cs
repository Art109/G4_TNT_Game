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
    [SerializeField]
    private float offsetY = 0f;

    void Start()
    {
        transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        if (playerScript.isLookingCamera())
        {
            transposer.m_TrackedObjectOffset = new Vector3(offsetX, offsetY, 0f);
        }
        else
        {
            transposer.m_TrackedObjectOffset = new Vector3(-offsetX, offsetY, 0f);
        }

        if (zooming)
        {
            Zoom();
            offsetY = 0.5f;
        }
    }

    void Zoom()
    {
        float currentZoom = virtualCam.m_Lens.OrthographicSize;
        float newZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        virtualCam.m_Lens.OrthographicSize = newZoom;
    }
}
