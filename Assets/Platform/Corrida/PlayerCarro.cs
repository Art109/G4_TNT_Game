using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCarro : MonoBehaviour
{
    public float velocidade = 8f;
    public float limiteEsquerdoX = -3.5f;
    public float limiteDireitoX = 3.5f;

    // Renomeado para consist�ncia e removido static se n�o for necess�rio globalmente
    public static int pontuacaoAtual = 0; // Mantido static para ColetavelPonto acessar
    public static int latasColetadas = 0; // Mantido static para ColetavelPonto acessar

    // --- Adi��o: Tempo de exibi��o do Game Over ---
    public float tempoExibicaoGameOver = 4.0f;
    // -------------------------------------------

    // Flag est�tica para parada de respawn (necess�ria para LinhaChegada/Gerenciador)
    // Certifique-se que ela existe se a l�gica de parada de respawn ainda for desejada
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
            Debug.LogError("UIManager n�o encontrado na cena! Verifique se o GameObject est� ativo e com o script anexado.");
        }
        // Opcional: Chamar reset do UIManager aqui se necess�rio
        // if (uiManager != null) uiManager.ResetUI();
    }

    void Update()
    {
        if (!jogoTerminou)
        {
            MoverJogador();
        }
        // Opcional: Atualizar HUD de pontua��o/latas via UIManager se necess�rio
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
            // A l�gica de coletar latas deve estar no ColetavelPonto.cs, incrementando as vari�veis est�ticas
        }
    }

    void IniciarGameOver()
    {
        if (jogoTerminou) return;
        jogoTerminou = true;
        Time.timeScale = 0f; // Pausa o jogo

        Debug.Log("Colis�o! Fim de Jogo! Pontua��o Final: " + pontuacaoAtual); // Log interno

        // --- Modifica��o: Passa a pontua��o para o UIManager ---
        if (uiManager != null)
        {
            // Assumindo que MostrarGameOver agora aceita a pontua��o
            uiManager.MostrarGameOver(pontuacaoAtual);
        }
        else
        {
            Debug.LogError("UIManager n�o encontrado para mostrar Game Over!");
        }
        // ------------------------------------------------------

        // Inicia a rotina para reiniciar usando o tempo definido
        StartCoroutine(ReiniciarAposDelay(tempoExibicaoGameOver));
    }

    IEnumerator ReiniciarAposDelay(float delay)
    {
        // Usa WaitForSecondsRealtime para esperar mesmo com Time.timeScale = 0
        yield return new WaitForSecondsRealtime(delay);

        // A��es ANTES de recarregar a cena
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
            const int maxScoreOriginalEstimado = 7000; // Mantido seu c�lculo
            int pontuacaoFinalMapeada = 0;
            if (maxScoreOriginalEstimado > 0)
            {
                float proporcao = Mathf.Clamp01((float)Mathf.Max(0, pontuacaoOriginal) / maxScoreOriginalEstimado);
                pontuacaoFinalMapeada = Mathf.RoundToInt(proporcao * 20f);
            }
            // Passa a pontua��o mapeada para o UIManager mostrar
            uiManager.MostrarPontuacaoFinal(pontuacaoFinalMapeada);
        }
        else
        {
            Debug.LogError("N�o foi poss�vel mostrar pontua��o final, UIManager n�o encontrado!");
        }
    }

    // C�lculo de pontua��o (mantido como estava)
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