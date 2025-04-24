using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    
    public TextMeshProUGUI textoGameOverMensagem;
    

    [Header("Tela de Instruções")]
    public GameObject painelInstrucoes;

    public KeyCode teclaFreio = KeyCode.LeftControl;
    public KeyCode teclaBoost = KeyCode.LeftShift;
    public KeyCode teclaIniciarJogo = KeyCode.Space;
    public KeyCode teclaVoltarMenu = KeyCode.Return;
    public KeyCode teclaPause = KeyCode.Escape;

    [Header("Painel de Pause")]
    public GameObject painelPause;

    private float tempoDecorrido = 0f;
    private bool cronometroAtivo = false;
    private bool instrucoesAtivas = false;
    private bool jogoPausado = false;

    private SceneLoader sceneLoader;


    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader == null) { /* Log Warning */ }

        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false);
        if (painelGameOver != null) painelGameOver.SetActive(false);
        if (painelPause != null) painelPause.SetActive(false);

        if (painelInstrucoes != null && painelInstrucoes.activeSelf)
        {
            instrucoesAtivas = true;
            Time.timeScale = 0f;
            EsconderUIDaCorrida();
        }
        else
        {
            instrucoesAtivas = false;
            Time.timeScale = 1f;
            MostrarUIDaCorrida();
            IniciarCronometro();
        }
        jogoPausado = false;
    }

    void Update()
    {

        if (!instrucoesAtivas)
        {
            VerificarInputPause();
        }
        
        if (jogoPausado)
        {
            return;
        }

        if (instrucoesAtivas)
        {
            if (Input.GetKeyDown(teclaIniciarJogo)) { FecharInstrucoesEIniciarJogo(); }
            return;
        }

        
        if (painelPontuacaoFinal != null && painelPontuacaoFinal.activeSelf)
        {
            if (Input.GetKeyDown(teclaVoltarMenu))
            {
                Debug.Log("Tecla Voltar Menu pressionada na tela de vitória.");
                if (sceneLoader != null) { sceneLoader.CarregarCenaPorIndice(0); }
                else { Debug.LogError("SceneLoader não encontrado!"); }
            }
            
            return;
        }

        
        if (painelGameOver != null && painelGameOver.activeSelf)
        {
            return;
        }

        AtualizarVelocidadeUI();
        AtualizarTempoUI();
        AtualizarLatasUI();

    }

    void VerificarInputPause()
    {
        bool fimDeJogoAtivo = (painelPontuacaoFinal != null && painelPontuacaoFinal.activeSelf) ||
                              (painelGameOver != null && painelGameOver.activeSelf);
        if (fimDeJogoAtivo) return;

        if (Input.GetKeyDown(teclaPause))
        {
            if (jogoPausado) { DespausarJogo(); }
            else { PausarJogo(); }
        }
    }

    void PausarJogo()
    {
        jogoPausado = true;
        Time.timeScale = 0f;
        if (painelPause != null) painelPause.SetActive(true);
        Debug.Log("Jogo Pausado");
    }

    void DespausarJogo()
    {
        jogoPausado = false;
        Time.timeScale = 1f;
        if (painelPause != null) painelPause.SetActive(false);
        Debug.Log("Jogo Despausado");
    }


    void FecharInstrucoesEIniciarJogo()
    {
        if (painelInstrucoes != null) { painelInstrucoes.SetActive(false); }
        instrucoesAtivas = false;
        Time.timeScale = 1f;
        MostrarUIDaCorrida();
        IniciarCronometro();
    }


    void AtualizarVelocidadeUI()
    {
        if (textoVelocidade == null) return;
        float velocidadeAtualExibida = 0f;
        Color corAtual = corVelocidadeNormal;

        if (Input.GetKey(teclaFreio))
        {
            velocidadeAtualExibida = 20;
            corAtual = corVelocidadeFreio;
        }
        
        else if (Input.GetKey(teclaBoost))
        {
            velocidadeAtualExibida = 120;
            corAtual = corVelocidadeBoost;
        }
        else
        {
            velocidadeAtualExibida = 80;
            corAtual = corVelocidadeNormal;
        }
        textoVelocidade.text = $"Velocidade: {velocidadeAtualExibida.ToString("F0")} km/h";
        textoVelocidade.color = corAtual;
    }

    void AtualizarTempoUI()
    {
        if (textoTempo == null) return;
        if (cronometroAtivo) { tempoDecorrido += Time.deltaTime; }
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
        PararCronometro();
        EsconderUIDaCorrida();
        if (painelGameOver != null) painelGameOver.SetActive(false);
        if (painelPontuacaoFinal != null && textoPontuacaoFinal != null)
        {
            textoPontuacaoFinal.text = $"VOCÊ VENCEU!\nPontuação: {pontuacaoMapeada} / 20";
            painelPontuacaoFinal.SetActive(true);
        }
    }

    
    public void MostrarGameOver(int pontuacaoFinal)
    {
        PararCronometro();
        EsconderUIDaCorrida();
        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false);

        if (painelGameOver != null)
        {
            if (textoGameOverMensagem != null)
            {
                textoGameOverMensagem.text = "VOCÊ BATEU!\nTENTE NOVAMENTE.\nPontuação: " + pontuacaoFinal;
            }
            else
            {
                Debug.LogError("Referência textoGameOverMensagem não definida no UIManager!");
                
                var tmp = painelGameOver.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null) tmp.text = "VOCÊ BATEU!";
            }
            
            painelGameOver.SetActive(true);
        }
        else
        {
            Debug.LogError("Referência painelGameOver não definida no UIManager!");
        }
    }
    

    private void EsconderUIDaCorrida()
    {
        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(false);
        if (textoTempo != null) textoTempo.gameObject.SetActive(false);
        if (textoLatas != null) textoLatas.gameObject.SetActive(false);
    }

    private void MostrarUIDaCorrida()
    {
        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(true);
        if (textoTempo != null) textoTempo.gameObject.SetActive(true);
        if (textoLatas != null) textoLatas.gameObject.SetActive(true);
    }
}