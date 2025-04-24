using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
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
    public GameObject RedMachine;
    public GameObject BluMachine;
    public GameObject PrpMachine;
    public Renderer Preview1;
    public Renderer Preview2;
    public Transform Finger;
    public string Lata;
    public GameObject LogoTNT;
    private bool inButtonS;
    private bool inButtonE;
    public LayerMask StartButton;
    public GameObject Exit;
    public LayerMask EndButton;
    public bool start;
    public GameObject Machine;
    public float size;
    public GameObject Popup;
    public GameObject spin;
    private int LataID;

    void Start()
    {
        
        HandRB = GetComponent<Rigidbody2D>();
        LataID = 0;
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

        

        if (Lata == "")
        {

            Popup.transform.position = new Vector3(32, -9, 0);
        }
        else
        {
            if (Lata == "Energy")
            {
                Preview1.material.color = Color.red;
                Preview2.material.color = Color.red;
                Popup.transform.position = new Vector3(27, -4, 0);
            }
            if (Lata == "Nutrition")
            {
                Preview1.material.color = Color.cyan;
                Preview2.material.color = Color.cyan;
                Popup.transform.position = new Vector3(27, -4, 0);
            }
            if (Lata == "Focus")
            {
                Preview1.material.color = new Color(0.6745098f, 0.02352941f, 0.7686275f, 1f);
                Preview2.material.color = new Color(0.6745098f, 0.02352941f, 0.7686275f, 1f);
                Popup.transform.position = new Vector3(27, -4, 0);
            }
        }
        inButtonE = Physics2D.OverlapCircle(Finger.position, 0f, EndButton);

        if (inButtonE && Input.GetKeyUp(KeyCode.Space))
        {
            End();
        }

    }
    private void StartGame()
    {
        inButtonS = Physics2D.OverlapCircle(Finger.position, 0f, StartButton);
        

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

        inRed = Physics2D.OverlapCircle(Finger.position, 0.01f, RedTNT);
        inBlue = Physics2D.OverlapCircle(Finger.position, 0.01f, BluTNT);
        inPurple = Physics2D.OverlapCircle(Finger.position, 0.01f, PrpTNT);

        if (inRed)
        {
            Lata = "Energy";
            
            RedMachine.SetActive(true);
            if (inRed && Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Energy");
                spin.transform.eulerAngles = new Vector3(0, 0, 20);
                RedMachine.transform.localScale = new Vector3(3, 3, 1);
                LataID = 1;
                StartCoroutine(Wait());
            }
        }
        else if (inBlue)
        {
            Lata = "Nutrition";
            
            BluMachine.SetActive(true);
            if (inBlue && Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Nutrition");
                spin.transform.eulerAngles = new Vector3(0, 0, 20);
                BluMachine.transform.localScale = new Vector3(3, 3, 1);
                LataID = 2;
                StartCoroutine(Wait());
            }
        }
        else if (inPurple)
        {
            Lata = "Focus";
            
            PrpMachine.SetActive(true);
            if (inPurple && Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Focus");
                spin.transform.eulerAngles = new Vector3(0, 0, 20);
                PrpMachine.transform.localScale = new Vector3(3, 3, 1);
                LataID = 3;
                StartCoroutine(Wait());

            }
        }
        else
        {
            Lata = "";
            spin.transform.eulerAngles = new Vector3(0, 0, 1);
            RedMachine.SetActive(false);
            BluMachine.SetActive(false);
            PrpMachine.SetActive(false);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Finger.position, size);
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(LataID);
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


