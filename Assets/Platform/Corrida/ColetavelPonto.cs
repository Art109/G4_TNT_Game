using UnityEngine;
using System.Collections.Generic;

public class ColetavelPonto : MonoBehaviour
{
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float velocidadeFreio = 2f;

    public float limiteInferiorY = -10f;
    public float posicaoRespawnYBase = 10f;

    public List<float> posicoesXDasFaixasLatas = new List<float>();

    public float espacamentoMinimoYLatas = 1.0f;
    public string tagOutraLata = "Coletavel";
    public LayerMask layerLatas; 

    private float raioVerificacaoYLatas;

    public int pontosPorColeta = 10;
    public AudioSource somColeta;

    private SpriteRenderer spriteRenderer;
    private Collider2D colisor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colisor = GetComponent<Collider2D>();
        raioVerificacaoYLatas = espacamentoMinimoYLatas * 1.1f;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        
        if (PlayerCarro.LinhaDeChegadaPassou)
        {
            if (transform.position.y > limiteInferiorY) { MoverColetavel(); }
            else { if (gameObject.activeSelf) gameObject.SetActive(false); }
            return;
        }

        
        MoverColetavel();
        VerificarSaidaDaTela();
    }

    
    void MoverColetavel()
    {
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
    }

    
    void VerificarSaidaDaTela()
    {
        if (transform.position.y < limiteInferiorY)
        {
            Respawn();
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.timeScale != 0f && other.CompareTag("Player"))
        {
            Coletar();
        }
    }

    
    void Coletar()
    {
        PlayerCarro.pontuacaoAtual += pontosPorColeta;
        PlayerCarro.latasColetadas++; 
        if (somColeta != null && somColeta.clip != null) { AudioSource.PlayClipAtPoint(somColeta.clip, transform.position, somColeta.volume > 0 ? somColeta.volume : 1f); }
        gameObject.SetActive(false);
    }

    
    void Respawn()
    {
        if (PlayerCarro.LinhaDeChegadaPassou) 
        {
            gameObject.SetActive(false);
            return;
        }

        if (posicoesXDasFaixasLatas == null || posicoesXDasFaixasLatas.Count == 0) { /*...*/ gameObject.SetActive(false); return; }
        int indiceAleatorio = Random.Range(0, posicoesXDasFaixasLatas.Count);
        float novaPosicaoX = posicoesXDasFaixasLatas[indiceAleatorio];
        float tentativaNovaPosicaoY = posicaoRespawnYBase;
        Collider2D[] colisoresProximos = Physics2D.OverlapCircleAll(
                                            new Vector2(novaPosicaoX, tentativaNovaPosicaoY),
                                            raioVerificacaoYLatas,
                                            layerLatas);
        float yMaisAltoOcupado = -Mathf.Infinity;
        foreach (Collider2D col in colisoresProximos) { /*...*/ }
        if (yMaisAltoOcupado > -Mathf.Infinity) { /*...*/ }
        float novaPosicaoY = Mathf.Max(posicaoRespawnYBase, tentativaNovaPosicaoY);
        transform.position = new Vector3(novaPosicaoX, novaPosicaoY, transform.position.z);
        spriteRenderer.enabled = true;
        colisor.enabled = true;
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }
    }
}