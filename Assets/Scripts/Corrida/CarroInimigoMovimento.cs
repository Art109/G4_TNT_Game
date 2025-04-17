using UnityEngine;
using System.Collections.Generic;

public class CarroInimigoMovimento : MonoBehaviour
{
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float velocidadeFreio = 2f;

    public float limiteInferiorY = -10f;
    public float posicaoRespawnYBase = 10f;

    public List<float> posicoesXDasFaixas = new List<float>();

    public float espacamentoMinimoY = 4.0f;
    public string tagOutroInimigo = "Inimigo";

    private float raioVerificacaoY;

    void Awake()
    {
        raioVerificacaoY = espacamentoMinimoY * 1.1f;
    }

    void Update()
    {
        float velocidadeAtual;
        if (Input.GetKey(KeyCode.LeftControl)) { velocidadeAtual = velocidadeFreio; }
        else if (Input.GetKey(KeyCode.LeftShift)) { velocidadeAtual = velocidadeBoost; }
        else { velocidadeAtual = velocidadeNormal; }

        transform.Translate(Vector3.down * velocidadeAtual * Time.deltaTime, Space.World);

        if (transform.position.y < limiteInferiorY)
        {
            Respawn();
        }
    }

    
    void Respawn()
    {

        if (posicoesXDasFaixas == null || posicoesXDasFaixas.Count == 0)
        {
            Debug.LogError("Lista 'posicoesXDasFaixas' não configurada para " + gameObject.name);
            gameObject.SetActive(false);
            return;
        }

        int indiceAleatorio = Random.Range(0, posicoesXDasFaixas.Count);
        float novaPosicaoX = posicoesXDasFaixas[indiceAleatorio];
        float tentativaNovaPosicaoY = posicaoRespawnYBase;

        
        Collider2D[] colisoresProximos = Physics2D.OverlapCircleAll(
                                            new Vector2(novaPosicaoX, tentativaNovaPosicaoY),
                                            raioVerificacaoY,
                                            LayerMask.GetMask("Default")
                                        );

        float yMaisAltoOcupado = -Mathf.Infinity;
        foreach (Collider2D col in colisoresProximos)
        {
            
            if (col.gameObject != this.gameObject && col.CompareTag(tagOutroInimigo))
            {
                yMaisAltoOcupado = Mathf.Max(yMaisAltoOcupado, col.transform.position.y);
            }
        }

        if (yMaisAltoOcupado > -Mathf.Infinity)
        {
            tentativaNovaPosicaoY = yMaisAltoOcupado + espacamentoMinimoY;
        }

        float novaPosicaoY = Mathf.Max(posicaoRespawnYBase, tentativaNovaPosicaoY);
        transform.position = new Vector3(novaPosicaoX, novaPosicaoY, transform.position.z);

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
}