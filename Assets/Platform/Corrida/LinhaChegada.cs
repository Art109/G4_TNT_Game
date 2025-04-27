using UnityEngine;
using UnityEngine.InputSystem;

public class LinhaChegada : MonoBehaviour
{
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float velocidadeFreio = 2f;

    public float pontoDeVitoriaY = -5f;

    private PlayerCarro scriptJogador;

    private bool jaCruzou = false;

    private PlayerInput carInput;


    void Start()
    {
        carInput = GetComponent<PlayerInput>();
        scriptJogador = FindObjectOfType<PlayerCarro>();
        if (scriptJogador == null)
        {
            Debug.LogError("ERRO: Script PlayerCarro não encontrado na cena!");
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (!jaCruzou && scriptJogador != null)
        {
            MoverLinhaChegada();
            VerificarCruzamento();
        }
    }

    void MoverLinhaChegada()
    {
        float velocidadeAtual;

        if (carInput.actions["Freiar"].IsPressed())
        {
            velocidadeAtual = velocidadeFreio;
        }
        else if (carInput.actions["Acelerar"].IsPressed())
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
        if (transform.position.y < pontoDeVitoriaY)
        {
            jaCruzou = true;
            scriptJogador.IniciarVitoria();
        }
    }
}