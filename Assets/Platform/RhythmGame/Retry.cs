using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public GameObject fadeOut;
    public GameObject gameOver, retry, giveUp;
    public AudioSource music;
    public AudioSource select;
    GamepadInput GamepadInputComponent;
    private bool isRetrying = false;

    private void Awake()
    {
        GamepadInputComponent = FindObjectOfType<GamepadInput>();
    }
    private void Start()
    {
        Invoke("MusicAfterDelay", 2f);
        Invoke("GameOverAfterDelay", 2.2f);
    }
    void Update()
    {
        // RETRY SYSTEM

        // GamePad
        if (GamepadInputComponent.onButtonDown["ActionButton"] && isRetrying == false)
        {
            select.Play();
            fadeOut.SetActive(true);
            Invoke("RetryAfterDelay", 3f);
            isRetrying = true;
            music.Stop();
        }

        if (GamepadInputComponent.onButtonDown["BackButton"])
        {
            select.Play();
            fadeOut.SetActive(true);
            Invoke("GiveUpAfterDelay", 3f);
            isRetrying = true;
            music.Stop();
        }

            // KeyBoard
            if (Input.GetKeyDown(KeyCode.Space) && isRetrying == false)
        {
            select.Play();
            fadeOut.SetActive(true);
            Invoke("RetryAfterDelay", 3f);
            isRetrying = true;
            music.Stop();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            select.Play();
            fadeOut.SetActive(true);
            Invoke("GiveUpAfterDelay", 3f);
            isRetrying = true;
            music.Stop();
        }
    }
    void RetryAfterDelay()
    {
        SceneManager.LoadScene(3);
    }
    void GiveUpAfterDelay()
    {
        SceneManager.LoadScene(0);
    }
    void MusicAfterDelay()
    {
        if (isRetrying == false)
        {
            music.Play();
        }
        else
        {
            music.Stop();
        }
    }
    void GameOverAfterDelay()
    {
        gameOver.SetActive(true);
        retry.SetActive(true);
        giveUp.SetActive(true);
    }
}
