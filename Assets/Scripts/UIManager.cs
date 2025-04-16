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

    [Header("Elementos de UI Fim de Corrida")]
    public GameObject painelPontuacaoFinal;
    public TextMeshProUGUI textoPontuacaoFinal;

    
    private float tempoDecorrido = 0f;
    private bool cronometroAtivo = false;

    public float velocidadeNormalBase = 5f;
    public float velocidadeBoostBase = 10f;

    void Start()
    {
        if (painelPontuacaoFinal != null)
        {
            painelPontuacaoFinal.SetActive(false);
        }
        
        IniciarCronometro();
    }

    void Update()
    {
        AtualizarVelocidadeUI();
        AtualizarTempoUI();
        AtualizarLatasUI();
    }

    void AtualizarVelocidadeUI()
    {
        if (textoVelocidade == null) return;

        float velocidadeAtualExibida;
        
        if (Input.GetKey(KeyCode.Space))
        {
            velocidadeAtualExibida = velocidadeBoostBase;
        }
        else
        {
            velocidadeAtualExibida = velocidadeNormalBase;
        }

        textoVelocidade.text = $"Velocidade: {velocidadeAtualExibida.ToString("F0")} km/h";
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

    public void MostrarPontuacaoFinal(int pontuacao)
    {
        PararCronometro();

        if (painelPontuacaoFinal != null && textoPontuacaoFinal != null)
        {
            textoPontuacaoFinal.text = $"Pontuação Final:\n{pontuacao}";
            painelPontuacaoFinal.SetActive(true);
        }
        
        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(false);
        if (textoTempo != null) textoTempo.gameObject.SetActive(false);
        if (textoLatas != null) textoLatas.gameObject.SetActive(false);
    }
}
