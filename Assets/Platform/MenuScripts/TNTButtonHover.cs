using UnityEngine;
using UnityEngine.EventSystems;

public class TNTButtonHover : MonoBehaviour
{
    public float floatHeight = 0.2f;
    public float shakeSpeed = 10f;
    public float shakeAmount = 0.05f;

    private Vector3 initialPosition;
    private bool isSelected;
    private bool wasSelectedLastFrame = false;

    [SerializeField]
    private AudioSource selectedAudio;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        isSelected = EventSystem.current.currentSelectedGameObject == this.gameObject;

        if (isSelected)
        {
            if (!wasSelectedLastFrame && selectedAudio != null)
            {
                selectedAudio.Play();
            }

            float yOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.localPosition = initialPosition + new Vector3(0f, floatHeight + yOffset, 0f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * 10f);
        }

        wasSelectedLastFrame = isSelected;
    }
}
