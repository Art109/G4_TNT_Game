using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float HandVelocity = 5f;
    private Rigidbody2D HandRB;
    private bool inPurple;
    private bool inRed;
    private bool inBlue;
    public Transform RedTNT;
    public Transform BluTNT;
    public Transform PrpTNT;
    public LayerMask Energy;
    public LayerMask Nutrition;
    public LayerMask Focus;

    void Start()
    {
        HandRB = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        float eixoX = Input.GetAxisRaw("Horizontal") * HandVelocity;
        float eixoY = Input.GetAxisRaw("Vertical") * HandVelocity;

        HandRB.velocity = new Vector2(eixoX, eixoY);
    }
    void StageSelect()
    {
        inRed = Physics2D.OverlapCircle(RedTNT.position, 0.2f, Energy);
        inBlue = Physics2D.OverlapCircle(BluTNT.position, 0.2f, Energy);
        inPurple = Physics2D.OverlapCircle(PrpTNT.position, 0.2f, Energy);
    }
}


