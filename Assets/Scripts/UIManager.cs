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

    [Header("Tela de Instruções")]
    public GameObject painelInstrucoes;

    [Header("Valores Base para UI")]
    public float velocidadeNormalBase = 5f;
    public float velocidadeBoostBase = 10f;
    public float velocidadeFreioBase = 2f;

    public KeyCode teclaFreio = KeyCode.LeftControl;
    public KeyCode teclaBoost = KeyCode.LeftShift;
    public KeyCode teclaIniciarJogo = KeyCode.Space;

    private float tempoDecorrido = 0f;
    private bool cronometroAtivo = false;
    private bool instrucoesAtivas = false;

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
        
        bool fimDeJogoAtivo = (painelPontuacaoFinal != null && painelPontuacaoFinal.activeSelf) ||
                              (painelGameOver != null && painelGameOver.activeSelf);

        if (!fimDeJogoAtivo)
        {
            AtualizarVelocidadeUI();
            AtualizarTempoUI();
            AtualizarLatasUI();
        }
        else if (cronometroAtivo)
        {
            PararCronometro();
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
        
        minutos = Mathf.Max(0, minutos);
        segundos = Mathf.Max(0, segundos);
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
        
        PararCronometro();
        return tempoDecorrido;
    }

    public void MostrarPontuacaoFinal(int pontuacaoMapeada)
    {
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