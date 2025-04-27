using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Painel de Pause")]
    public GameObject painelPause;

    [Header("Controles")]
    public KeyCode teclaFreio = KeyCode.LeftControl;
    public KeyCode teclaBoost = KeyCode.LeftShift;
    public KeyCode teclaIniciarJogo = KeyCode.Space;
    public KeyCode teclaPause = KeyCode.Escape;
    public KeyCode teclaVoltarMenu = KeyCode.Return;

    [Header("Contagem Regressiva")]
    public TextMeshProUGUI textoContagemRegressiva;
    public AudioClip somContagem;
    
    public float delayInicialContagem = 0.5f;
    public float tempoEntreNumeros = 1.0f;

    [Header("Sons Adicionais")]
    public AudioClip somVitoria;
    public AudioSource audioSourceMusicaFundo;
    private AudioSource audioSourceEfeitos;

    [Header("Configurações de Pitch")]
    public float pitchNormalMusica = 1.0f;
    public float pitchBoostMusica = 1.2f;
    public float pitchFreioMusica = 0.8f;

    
    private float tempoDecorrido = 0f;
    private bool cronometroAtivo = false;
    private bool instrucoesAtivas = false;
    private bool jogoPausado = false;
    private bool jogoRealmenteIniciado = false;

    private SceneLoader sceneLoader;

    
    void Awake()
    {
        audioSourceEfeitos = GetComponent<AudioSource>();
        if (audioSourceEfeitos == null)
        {
            
            audioSourceEfeitos = gameObject.AddComponent<AudioSource>();
            audioSourceEfeitos.playOnAwake = false;
            audioSourceEfeitos.spatialBlend = 0f;
        }
        
    }

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        

        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false);
        if (painelGameOver != null) painelGameOver.SetActive(false);
        if (painelPause != null) painelPause.SetActive(false);

        jogoRealmenteIniciado = false;
        jogoPausado = false;
        instrucoesAtivas = false;

        EsconderUIDaCorrida();
        if (textoContagemRegressiva != null)
        {
            textoContagemRegressiva.text = "";
            textoContagemRegressiva.gameObject.SetActive(false);
        }

        if (painelInstrucoes != null && painelInstrucoes.activeSelf)
        {
            instrucoesAtivas = true;
            Time.timeScale = 0f;
        }
        else
        {
            instrucoesAtivas = false;
            Time.timeScale = 0f;
            StartCoroutine(RotinaContagemRegressiva());
        }

        
        if (audioSourceMusicaFundo != null)
        {
            audioSourceMusicaFundo.pitch = pitchNormalMusica;
            
            if (audioSourceMusicaFundo.playOnAwake && !audioSourceMusicaFundo.isPlaying)
            {
                audioSourceMusicaFundo.Play();
            }
        }
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
            if (Input.GetKeyDown(teclaIniciarJogo))
            {
                FecharInstrucoesEIniciarContagem();
            }
            else if (Input.GetKeyDown(teclaVoltarMenu))
            {
                VoltarAoMenuPrincipal();
            }

            return;
        }

        
        bool fimDeJogoAtivo = (painelPontuacaoFinal != null && painelPontuacaoFinal.activeSelf) ||
                              (painelGameOver != null && painelGameOver.activeSelf);
        if (fimDeJogoAtivo)
        {
            
            if (painelPontuacaoFinal != null && painelPontuacaoFinal.activeSelf && Input.GetKeyDown(teclaVoltarMenu))
            {
                if (sceneLoader != null) { Time.timeScale = 1f; sceneLoader.CarregarCenaPorIndice(0); }
                else { Debug.LogError("SceneLoader não encontrado!"); }
            }
            
            return;
        }

        
        if (jogoRealmenteIniciado)
        {
            
            AtualizarPitchMusica();
            AtualizarVelocidadeUI();
            AtualizarTempoUI();
            AtualizarLatasUI();
        }
        
    }

    void VoltarAoMenuPrincipal()
    {
        Time.timeScale = 1f;
        if (sceneLoader != null)
        {
            sceneLoader.CarregarCenaPorIndice(0);
        }
        else
        {
            Debug.LogError("SceneLoader não encontrado, não é possível voltar ao menu!");
        }
    }



    IEnumerator RotinaContagemRegressiva()
    {
        
        EsconderUIDaCorrida();
        if (textoContagemRegressiva != null) textoContagemRegressiva.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(delayInicialContagem);

        
        if (textoContagemRegressiva != null) textoContagemRegressiva.text = "3";
        TocarSom(somContagem);
        yield return new WaitForSecondsRealtime(tempoEntreNumeros);
        
        if (textoContagemRegressiva != null) textoContagemRegressiva.text = "2";
        yield return new WaitForSecondsRealtime(tempoEntreNumeros + 1.0f);
        
        if (textoContagemRegressiva != null) textoContagemRegressiva.text = "1";
        yield return new WaitForSecondsRealtime(tempoEntreNumeros + 0.7f);
        
        if (textoContagemRegressiva != null) textoContagemRegressiva.text = "GO!";
        

        yield return new WaitForSecondsRealtime(0.5f);
        if (textoContagemRegressiva != null) textoContagemRegressiva.gameObject.SetActive(false);

        
        Time.timeScale = 1f;

        
        if (audioSourceMusicaFundo != null && !audioSourceMusicaFundo.isPlaying)
        {
            audioSourceMusicaFundo.Play();
        }

        IniciarJogoDeVerdade();

        
    }

    
    void IniciarJogoDeVerdade()
    {
        jogoRealmenteIniciado = true;
        MostrarUIDaCorrida();
        IniciarCronometro();
        
    }

    void TocarSom(AudioClip clip)
    {
        if (audioSourceEfeitos != null && clip != null)
        {
            audioSourceEfeitos.PlayOneShot(clip);
        }
    }

    void AtualizarPitchMusica()
    {
        if (audioSourceMusicaFundo == null) return;

        if (Input.GetKey(teclaBoost))
        {
            audioSourceMusicaFundo.pitch = pitchBoostMusica;
        }
        else if (Input.GetKey(teclaFreio))
        {
            audioSourceMusicaFundo.pitch = pitchFreioMusica;
        }
        else
        {
            audioSourceMusicaFundo.pitch = pitchNormalMusica;
        }
    }

    
    void VerificarInputPause()
    {
        
        bool fimDeJogoAtivo = (painelPontuacaoFinal != null && painelPontuacaoFinal.activeSelf) ||
                              (painelGameOver != null && painelGameOver.activeSelf);
        if (fimDeJogoAtivo) return;

        if (Input.GetKeyDown(teclaPause))
        {
            Debug.Log("Escape Pressionado. Estado atual jogoPausado = " + jogoPausado);

            if (jogoPausado)
            {
                Debug.Log("-> Chamando DespausarJogo()");
                DespausarJogo();
            }
            
            else if (Time.timeScale > 0f && jogoRealmenteIniciado)
            {
                Debug.Log("-> Chamando PausarJogo()");
                PausarJogo();
            }
            else
            {
                Debug.Log("-> Tentou pausar, mas TimeScale=" + Time.timeScale + " ou jogoRealmenteIniciado=" + jogoRealmenteIniciado);
            }
        }
    }

    void PausarJogo()
    {
        Time.timeScale = 0f;
        if (audioSourceMusicaFundo != null && audioSourceMusicaFundo.isPlaying) { audioSourceMusicaFundo.Pause(); }
        if (painelPause != null) painelPause.SetActive(true);
        jogoPausado = true;
        Debug.Log("Jogo Pausado. jogoPausado = " + jogoPausado + ", Time.timeScale = " + Time.timeScale);
    }

    void DespausarJogo()
    {
        
        jogoPausado = false;
        Time.timeScale = 1f;
        if (audioSourceMusicaFundo != null && !audioSourceMusicaFundo.isPlaying)
        {
            audioSourceMusicaFundo.UnPause();
        }
        if (painelPause != null) painelPause.SetActive(false);
        Debug.Log("Jogo Despausado. jogoPausado = " + jogoPausado + ", Time.timeScale = " + Time.timeScale);
    }
    


    
    void FecharInstrucoesEIniciarContagem()
    {
        if (painelInstrucoes != null) { painelInstrucoes.SetActive(false); }
        instrucoesAtivas = false;
        Time.timeScale = 0f;
        EsconderUIDaCorrida();
        StartCoroutine(RotinaContagemRegressiva());
    }



    void AtualizarVelocidadeUI()
    {
        if (textoVelocidade == null) return;
        float velocidadeAtualExibida = 80;
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
        textoVelocidade.text = $"Vel: {velocidadeAtualExibida:F0} km/h";
        textoVelocidade.color = corAtual;
    }

    void AtualizarTempoUI()
    {
        if (textoTempo == null) return;
        
        if (jogoRealmenteIniciado && cronometroAtivo)
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
        PararCronometro();
        EsconderUIDaCorrida();
        if (painelGameOver != null) painelGameOver.SetActive(false);
        if (painelPause != null) painelPause.SetActive(false);

        TocarSom(somVitoria);

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
        if (painelPause != null) painelPause.SetActive(false);

        if (painelGameOver != null)
        {
            if (textoGameOverMensagem != null)
            {
                
                textoGameOverMensagem.text = "Game Over!\nPontuação: " + pontuacaoFinal;
            }
            else { /* Log Erro */ }
            painelGameOver.SetActive(true);
        }
        else { /* Log Erro */ }
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