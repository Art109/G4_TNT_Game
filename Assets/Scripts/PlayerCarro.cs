using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCarro : MonoBehaviour
{
    public float velocidade = 8f;
    public float limiteEsquerdoX = -3.5f;
    public float limiteDireitoX = 3.5f;

    private bool jogoTerminou = false;
    public static int pontuacaoAtual = 0;

    void Start()
    {
        pontuacaoAtual = 0;
        Time.timeScale = 1f;
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
        if (!jogoTerminou && other.CompareTag("Inimigo"))
        {
            IniciarGameOver();
        }
    }

    void IniciarGameOver()
    {
        if (jogoTerminou) return;

        jogoTerminou = true;
        Debug.Log("Colisão! Fim de Jogo!");
        Debug.Log("Pontuação Final: " + pontuacaoAtual);

        Time.timeScale = 0f;
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
        Debug.Log("Você Venceu!");
        Debug.Log("Pontuação Final: " + pontuacaoAtual);

        Time.timeScale = 0f;
    }
}