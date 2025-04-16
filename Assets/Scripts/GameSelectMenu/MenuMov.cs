using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMov : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public List<Transform> waypoints;
    public float velocidade = 5f;
    int proximoPonto = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Count > 0)
        {
            Vector2 direcao = (waypoints[proximoPonto].position - transform.position);
            rb.velocity = direcao * velocidade;
                       
        }
    }
}
