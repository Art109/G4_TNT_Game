using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Carro : MonoBehaviour
{
    public float velocidade = 8f;
    public float limiteEsquerdoX = -3.5f;
    public float limiteDireitoX = 3.5f;

    private bool jogoTerminou = false;

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
        jogoTerminou = true;
        Debug.Log("Colisão! Fim de Jogo!");
        
        Time.timeScale = 0f;

        StartCoroutine(ReiniciarAposDelay(1.5f));
    }

    IEnumerator ReiniciarAposDelay(float delay)
    {
        
        yield return new WaitForSecondsRealtime(delay);

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}