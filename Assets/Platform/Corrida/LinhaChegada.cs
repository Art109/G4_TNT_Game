using UnityEngine;

public class LinhaChegada : MonoBehaviour
{
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float velocidadeFreio = 2f; 

    public float pontoDeVitoriaY = -5f; 

    
    public float pontoYParaPararRespawn = 0f;
    private bool respawnParado = false; 

    private PlayerCarro scriptJogador; 
    private bool jaCruzou = false;     

    void Start()
    {
        scriptJogador = FindObjectOfType<PlayerCarro>();
        if (scriptJogador == null) { /* Log Erro */ }
        jaCruzou = false;
        respawnParado = false;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        
        if (scriptJogador != null)
        {
            
            if (!jaCruzou)
            {
                MoverLinhaChegada();

                
                if (!respawnParado && transform.position.y < pontoYParaPararRespawn)
                {
                    PararRespawnsGlobal();
                }

                
                if (transform.position.y < pontoDeVitoriaY)
                {
                    DeclararVitoria();
                }
            }
        }
    }

    
    void MoverLinhaChegada()
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

    
    void VerificarCruzamento() 
    {
        
    }

    
    void PararRespawnsGlobal()
    {
        if (!respawnParado)
        {
            respawnParado = true;
            PlayerCarro.LinhaDeChegadaPassou = true; 
            Debug.LogWarning("!!! SETANDO PlayerCarro.LinhaDeChegadaPassou = TRUE em Y=" + transform.position.y + " !!!");
        }
    }

    
    void DeclararVitoria()
    {
        if (!jaCruzou)
        {
            jaCruzou = true;
            scriptJogador.IniciarVitoria();
        }
    }
}