using UnityEngine;
using TMPro; // Usando TextMeshPro como no seu código original
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
    public GameObject painelPontuacaoFinal; // Painel de Vitória
    public TextMeshProUGUI textoPontuacaoFinal; // Texto de Vitória
    public GameObject painelGameOver;           // Painel de Game Over
    // --- Adição: Referência ao TEXTO dentro do painel de Game Over ---
    public TextMeshProUGUI textoGameOverMensagem; // Conecte o texto de "VOCÊ BATEU..." aqui
    // ----------------------------------------------------------------

    [Header("Tela de Instruções")]
    public GameObject painelInstrucoes;

    // Removido: Valores Base para UI (Parece que a velocidade é lida de Input agora)
    // public float velocidadeNormalBase = 5f;
    // public float velocidadeBoostBase = 10f;
    // public float velocidadeFreioBase = 2f;

    // Mantido KeyCodes para referência
    public KeyCode teclaFreio = KeyCode.LeftControl;
    public KeyCode teclaBoost = KeyCode.LeftShift; // Shift Esquerdo para Boost? Verifique PlayerCarro se usa Space
    public KeyCode teclaIniciarJogo = KeyCode.Space;
    public KeyCode teclaVoltarMenu = KeyCode.Return; // Enter para voltar ao menu

    private float tempoDecorrido = 0f;
    private bool cronometroAtivo = false;
    private bool instrucoesAtivas = false;

    private SceneLoader sceneLoader;


    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader == null) { /* Log Warning */ }

        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false);
        if (painelGameOver != null) painelGameOver.SetActive(false); // Garante que Game Over começa escondido

        if (painelInstrucoes != null && painelInstrucoes.activeSelf) // Lógica de Instruções OK
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
    }

    void Update()
    {
        if (instrucoesAtivas)
        {
            if (Input.GetKeyDown(teclaIniciarJogo)) { FecharInstrucoesEIniciarJogo(); }
            return;
        }

        // Verifica se está na tela de vitória para voltar ao menu
        if (painelPontuacaoFinal != null && painelPontuacaoFinal.activeSelf)
        {
            if (Input.GetKeyDown(teclaVoltarMenu))
            {
                Debug.Log("Tecla Voltar Menu pressionada na tela de vitória.");
                if (sceneLoader != null) { sceneLoader.CarregarCenaPorIndice(0); }
                else { Debug.LogError("SceneLoader não encontrado!"); }
            }
            // --- Adição: Impede que a lógica da corrida continue na tela de vitória ---
            return; // Sai do Update se a tela de vitória está ativa
                    // ----------------------------------------------------------------------
        }

        // Verifica se está na tela de Game Over (O PlayerCarro já cuida do reinicio com delay)
        if (painelGameOver != null && painelGameOver.activeSelf)
        {
            // Não precisa de input aqui, PlayerCarro já iniciou a coroutine de reinício
            // Apenas impede que a lógica da corrida continue
            return; // Sai do Update se a tela de Game Over está ativa
        }


        // --- Lógica Durante a Corrida ---
        // (Só executa se não estiver nas instruções, nem em vitória, nem em game over)
        AtualizarVelocidadeUI(); // Atualiza baseado em Input, OK
        AtualizarTempoUI();      // Atualiza o cronômetro
        AtualizarLatasUI();      // Atualiza contador de latas
        // -----------------------------

        // Parar cronômetro não é mais necessário aqui, GetTempoFinal já faz isso
        // if (cronometroAtivo && fimDeJogoAtivo) { PararCronometro(); }
    }


    void FecharInstrucoesEIniciarJogo() // OK
    {
        if (painelInstrucoes != null) { painelInstrucoes.SetActive(false); }
        instrucoesAtivas = false;
        Time.timeScale = 1f;
        MostrarUIDaCorrida();
        IniciarCronometro();
    }


    void AtualizarVelocidadeUI() // Lógica OK (usa Input, não valores base)
    {
        if (textoVelocidade == null) return;
        float velocidadeAtualExibida = 0f; // Valor padrão
        Color corAtual = corVelocidadeNormal;

        // Nota: PlayerCarro usa Space para boost? Este UIManager usa Shift. Manter consistente.
        if (Input.GetKey(teclaFreio)) // Usando teclaFreio definida
        {
            // Simula uma velocidade de freio visualmente
            // Seria melhor obter a velocidade REAL do PlayerCarro se possível
            velocidadeAtualExibida = 20; // Valor de exemplo para freio
            corAtual = corVelocidadeFreio;
        }
        // Usando teclaBoost definida
        else if (Input.GetKey(teclaBoost)) // <<<<<<< VERIFICAR SE PLAYER USA SHIFT OU SPACE
        {
            // Simula velocidade de boost visualmente
            velocidadeAtualExibida = 120; // Valor de exemplo para boost
            corAtual = corVelocidadeBoost;
        }
        else
        {
            // Simula velocidade normal visualmente
            velocidadeAtualExibida = 80; // Valor de exemplo para normal
            corAtual = corVelocidadeNormal;
        }
        textoVelocidade.text = $"Velocidade: {velocidadeAtualExibida.ToString("F0")} km/h";
        textoVelocidade.color = corAtual;
    }

    void AtualizarTempoUI() // OK
    {
        if (textoTempo == null) return;
        if (cronometroAtivo) { tempoDecorrido += Time.deltaTime; }
        int minutos = Mathf.FloorToInt(tempoDecorrido / 60F);
        int segundos = Mathf.FloorToInt(tempoDecorrido - minutos * 60);
        minutos = Mathf.Max(0, minutos);
        segundos = Mathf.Max(0, segundos);
        textoTempo.text = $"Tempo: {minutos:00}:{segundos:00}";
    }

    void AtualizarLatasUI() // OK (usa variável estática)
    {
        if (textoLatas == null) return;
        textoLatas.text = $"Latas: {PlayerCarro.latasColetadas}";
    }


    public void IniciarCronometro() // OK
    {
        tempoDecorrido = 0f;
        cronometroAtivo = true;
    }

    public void PararCronometro() // OK
    {
        cronometroAtivo = false;
    }

    public float GetTempoFinal() // OK
    {
        PararCronometro();
        return tempoDecorrido;
    }

    // MostrarPontuacaoFinal (Vitória) - OK
    public void MostrarPontuacaoFinal(int pontuacaoMapeada)
    {
        PararCronometro(); // Garante que parou
        EsconderUIDaCorrida();
        if (painelGameOver != null) painelGameOver.SetActive(false); // Esconde Game Over se estiver ativo
        if (painelPontuacaoFinal != null && textoPontuacaoFinal != null)
        {
            textoPontuacaoFinal.text = $"VOCÊ VENCEU!\nPontuação: {pontuacaoMapeada} / 20";
            painelPontuacaoFinal.SetActive(true);
        }
    }

    // --- Função MostrarGameOver MODIFICADA ---
    // Agora aceita a pontuação como parâmetro
    public void MostrarGameOver(int pontuacaoFinal)
    {
        PararCronometro(); // Garante que parou
        EsconderUIDaCorrida();
        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false); // Esconde Vitória se estiver ativa

        if (painelGameOver != null)
        {
            // Verifica se a referência ao texto da mensagem existe
            if (textoGameOverMensagem != null)
            {
                // Define o texto incluindo a pontuação
                textoGameOverMensagem.text = "VOCÊ BATEU!\nTENTE NOVAMENTE.\nPontuação: " + pontuacaoFinal;
            }
            else
            {
                Debug.LogError("Referência textoGameOverMensagem não definida no UIManager!");
                // Opcional: Mostrar uma mensagem padrão se o texto não foi ligado
                // Tente pegar o componente TextMeshProUGUI no painel se possível
                var tmp = painelGameOver.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null) tmp.text = "VOCÊ BATEU!";
            }
            // Ativa o painel de Game Over
            painelGameOver.SetActive(true);
        }
        else
        {
            Debug.LogError("Referência painelGameOver não definida no UIManager!");
        }
    }
    // --- Fim da Função Modificada ---

    private void EsconderUIDaCorrida() // OK
    {
        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(false);
        if (textoTempo != null) textoTempo.gameObject.SetActive(false);
        if (textoLatas != null) textoLatas.gameObject.SetActive(false);
    }

    private void MostrarUIDaCorrida() // OK
    {
        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(true);
        if (textoTempo != null) textoTempo.gameObject.SetActive(true);
        if (textoLatas != null) textoLatas.gameObject.SetActive(true);
    }
}