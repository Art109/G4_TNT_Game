using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Elementos de UI Durante a Corrida")]
    public TextMeshProUGUI textoVelocidade;
    public TextMeshProUGUI textoTempo;
    public TextMeshProUGUI textoLatas;

    [Header("Cores Indicadoras de Velocidade")]
    public Color corVelocidadeNormal = Color.yellow;
    public Color corVelocidadeBoost = Color.blue;
    public Color corVelocidadeFreio = Color.red;

    [Header("Elementos de UI Fim de Corrida")]
    public GameObject painelPontuacaoFinal;
    public TextMeshProUGUI textoPontuacaoFinal;
    public GameObject painelGameOver;


    private float tempoDecorrido = 0f;
    private bool cronometroAtivo = false;

    public float velocidadeNormalBase = 5f;
    public float velocidadeBoostBase = 10f;
    public float velocidadeFreioBase = 2f;

    void Start()
    {
        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false);
        if (painelGameOver != null) painelGameOver.SetActive(false);

        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(true);
        if (textoTempo != null) textoTempo.gameObject.SetActive(true);
        if (textoLatas != null) textoLatas.gameObject.SetActive(true);

        IniciarCronometro();
    }

    void Update()
    {
        if ((painelPontuacaoFinal == null || !painelPontuacaoFinal.activeSelf) &&
            (painelGameOver == null || !painelGameOver.activeSelf))
        {
            AtualizarVelocidadeUI();
            AtualizarTempoUI();
            AtualizarLatasUI();
        }
    }

    void AtualizarVelocidadeUI()
    {
        if (textoVelocidade == null) return;

        float velocidadeAtualExibida;

        Color corAtual = corVelocidadeNormal;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            velocidadeAtualExibida = velocidadeFreioBase;
            corAtual = corVelocidadeFreio;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            velocidadeAtualExibida = velocidadeBoostBase;
            corAtual = corVelocidadeBoost;
        }
        else
        {
            velocidadeAtualExibida = velocidadeNormalBase;
            corAtual = corVelocidadeNormal;
        }

        textoVelocidade.text = $"Velocidade: {velocidadeAtualExibida.ToString("F0")} km/h";
        textoVelocidade.color = corAtual;
    }

    void AtualizarTempoUI()
    {
        if (textoTempo == null) return;

        if (cronometroAtivo)
        {
            tempoDecorrido += Time.deltaTime;
        }

        int minutos = Mathf.FloorToInt(tempoDecorrido / 60F);
        int segundos = Mathf.FloorToInt(tempoDecorrido - minutos * 60);
        textoTempo.text = $"Tempo: {minutos:00}:{segundos:00}";
    }

    void AtualizarLatasUI()
    {
        if (textoLatas == null) return;

        textoLatas.text = $"Latas: {PlayerCarro.latasColetadas}";
    }

    public void IniciarCronometro()
    {
        tempoDecorrido = 0f;
        cronometroAtivo = true;
    }

    public void PararCronometro()
    {
        cronometroAtivo = false;
    }

    public float GetTempoFinal()
    {
        return tempoDecorrido;
    }

    public void MostrarPontuacaoFinal(int pontuacaoMapeada)
    {
        PararCronometro();
        EsconderUIDaCorrida();

        if (painelGameOver != null) painelGameOver.SetActive(false);

        if (painelPontuacaoFinal != null && textoPontuacaoFinal != null)
        {
            textoPontuacaoFinal.text = $"VOCÊ VENCEU!\nPontuação: {pontuacaoMapeada} / 20";
            painelPontuacaoFinal.SetActive(true);
        }
    }

    public void MostrarGameOver()
    {
        PararCronometro();
        EsconderUIDaCorrida();

        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false);

        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
        }
    }

    private void EsconderUIDaCorrida()
    {
        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(false);
        if (textoTempo != null) textoTempo.gameObject.SetActive(false);
        if (textoLatas != null) textoLatas.gameObject.SetActive(false);
    }
}
