using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Play Button")]
    [SerializeField]
    [Tooltip("A câmera se movimentará para essa posição")]
    private Transform targetPosition;
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private Transform mainCamera;
    [SerializeField]
    private bool isMoving = false;
    private bool inMenu = true;
    private bool isFocusApplied = false;

    [Header("Buttons Objects Menu")]
    [SerializeField]
    private GameObject buttonPlay;
    [SerializeField]
    private GameObject buttonCredits;
    [SerializeField]
    private GameObject buttonQuit;

    [Header("Buttons Transform Menu")]
    [SerializeField]
    private Transform buttonPlayTransform;
    [SerializeField]
    private Transform buttonCreditsTransform;
    [SerializeField]
    private Transform buttonQuitTransform;
    private Vector3 originalScale; 
    private Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1f);

    [Header("Buttons Image Menu")]
    [SerializeField]
    private Image buttonImagePlay;
    [SerializeField]
    private Image buttonImageCredits;
    [SerializeField]
    private Image buttonImageQuit;

    [Header("Buttons TNT")]
    [SerializeField]
    private GameObject TNTEnergyButton;
    [SerializeField]
    private GameObject TNTMangoButton;
    [SerializeField]
    private GameObject TNTFocusButton;

    [Header("Canvas Carregamento")]
    [SerializeField]
    private GameObject canvasLoading;

    [Header("Mage")]
    [SerializeField]
    private Animator mageAnimator;

    [Header("Sound")]
    [SerializeField]
    private AudioSource selectAudio;
    private bool isSoundPlayed = false;
    private GameObject lastSelectedButton = null;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        originalScale = buttonPlayTransform.localScale;
    }

    private void Update()
    {
        HoverButton();


        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SetInitialSelection();
        }

        if (isMoving && inMenu)
        {
            mainCamera.position = Vector3.Lerp(mainCamera.position, targetPosition.position, moveSpeed * Time.deltaTime);
            EventSystem.current.SetSelectedGameObject(TNTEnergyButton);
     




            if (Vector3.Distance(mainCamera.position, targetPosition.position) < 0.01f)
            {
                mainCamera.position = targetPosition.position;
                isMoving = false;
                inMenu = false;
               
            }
        }
    }

    public void HoverButton()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != lastSelectedButton)
        {
            if (currentSelected == buttonPlay || currentSelected == buttonCredits || currentSelected == buttonQuit)
            {
                selectAudio.Play(); 
            }

            lastSelectedButton = currentSelected; 
        }

        if (EventSystem.current.currentSelectedGameObject == buttonPlay)
        {
            buttonPlayTransform.localScale = hoverScale;
            buttonImagePlay.color = Color.Lerp(buttonImagePlay.color, new Color(0.5f, 0.5f, 0.5f), Time.deltaTime * 5f); // Cor mais clara
        }
        else
        {
            buttonPlayTransform.localScale = originalScale;
            buttonImagePlay.color = Color.Lerp(buttonImagePlay.color, new Color(0f, 0f, 0f), Time.deltaTime * 5f); // Cor preta
        }

        // Credits Button
        if (EventSystem.current.currentSelectedGameObject == buttonCredits)
        {
            buttonCreditsTransform.localScale = hoverScale;
            buttonImageCredits.color = Color.Lerp(buttonImageCredits.color, new Color(0.5f, 0.5f, 0.5f), Time.deltaTime * 5f); // Cor mais clara
        }
        else
        {
            buttonCreditsTransform.localScale = originalScale;
            buttonImageCredits.color = Color.Lerp(buttonImageCredits.color, new Color(0f, 0f, 0f), Time.deltaTime * 5f); // Cor preta
        }

        // Quit Button
        if (EventSystem.current.currentSelectedGameObject == buttonQuit)
        {
            buttonQuitTransform.localScale = hoverScale;
            buttonImageQuit.color = Color.Lerp(buttonImageQuit.color, new Color(0.5f, 0.5f, 0.5f), Time.deltaTime * 5f); // Cor mais clara
            mageAnimator.Play("angry");
        }
        else
        {
            buttonQuitTransform.localScale = originalScale;
            buttonImageQuit.color = Color.Lerp(buttonImageQuit.color, new Color(0f, 0f, 0f), Time.deltaTime * 5f); // Cor preta
            mageAnimator.Play("idle");
        }
    }

    public void StartMoving()
    {
        ToggleMenu();
        StartCoroutine(SetFocusOnNewMenuButton());
        isMoving = true;
    }
        
    void ToggleMenu()
    {
        buttonPlay.SetActive(false);
        buttonCredits.SetActive(false);
        buttonQuit.SetActive(false);


        TNTEnergyButton.SetActive(true);
        TNTMangoButton.SetActive(true);
        TNTFocusButton.SetActive(true);
    }

    private void SetInitialSelection()
    {
        if (inMenu)
        {
            EventSystem.current.SetSelectedGameObject(buttonPlay);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(TNTEnergyButton);
        }
    }

    private IEnumerator SetFocusOnNewMenuButton()
    {
        yield return new WaitForSeconds(0f);
        EventSystem.current.SetSelectedGameObject(TNTEnergyButton);

 
        if (EventSystem.current.currentSelectedGameObject != TNTEnergyButton)
        {
            EventSystem.current.SetSelectedGameObject(TNTEnergyButton);
        }

        
    }

    public void Quit()
    {
        StartCoroutine(QuitCoroutine());
    }

    IEnumerator QuitCoroutine()
    {
        selectAudio.Play();
        yield return new WaitForSeconds(0.15f);
        Application.Quit();
        Debug.Log("SAIU");
    }

}
