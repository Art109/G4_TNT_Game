using UnityEngine;

public class LinhaChegada : MonoBehaviour
{
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float velocidadeFreio = 2f;

    public float pontoDeVitoriaY = -5f;

    private PlayerCarro scriptJogador;

    private bool jaCruzou = false;

    void Start()
    {
        scriptJogador = FindObjectOfType<PlayerCarro>();
        if (scriptJogador == null)
        {
            Debug.LogError("ERRO: Script PlayerCarro não encontrado na cena!");
        }
    }

    void Update()
    {
        if (!jaCruzou && scriptJogador != null)
        {
            MoverLinhaChegada();
            VerificarCruzamento();
        }
    }

    void MoverLinhaChegada()
    {
        float velocidadeAtual;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            velocidadeAtual = velocidadeFreio;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
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