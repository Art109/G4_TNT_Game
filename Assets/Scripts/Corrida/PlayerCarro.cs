using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerCarro : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 8f;
    public float limiteEsquerdoX = -3.5f;
    public float limiteDireitoX = 3.5f;

    [Header("Referências UI (Opcional)")]
    

    [Header("Sons")]
    public AudioClip somBatida;

    [Header("Configurações de Jogo")]
    public float tempoExibicaoGameOver = 4.0f;

    
    public static int pontuacaoAtual = 0;
    public static int latasColetadas = 0;
    public static bool LinhaDeChegadaPassou = false;

    
    private ControlesJogador controles;
    private InputAction acaoMover;
   

    
    private UIManager uiManager;
    private bool jogoTerminou = false;
    private AudioSource audioSourceMusicaFundo;


    
    void Awake()
    {
        
        controles = new ControlesJogador();
    }

    void OnEnable()
    {
        
        acaoMover = controles.Gameplay.Mover;
        controles.Gameplay.Enable();
    }

    void OnDisable()
    {
        
        controles.Gameplay.Disable();
    }

    void Start()
    {
        
        pontuacaoAtual = 0;
        latasColetadas = 0;
        jogoTerminou = false;
        Time.timeScale = 1f;
        LinhaDeChegadaPassou = false;

        
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager não encontrado na cena pelo PlayerCarro!");
        }

        
        if (uiManager != null && uiManager.audioSourceMusicaFundo != null)
        {
            audioSourceMusicaFundo = uiManager.audioSourceMusicaFundo;
        }
        
    }

    void Update()
    {
        
        if (Time.timeScale == 0f) return;

        
        if (!jogoTerminou)
        {
            MoverJogador();
        }
    }

    
    void MoverJogador()
    {
        
        if (acaoMover == null) return;

        
        Vector2 inputVector = acaoMover.ReadValue<Vector2>();

        
        float movimentoHorizontal = inputVector.x;

        
        float deslocamentoX = movimentoHorizontal * velocidade * Time.deltaTime;
        float proximaPosicaoX = transform.position.x + deslocamentoX;
        float xLimitado = Mathf.Clamp(proximaPosicaoX, limiteEsquerdoX, limiteDireitoX);
        transform.position = new Vector3(xLimitado, transform.position.y, transform.position.z);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!jogoTerminou && other.CompareTag("Inimigo"))
        {
            IniciarGameOver();
        }
        
    }

    
    void IniciarGameOver()
    {
        if (jogoTerminou) return;
        jogoTerminou = true;
        Time.timeScale = 0f;

        

        
        if (audioSourceMusicaFundo != null && audioSourceMusicaFundo.isPlaying) { audioSourceMusicaFundo.Stop(); }

        
        if (somBatida != null) { AudioSource.PlayClipAtPoint(somBatida, transform.position); }

        
        if (uiManager != null) { uiManager.MostrarGameOver(pontuacaoAtual); }
        else { Debug.LogError("UIManager não encontrado para mostrar Game Over!"); }

        StartCoroutine(ReiniciarAposDelay(tempoExibicaoGameOver));
    }

    IEnumerator ReiniciarAposDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        LinhaDeChegadaPassou = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IniciarVitoria()
    {
        if (jogoTerminou) return;
        jogoTerminou = true;
        Time.timeScale = 0f;

        

        
        if (audioSourceMusicaFundo != null && audioSourceMusicaFundo.isPlaying) { audioSourceMusicaFundo.Stop(); }

        
        if (uiManager != null)
        {
            float tempoFinal = uiManager.GetTempoFinal();
            int pontuacaoOriginal = CalcularPontuacaoFinal(tempoFinal, latasColetadas);
            const int maxScoreOriginalEstimado = 4500;
            int pontuacaoFinalMapeada = 0;
            if (maxScoreOriginalEstimado > 0)
            {
                float proporcao = Mathf.Clamp01((float)Mathf.Max(0, pontuacaoOriginal) / maxScoreOriginalEstimado);
                float proporcaoAjustada = Mathf.Sqrt(proporcao);
                pontuacaoFinalMapeada = Mathf.RoundToInt(proporcaoAjustada * 20f);
            }
            
            uiManager.MostrarPontuacaoFinal(pontuacaoFinalMapeada);
        }
        else { Debug.LogError("UIManager não encontrado para mostrar Vitória!"); }
    }

    
    int CalcularPontuacaoFinal(float tempo, int latas)
    {
        int pontosBasePorLata = 150;
        float fatorTempo = 8000f;
        int bonusBase = 1000;
        if (tempo < 0.1f) tempo = 0.1f;
        int pontuacaoTempo = (int)(fatorTempo / tempo);
        int pontuacaoLatas = latas * pontosBasePorLata;
        int pontuacao = bonusBase + pontuacaoLatas + pontuacaoTempo;
        return Mathf.Max(0, pontuacao);
    }
}