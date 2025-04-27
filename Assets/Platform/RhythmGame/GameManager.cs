using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // Components
    public AudioSource audSrc;
    public AudioSource missSrc;
    public AudioSource winSrc;
    public AudioSource finSrc;
    public bool startplaying;
    public NoteScroll noteScroll;
    public static GameManager instance;
    public bool menuPause = false;
    public bool finalContinue = false;
    GamepadInput GamepadInputComponent;


    // Scores
    public int scorePerBadHit = 100;
    public int scorePerGoodHit = 125;
    public int scorePerPerfectHit = 170;
    public int scorePerMissHit = 100;
    public int score;
    public Text scoreText;
    public Text multiText;

    // Multiplier
    public int multiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    // Health
    public int maxPlayerHealth = 100;
    public int playerHealth;
    public int damagePerMiss = 10;
    public int healingPerHit = 15;
    public Slider healthBar;
    public bool missed = false;

    // Results
    public float totalNotes;
    public float totalNotesLeft;
    public float totalNotesRight;
    public float totalNotesUp;
    public float totalNotesDown; 
    public float totalBadHits;
    public float totalGoodHits;
    public float totalPerfectHits;
    public float totalPercentHit;
    public float totalMissHits;
    public float totalHits;
    private float badweight = 0.5f;
    private float goodweight = 0.75f;
    private float perfectweight = 1.0f;
    public bool gameOver = false;
    public bool canContinue = false;
    public bool isContinuing = false;

    // Text Results
    public GameObject continueCanva;
    public GameObject transiction;
    public GameObject resultScreen;
    public Text missText, badsText, goodsText, perfectsText, percentsText, ranksText, finalScoresText;

    // UI

    public GameObject buttons;
    public GameObject UI;
    public GameObject Tutorial;
    public GameObject pauseMenu;
    public GameObject L2, L1, R1, R2;

    private void Awake()
    {
        GamepadInputComponent = FindObjectOfType<GamepadInput>();
    }
    void Start()
    {
        instance = this;
        playerHealth = maxPlayerHealth;
        scoreText.text = "Pontos: 0";
        multiplier = 1;
        healthBar.maxValue = maxPlayerHealth;
        healthBar.value = playerHealth;
        totalNotesLeft = FindObjectsOfType<NoteObjectLeft>().Length;
        totalNotesDown = FindObjectsOfType<NoteObjectDown>().Length;
        totalNotesUp = FindObjectsOfType<NoteObjectUp>().Length;
        totalNotesRight = FindObjectsOfType<NoteObjectRight>().Length;
        Time.timeScale = 0f;
        totalNotes = totalNotesDown + totalNotesUp + totalNotesRight + totalNotesLeft;
    }



    void Update()
    {


        if (!startplaying)
        {
            // START SCREEN

            // GamePad
            if (GamepadInputComponent.onButtonDown["ActionButton"])
            {
                Tutorial.SetActive(false);
                winSrc.Play();
                audSrc.Play();
                startplaying = true;
                noteScroll.hasStarted = true;
                Time.timeScale = 1f;
                L2.gameObject.SetActive(true);
                L1.gameObject.SetActive(true);
                R1.gameObject.SetActive(true);
                R2.gameObject.SetActive(true);
            }
            if (GamepadInputComponent.onButtonDown["BackButton"])
            {
                SceneManager.LoadScene(0);
            }

            // KeyBoard
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Tutorial.SetActive(false);
                winSrc.Play();
                audSrc.Play();
                startplaying = true;
                noteScroll.hasStarted = true;
                Time.timeScale = 1f;
                L2.gameObject.SetActive(true);
                L1.gameObject.SetActive(true);
                R1.gameObject.SetActive(true);
                R2.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }

            return;
        }
        else
        {
            if (!audSrc.isPlaying && !resultScreen.activeInHierarchy && menuPause == false && isContinuing == false)
            {
                winSrc.Play();
                transiction.SetActive(true);
                Invoke("ResultsAfterDelay", 1.3f);
                badsText.text = "" + totalBadHits;
                goodsText.text = totalGoodHits.ToString();
                perfectsText.text = totalPerfectHits.ToString();
                missText.text = "" + totalMissHits;

                totalHits = (totalBadHits * badweight) + (totalGoodHits * goodweight) + (totalPerfectHits * perfectweight);
                totalPercentHit = (totalHits / totalNotes) * 100;

                percentsText.text = totalPercentHit.ToString("F1") + "%";

                buttons.SetActive(false);
                UI.SetActive(false);
                StartCoroutine(CanPressAfterDelay());

                string rankVal = "F";


                if (score >= 1200)
                {
                    rankVal = "D";
                    ranksText.color = Color.blue;
                    if (score >= 5000)
                    {
                        rankVal = "C";
                        ranksText.color = Color.green;
                    }
                    if (score >= 8000)
                    {
                        rankVal = "B";
                        ranksText.color = Color.yellow;
                    }
                    if (score >= 15000)
                    {
                        rankVal = "A";
                        ranksText.color = Color.magenta;
                    }
                    if (score >= 20000)
                    {
                        rankVal = "S";
                        ranksText.color = Color.red;
                    }

                }
                ranksText.text = rankVal;
                finalScoresText.text = score.ToString();
            }

        }

            // PAUSE MENU


            // GamePad
            if (GamepadInputComponent.onButtonDown["Start"] && menuPause == false)
            {
            menuPause = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            audSrc.Pause();
            }
    
            if (GamepadInputComponent.onButtonDown["Start"] && menuPause == true)
            {
                menuPause = false;
               Time.timeScale = 1f;
               audSrc.Play();
               pauseMenu.SetActive(false);
            }


            // KeyBoard
            if (Input.GetKeyDown(KeyCode.Escape) && menuPause == false)
            {
        
            menuPause = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            audSrc.Pause();

            }
            else if (Input.GetKeyDown(KeyCode.Escape) && menuPause == true)
            {
            menuPause = false;
            Time.timeScale = 1f;
            audSrc.Play();
            pauseMenu.SetActive(false);
            }


        // AFTER RESULTS

        // GamePad
        if (GamepadInputComponent.onButtonDown["ActionButton"] && canContinue == true)
        {
            finSrc.Play();
            continueCanva.SetActive(true);
            resultScreen.SetActive(false);
            isContinuing = true;
            canContinue = false;
            StartCoroutine(ContinueAfterDelay());
        }
        if (finalContinue == true)
        {
            if (GamepadInputComponent.onButtonDown["ActionButton"])
            {
                SceneManager.LoadScene(3);
            }
            if (GamepadInputComponent.onButtonDown["BackButton"])
            {
                SceneManager.LoadScene(0);
            }
        }

        // KeyBoard
        if (canContinue == true && Input.GetKeyDown(KeyCode.Space))
           {
            finSrc.Play();
            continueCanva.SetActive(true);
            resultScreen.SetActive(false);
            isContinuing = true;
            canContinue = false;
            StartCoroutine(ContinueAfterDelay());
            
           }
           if (finalContinue == true)
           {
             if (Input.GetKeyDown(KeyCode.Space))
             {
 
                 SceneManager.LoadScene(3);
             }

             if (Input.GetKeyDown(KeyCode.R))
             {
                 SceneManager.LoadScene(0);
             }
           }
    


}




    public void NoteHit()
    {
        if (gameOver) return;
        if (multiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;


            if (multiplierThresholds[multiplier - 1] <= multiplierTracker)
            {
                Debug.Log("multiplied");
                multiplierTracker = 0;
                multiplier++;
            }
        }

        multiText.text = "Multiplier: x" + multiplier;
        scoreText.text = "Pontos: " + score;
    }
    public void BadHit()
    {
        if (gameOver) return;
        score += scorePerBadHit * multiplier;
        healthBar.value = playerHealth;
        Heal();
        NoteHit();
        totalBadHits++;

    }
    public void GoodHit()
    {
        if (gameOver) return;
        score += scorePerGoodHit * multiplier;
        Heal();
        healthBar.value = playerHealth;
        NoteHit();
        totalGoodHits++;
    }
    public void PerfectHit()
    {
        if (gameOver) return;
        score += scorePerPerfectHit * multiplier;
        NoteHit();
        Heal();
        healthBar.value = playerHealth;
        totalPerfectHits++;
    }
    public void NoteMiss()
    {

        if (gameOver) return;
        Debug.Log("Note Miss");
        multiplier = 1;
        multiplierTracker = 0;
        multiText.text = "Multiplier: x" + multiplier;
        score -= scorePerMissHit;
        scoreText.text = "Pontos: " + score;
        totalMissHits++;


    }
    public void Punishment()
    {
        if (gameOver) return;

        score -= scorePerMissHit;
        playerHealth -= damagePerMiss;
        healthBar.value = playerHealth;
        scoreText.text = "Pontos: " + score;
        missSrc.Play();


        if (playerHealth <= 0)
        {
            gameOver = true;
            Invoke("LoadSceneAfterDelay", 0.1f);
            return;
        }

        if (playerHealth > 100)
        {
            Debug.Log("Tem algo errado aí patrão :/");
        }
    }
    public void Heal()
    {
        playerHealth += healingPerHit;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxPlayerHealth);
        updateHealthBar();
    }
    private void updateHealthBar()
    {
        healthBar.value = playerHealth;
    }

    void LoadSceneAfterDelay()
    {
        SceneManager.LoadScene("Fail");
    }
    public void Resume()
    {
        menuPause = false;
        Time.timeScale = 1f;
        audSrc.Play();
        pauseMenu.SetActive(false);

    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    void ResultsAfterDelay()
    {
        resultScreen.SetActive(true);
    }
    IEnumerator CanPressAfterDelay()
    {
        yield return new WaitForSeconds(9);

        canContinue = true;

    }
    IEnumerator ContinueAfterDelay()
    {
        yield return new WaitForSeconds(1);
        finalContinue = true;
    }
}

