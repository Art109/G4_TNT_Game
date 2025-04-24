using UnityEngine;
using TMPro; // Usando TextMeshPro como no seu c�digo original
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
    public GameObject painelPontuacaoFinal; // Painel de Vit�ria
    public TextMeshProUGUI textoPontuacaoFinal; // Texto de Vit�ria
    public GameObject painelGameOver;           // Painel de Game Over
    // --- Adi��o: Refer�ncia ao TEXTO dentro do painel de Game Over ---
    public TextMeshProUGUI textoGameOverMensagem; // Conecte o texto de "VOC� BATEU..." aqui
    // ----------------------------------------------------------------

    [Header("Tela de Instru��es")]
    public GameObject painelInstrucoes;

    // Removido: Valores Base para UI (Parece que a velocidade � lida de Input agora)
    // public float velocidadeNormalBase = 5f;
    // public float velocidadeBoostBase = 10f;
    // public float velocidadeFreioBase = 2f;

    // Mantido KeyCodes para refer�ncia
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
        if (painelGameOver != null) painelGameOver.SetActive(false); // Garante que Game Over come�a escondido

        if (painelInstrucoes != null && painelInstrucoes.activeSelf) // L�gica de Instru��es OK
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

        // Verifica se est� na tela de vit�ria para voltar ao menu
        if (painelPontuacaoFinal != null && painelPontuacaoFinal.activeSelf)
        {
            if (Input.GetKeyDown(teclaVoltarMenu))
            {
                Debug.Log("Tecla Voltar Menu pressionada na tela de vit�ria.");
                if (sceneLoader != null) { sceneLoader.CarregarCenaPorIndice(0); }
                else { Debug.LogError("SceneLoader n�o encontrado!"); }
            }
            // --- Adi��o: Impede que a l�gica da corrida continue na tela de vit�ria ---
            return; // Sai do Update se a tela de vit�ria est� ativa
                    // ----------------------------------------------------------------------
        }

        // Verifica se est� na tela de Game Over (O PlayerCarro j� cuida do reinicio com delay)
        if (painelGameOver != null && painelGameOver.activeSelf)
        {
            // N�o precisa de input aqui, PlayerCarro j� iniciou a coroutine de rein�cio
            // Apenas impede que a l�gica da corrida continue
            return; // Sai do Update se a tela de Game Over est� ativa
        }


        // --- L�gica Durante a Corrida ---
        // (S� executa se n�o estiver nas instru��es, nem em vit�ria, nem em game over)
        AtualizarVelocidadeUI(); // Atualiza baseado em Input, OK
        AtualizarTempoUI();      // Atualiza o cron�metro
        AtualizarLatasUI();      // Atualiza contador de latas
        // -----------------------------

        // Parar cron�metro n�o � mais necess�rio aqui, GetTempoFinal j� faz isso
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


    void AtualizarVelocidadeUI() // L�gica OK (usa Input, n�o valores base)
    {
        if (textoVelocidade == null) return;
        float velocidadeAtualExibida = 0f; // Valor padr�o
        Color corAtual = corVelocidadeNormal;

        // Nota: PlayerCarro usa Space para boost? Este UIManager usa Shift. Manter consistente.
        if (Input.GetKey(teclaFreio)) // Usando teclaFreio definida
        {
            // Simula uma velocidade de freio visualmente
            // Seria melhor obter a velocidade REAL do PlayerCarro se poss�vel
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

    void AtualizarLatasUI() // OK (usa vari�vel est�tica)
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

    // MostrarPontuacaoFinal (Vit�ria) - OK
    public void MostrarPontuacaoFinal(int pontuacaoMapeada)
    {
        PararCronometro(); // Garante que parou
        EsconderUIDaCorrida();
        if (painelGameOver != null) painelGameOver.SetActive(false); // Esconde Game Over se estiver ativo
        if (painelPontuacaoFinal != null && textoPontuacaoFinal != null)
        {
            textoPontuacaoFinal.text = $"VOC� VENCEU!\nPontua��o: {pontuacaoMapeada} / 20";
            painelPontuacaoFinal.SetActive(true);
        }
    }

    // --- Fun��o MostrarGameOver MODIFICADA ---
    // Agora aceita a pontua��o como par�metro
    public void MostrarGameOver(int pontuacaoFinal)
    {
        PararCronometro(); // Garante que parou
        EsconderUIDaCorrida();
        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false); // Esconde Vit�ria se estiver ativa

        if (painelGameOver != null)
        {
            // Verifica se a refer�ncia ao texto da mensagem existe
            if (textoGameOverMensagem != null)
            {
                // Define o texto incluindo a pontua��o
                textoGameOverMensagem.text = "VOC� BATEU!\nTENTE NOVAMENTE.\nPontua��o: " + pontuacaoFinal;
            }
            else
            {
                Debug.LogError("Refer�ncia textoGameOverMensagem n�o definida no UIManager!");
                // Opcional: Mostrar uma mensagem padr�o se o texto n�o foi ligado
                // Tente pegar o componente TextMeshProUGUI no painel se poss�vel
                var tmp = painelGameOver.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null) tmp.text = "VOC� BATEU!";
            }
            // Ativa o painel de Game Over
            painelGameOver.SetActive(true);
        }
        else
        {
            Debug.LogError("Refer�ncia painelGameOver n�o definida no UIManager!");
        }
    }
    // --- Fim da Fun��o Modificada ---

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