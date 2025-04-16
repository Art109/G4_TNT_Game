using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCarro : MonoBehaviour
{
    public float velocidade = 5f;
    public float limiteEsquerdoX = -2.3f;
    public float limiteDireitoX = 2.3f;
    

    public static int pontuacaoAtual = 0;

    public static int latasColetadas = 0;

    private UIManager uiManager;

    private bool jogoTerminou = false;

    void Start()
    {
        pontuacaoAtual = 0;
        latasColetadas = 0;
        jogoTerminou = false;
        Time.timeScale = 1f;
        uiManager = FindObjectOfType<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("UIManager não encontrado na cena!");
        }
    }

    void Update()
    {
        if (!jogoTerminou)
        {
            MoverJogador();
        }
    }

    void MoverJogador()
    {
        float movimentoHorizontal = Input.GetAxis("Horizontal");
        float deslocamentoX = movimentoHorizontal * velocidade * Time.deltaTime;
        float proximaPosicaoX = transform.position.x + deslocamentoX;
        float xLimitado = Mathf.Clamp(proximaPosicaoX, limiteEsquerdoX, limiteDireitoX);
        transform.position = new Vector3(xLimitado, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Inimigo"))
        {
            IniciarGameOver();
        }
    }

    void IniciarGameOver()
    {
        if (jogoTerminou) return;

        jogoTerminou = true;

        //Debug.Log("Colisão! Fim de Jogo!");

        //Debug.Log("Pontuação Final: " + pontuacaoAtual);

        Time.timeScale = 0f;

        if (uiManager != null)
        {
            uiManager.MostrarGameOver();
        }

        StartCoroutine(ReiniciarAposDelay(2.0f));
    }

    IEnumerator ReiniciarAposDelay(float delay)
    {
        float tempoEsperado = Time.realtimeSinceStartup + delay;
        while (Time.realtimeSinceStartup < tempoEsperado)
        {
            yield return null;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    public void IniciarVitoria()
    {
        if (jogoTerminou) return;

        jogoTerminou = true;

        //Debug.Log("Você Venceu!");

        Time.timeScale = 0f;

        if (uiManager != null)
        {
            float tempoFinal = uiManager.GetTempoFinal();

            int pontuacaoOriginal = CalcularPontuacaoFinal(tempoFinal, latasColetadas);

            const int maxScoreOriginalEstimado = 5000;

            int pontuacaoFinalMapeada = 0;

            if (maxScoreOriginalEstimado > 0)
            {
                float proporcao = Mathf.Clamp01((float)Mathf.Max(0, pontuacaoOriginal) / maxScoreOriginalEstimado);
                pontuacaoFinalMapeada = Mathf.RoundToInt(proporcao * 20f);
            }

            Debug.Log($"Tempo Final: {tempoFinal:F2}s, Latas: {latasColetadas}, Pontuação Original: {pontuacaoOriginal}, Pontuação Mapeada (0-20): {pontuacaoFinalMapeada}");

            //Debug.Log($"Tempo Final: {tempoFinal:F2}s, Latas: {latasColetadas}, Pontuação Calculada: {pontuacaoCalculada}");

            uiManager.MostrarPontuacaoFinal(pontuacaoFinalMapeada);
        }
        else
        {
            Debug.LogError("Não foi possível mostrar pontuação final, UIManager não encontrado!");
        }
    }

    int CalcularPontuacaoFinal(float tempo, int latas)
    {
        int pontosBasePorLata = 100;
        float fatorTempo = 10000f;
        int bonusBase = 500;

        if (tempo < 0.1f) tempo = 0.1f;

        int pontuacao = bonusBase + (latas * pontosBasePorLata) + (int)(fatorTempo / tempo);

        return Mathf.Max(0, pontuacao);
    }
}