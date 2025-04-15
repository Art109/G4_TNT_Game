using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Hand : MonoBehaviour
{
    
    public float HandVelocity = 5f;
    private Rigidbody2D HandRB;
    private bool inPurple;
    private bool inRed;
    private bool inBlue;
    public LayerMask RedTNT;
    public LayerMask BluTNT;
    public LayerMask PrpTNT;
    public Transform Finger;
    private string Lata;
    public RawImage LogoTNT;
    private bool inButtonS;
    private bool inButtonE;
    public LayerMask StartButton;
    public GameObject Exit;
    public LayerMask EndButton;
    public bool start;
    public GameObject Machine;
    void Start()
    {
        
        HandRB = GetComponent<Rigidbody2D>();

        
    }


    void Update()
    {
        GameSelect();
        
        StartGame();

        
        float eixoX = Input.GetAxisRaw("Horizontal") * HandVelocity;
        float eixoY = Input.GetAxisRaw("Vertical") * HandVelocity;
        //Vector2 pos;
        //pos = Input.mousePosition;
        //transform.position = Camera.main.ScreenToWorldPoint(pos);
        HandRB.velocity = new Vector2(eixoX, eixoY);

        

        if (Lata != "")
        {
            LogoTNT.transform.position = new Vector3(840f, 260f);
        }
        else
        {
            LogoTNT.transform.position = new Vector3(-160f, -40f);
        }
        inButtonE = Physics2D.OverlapCircle(Finger.position, 0.2f, EndButton);

        if (inButtonE && Input.GetKeyUp(KeyCode.Space))
        {
            End();
        }

    }
    private void StartGame()
    {
        inButtonS = Physics2D.OverlapCircle(Finger.position, 0.2f, StartButton);
        

        if (inButtonS && Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Start");
            start = true;
            Exit.SetActive(false); 

            Debug.Log("Camera Move");
            Machine.transform.position = new Vector3(25, 0,-10);


        }
        if (start == true && Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Return");
            start = false;
            Exit.SetActive(true);

            Machine.transform.position = new Vector3(0, 0, -10);
        }

    }
    private void GameSelect()
    {

        inRed = Physics2D.OverlapCircle(Finger.position, 0.2f, RedTNT);
        inBlue = Physics2D.OverlapCircle(Finger.position, 0.2f, BluTNT);
        inPurple = Physics2D.OverlapCircle(Finger.position, 0.2f, PrpTNT);

        if (inRed)
        {
            Lata = "Energy";

            if (inRed && Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Energy");
                SceneManager.LoadScene(1);
            }
        }
        else if (inBlue)
        {
            Lata = "Nutrition";

            if (inBlue && Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Nutrition");
                SceneManager.LoadScene(2);
            }
        }
        else if (inPurple)
        {
            Lata = "Focus";

            if (inPurple && Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Focus");
                SceneManager.LoadScene(3);
            }
        }
        else
        {
            Lata = "";
        }

    }
    private void End()
    {

            Debug.Log("Exit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Aplication.OpenURL("https://tntenergydrink.com.br/");
#else
            Application.Quit();
#endif
        


    }
}


