using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pista : MonoBehaviour
{
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float alturaSegmento;
    public float limiteInferiorY;

    void Start()
    {
        
    }
    
    void Update()
    {

        float velocidadeAtual;

        if (Input.GetKey(KeyCode.Space))
        {
            velocidadeAtual = velocidadeBoost;
        }
        else
        {
            velocidadeAtual = velocidadeNormal;
        }

        transform.Translate(Vector3.down * velocidadeAtual * Time.deltaTime, Space.World);        

        if (transform.position.y < limiteInferiorY)
        {
            Reposicionar();
        }
    }

    void Reposicionar()
    {
        float distanciaSalto = alturaSegmento * 2f;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + distanciaSalto,
            transform.position.z
        );
    }
}
