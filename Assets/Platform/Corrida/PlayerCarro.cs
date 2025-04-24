using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCarro : MonoBehaviour
{
    public float velocidade = 8f;
    public float limiteEsquerdoX = -3.5f;
    public float limiteDireitoX = 3.5f;

    // Renomeado para consistência e removido static se não for necessário globalmente
    public static int pontuacaoAtual = 0; // Mantido static para ColetavelPonto acessar
    public static int latasColetadas = 0; // Mantido static para ColetavelPonto acessar

    // --- Adição: Tempo de exibição do Game Over ---
    public float tempoExibicaoGameOver = 4.0f;
    // -------------------------------------------

    // Flag estática para parada de respawn (necessária para LinhaChegada/Gerenciador)
    // Certifique-se que ela existe se a lógica de parada de respawn ainda for desejada
    public static bool LinhaDeChegadaPassou = false;

    private UIManager uiManager;
    private bool jogoTerminou = false;

    void Start()
    {
        pontuacaoAtual = 0;
        latasColetadas = 0;
        jogoTerminou = false;
        Time.timeScale = 1f;
        LinhaDeChegadaPassou = false; // Reseta a flag

        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager não encontrado na cena! Verifique se o GameObject está ativo e com o script anexado.");
        }
        // Opcional: Chamar reset do UIManager aqui se necessário
        // if (uiManager != null) uiManager.ResetUI();
    }

    void Update()
    {
        if (!jogoTerminou)
        {
            MoverJogador();
        }
        // Opcional: Atualizar HUD de pontuação/latas via UIManager se necessário
        // if (uiManager != null && !jogoTerminou) uiManager.AtualizarHUD(pontuacaoAtual, latasColetadas);
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
        if (!jogoTerminou)
        {
            if (other.CompareTag("Inimigo"))
            {
                IniciarGameOver();
            }
            // A lógica de coletar latas deve estar no ColetavelPonto.cs, incrementando as variáveis estáticas
        }
    }

    void IniciarGameOver()
    {
        if (jogoTerminou) return;
        jogoTerminou = true;
        Time.timeScale = 0f; // Pausa o jogo

        Debug.Log("Colisão! Fim de Jogo! Pontuação Final: " + pontuacaoAtual); // Log interno

        // --- Modificação: Passa a pontuação para o UIManager ---
        if (uiManager != null)
        {
            // Assumindo que MostrarGameOver agora aceita a pontuação
            uiManager.MostrarGameOver(pontuacaoAtual);
        }
        else
        {
            Debug.LogError("UIManager não encontrado para mostrar Game Over!");
        }
        // ------------------------------------------------------

        // Inicia a rotina para reiniciar usando o tempo definido
        StartCoroutine(ReiniciarAposDelay(tempoExibicaoGameOver));
    }

    IEnumerator ReiniciarAposDelay(float delay)
    {
        // Usa WaitForSecondsRealtime para esperar mesmo com Time.timeScale = 0
        yield return new WaitForSecondsRealtime(delay);

        // Ações ANTES de recarregar a cena
        Time.timeScale = 1f; // Restaura o tempo
        LinhaDeChegadaPassou = false; // Reseta a flag

        // Recarrega a cena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void IniciarVitoria()
    {
        if (jogoTerminou) return;
        jogoTerminou = true;
        Time.timeScale = 0f;

        if (uiManager != null)
        {
            float tempoFinal = uiManager.GetTempoFinal(); // Assume que UIManager controla o tempo
            int pontuacaoOriginal = CalcularPontuacaoFinal(tempoFinal, latasColetadas);
            const int maxScoreOriginalEstimado = 7000; // Mantido seu cálculo
            int pontuacaoFinalMapeada = 0;
            if (maxScoreOriginalEstimado > 0)
            {
                float proporcao = Mathf.Clamp01((float)Mathf.Max(0, pontuacaoOriginal) / maxScoreOriginalEstimado);
                pontuacaoFinalMapeada = Mathf.RoundToInt(proporcao * 20f);
            }
            // Passa a pontuação mapeada para o UIManager mostrar
            uiManager.MostrarPontuacaoFinal(pontuacaoFinalMapeada);
        }
        else
        {
            Debug.LogError("Não foi possível mostrar pontuação final, UIManager não encontrado!");
        }
    }

    // Cálculo de pontuação (mantido como estava)
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