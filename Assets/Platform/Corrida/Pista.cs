using UnityEngine;

public class Pista : MonoBehaviour 
{
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float velocidadeFreio = 2f; 
    public float alturaSegmento = 20f;
    public float limiteInferiorY = -10f;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        float velocidadeAtual;

        
        if (UIManager.EstaFreando)
        {
            velocidadeAtual = velocidadeFreio;
        }
        else if (UIManager.EstaAcelerando)
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