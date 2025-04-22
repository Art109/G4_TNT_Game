using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speen : MonoBehaviour
{
    public GameObject angle;
    public float speed = 5f;
    private float time;
    private Vector3 startingAngle;

    public Rigidbody2D rb;
    public List<Transform> waypoints;
    public float velocidade = 5f;
    int proximoPonto = 0;

    // Start is called before the first frame update
    void Start()
    {
        startingAngle = angle.transform.eulerAngles;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += angle.transform.eulerAngles * speed * Time.deltaTime;

        time += Time.deltaTime;

        if (angle.transform.eulerAngles != startingAngle)
        {
            Vector2 direcao = (waypoints[proximoPonto].position - transform.position);
            rb.velocity = direcao * velocidade;
        }
    }
}
