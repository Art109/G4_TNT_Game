using UnityEngine;

public class CarroInimigo : MonoBehaviour
{
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float limiteInferiorY = -10f;
    public float posicaoRespawnY = 10f;

    
    public float limiteEsquerdoX = -3.5f;
    public float limiteDireitoX = 3.5f;

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
            Respawn();
        }
    }

    void Respawn()
    {
        
        float novaPosicaoX = Random.Range(limiteEsquerdoX, limiteDireitoX);

        
        transform.position = new Vector3(novaPosicaoX, posicaoRespawnY, transform.position.z);
    }
}