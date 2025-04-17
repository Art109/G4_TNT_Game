using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Components
    public AudioSource audSrc;
    public AudioSource missSrc;
    public AudioSource winSrc;
    public bool startplaying;
    public NoteScroll noteScroll;
    public static GameManager instance;

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

    // Results

    public float totalNotes;
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

    // Text Results

    public GameObject resultScreen;
    public Text missText, badsText, goodsText, perfectsText, percentsText, ranksText, finalScoresText;

    // UI

    public GameObject buttons;
    public GameObject UI;
    public GameObject Tutorial;
   

    void Start()
    {
        instance = this;
        playerHealth = maxPlayerHealth;
        scoreText.text = "Score: 0";
        multiplier = 1;
        healthBar.maxValue = maxPlayerHealth;
        healthBar.value = playerHealth;
        totalNotes = FindObjectsOfType<NoteObject>().Length;
        Time.timeScale = 0f;

    }


    void Update()
    {
        

        if (!startplaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Tutorial.SetActive(false);
                winSrc.Play();
                startplaying = true;
                noteScroll.hasStarted = true;
                audSrc.Play();
                Time.timeScale = 1f;
            }
            return;
        }
        else
        {
            if (!audSrc.isPlaying && !resultScreen.activeInHierarchy)
            {
                resultScreen.SetActive(true);
                winSrc.Play();
                badsText.text = "" + totalBadHits;
                goodsText.text = totalGoodHits.ToString();
                perfectsText.text = totalPerfectHits.ToString();
                missText.text = "" + totalMissHits;

                totalHits = (totalBadHits * badweight) + (totalGoodHits * goodweight) + (totalPerfectHits * perfectweight);
                totalPercentHit = (totalHits / totalNotes) * 100;

                percentsText.text = totalPercentHit.ToString("F1") + "%";

                buttons.SetActive(false);
                UI.SetActive(false);

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
    }


    public void NoteHit()
    {
        if(gameOver) return;
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


        scoreText.text = "Score: " + score;
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
        scoreText.text = "Score: " + score;
        totalMissHits++;

    }
    public void Punishment()
    {
        if (gameOver) return;

        score -= scorePerMissHit;
        playerHealth -= damagePerMiss;
        healthBar.value = playerHealth;
        scoreText.text = "Score: " + score;
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
        SceneManager.LoadScene(1);
    }
}

