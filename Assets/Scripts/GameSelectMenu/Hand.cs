using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public string Lata;
    public RawImage LogoTNT;
    void Start()
    {
        HandRB = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        GameSelect();
        float eixoX = Input.GetAxisRaw("Horizontal") * HandVelocity;
        float eixoY = Input.GetAxisRaw("Vertical") * HandVelocity;

        HandRB.velocity = new Vector2(eixoX, eixoY);
        if (Lata != "")
        {
            LogoTNT.transform.position = new Vector3(840f, 260f);
        }
        else
        {
            LogoTNT.transform.position = new Vector3(-160f, -40f);
        }
    }
    public void GameSelect()
    {
        
        inRed = Physics2D.OverlapCircle(Finger.position, 0.2f, RedTNT);
        inBlue = Physics2D.OverlapCircle(Finger.position, 0.2f, BluTNT);
        inPurple = Physics2D.OverlapCircle(Finger.position, 0.2f, PrpTNT);
        
        if (inRed)
        {
            Lata = "Energy";

            if (inRed && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Energy");
                //SceneManager.LoadScene(1);
            }
        }
        else if (inBlue)
        {
            Lata = "Nutrition";

            if (inBlue && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Nutrition");
                //SceneManager.LoadScene(2);
            }
        }
        else if (inPurple)
        {
            Lata = "Focus";

            if (inPurple && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Focus");
                //SceneManager.LoadScene(3);
            }
        }
        else
        {
            Lata = "";
        }
    }
}


