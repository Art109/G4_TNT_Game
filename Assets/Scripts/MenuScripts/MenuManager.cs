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
    private Transform targetPositionFrigobar;
    [SerializeField]
    private Transform targetPositionMenu;
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private Transform mainCamera;
    [SerializeField]
    private bool isMoving = false;
    private bool inMenu = true;

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
    [SerializeField]
    private GameObject BackMenuButton;

    [Header("Canvas Carregamento")]
    [SerializeField]
    private GameObject canvasLoading;

    [Header("Characters")]
    [SerializeField]
    private Animator mageAnimator;
    [SerializeField]
    private Animator witchAnimator;

    [Header("Sound")]
    [SerializeField]
    private AudioSource selectAudio;
    private GameObject lastSelectedButton = null;

    private bool goingToFrigobar = false;
    private bool returningToMenu = false;

    [Header("Botões Interação")]
    [SerializeField]
    private Button PlayInteract;
    [SerializeField]
    private Button CreditsInteract;
    [SerializeField]
    private Button QuitInteract;

    [SerializeField]
    private Button BackInteract;
    [SerializeField]
    private Button EnergyInteract;
    [SerializeField]
    private Button MangoInteract;
    [SerializeField]
    private Button FocusInteract;


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        originalScale = buttonPlayTransform.localScale;

        ActiveDesactiveMenu(true);
        ActiveDesactiveLata(false);
    }

    private void Update()
    {
        HoverButton();
        if(EventSystem.current.currentSelectedGameObject == null)
            SetInitialSelection();

        if (goingToFrigobar)
        {
            mainCamera.position = Vector3.Lerp(mainCamera.position, targetPositionFrigobar.position, moveSpeed * Time.deltaTime);
            EventSystem.current.SetSelectedGameObject(TNTEnergyButton);
            ActiveDesactiveMenu(false);
            if (Vector3.Distance(mainCamera.position, targetPositionFrigobar.position) < 0.01f)
            {
                ActiveDesactiveLata(true);
                StartCoroutine(SetFocusOnNewMenuButton());
                mainCamera.position = targetPositionFrigobar.position;
                goingToFrigobar = false;
            }
        }

        if (returningToMenu)
        {
            mainCamera.position = Vector3.Lerp(mainCamera.position, targetPositionMenu.position, moveSpeed * Time.deltaTime);

            EventSystem.current.SetSelectedGameObject(buttonPlay);
            ActiveDesactiveLata(false);
            if (Vector3.Distance(mainCamera.position, targetPositionMenu.position) < 0.01f)
            {
                ActiveDesactiveMenu(true);
                mainCamera.position = targetPositionMenu.position;
                returningToMenu = false;
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
            witchAnimator.Play("fly");
        }
        else
        {
            buttonQuitTransform.localScale = originalScale;
            buttonImageQuit.color = Color.Lerp(buttonImageQuit.color, new Color(0f, 0f, 0f), Time.deltaTime * 5f); // Cor preta
            mageAnimator.Play("idle");
            witchAnimator.Play("idle");
        }
    }

    public void StartMoving()
    {
        goingToFrigobar = true;
        inMenu = false;
    }

    public void ReturnMenu()
    {
        returningToMenu = true;
        inMenu = true;
    }

    public void ActiveDesactiveMenu(bool status)
    {
        PlayInteract.interactable = status;
        CreditsInteract.interactable = status;
        QuitInteract.interactable = status;
    }
    
    void ActiveDesactiveLata(bool status)
    {
        BackInteract.interactable = status;
        EnergyInteract.interactable = status;
        MangoInteract.interactable = status;
        FocusInteract.interactable = status;
    }

    private void SetInitialSelection()
    {
        if (goingToFrigobar || returningToMenu)
            return;

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
    }

}
