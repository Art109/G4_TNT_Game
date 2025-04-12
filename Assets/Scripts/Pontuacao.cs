using UnityEngine;

public class ColetavelPonto : MonoBehaviour
{
    
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float limiteInferiorY = -10f;
    public float posicaoRespawnY = 10f;
    public float limiteEsquerdoX = -3.5f;
    public float limiteDireitoX = 3.5f;

    
    public int pontosPorColeta = 10;
    public AudioSource somColeta;

    private SpriteRenderer spriteRenderer;
    private Collider2D colisor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colisor = GetComponent<Collider2D>();
    }

    void Update()
    {
        MoverColetavel();
        VerificarSaidaDaTela();
    }

    void MoverColetavel()
    {
        float velocidadeAtual;
        if (Input.GetKey(KeyCode.Space))
        {
            velocidadeAtual = velocidadeBoost;
        }
        else
        {
            velocidadeAtual = velocidadeNormal;
        }
        transform.Translate(Vector3.down * velocidadeAtual * Time.deltaTime, Space.World);
    }

    void VerificarSaidaDaTela()
    {
        if (transform.position.y < limiteInferiorY)
        {
            Respawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Coletar();
        }
    }

    void Coletar()
    {
        PlayerCarro.pontuacaoAtual += pontosPorColeta;
        Debug.Log("Coletou! Pontuação atual: " + PlayerCarro.pontuacaoAtual);

        if (somColeta != null)
        {
            AudioSource.PlayClipAtPoint(somColeta.clip, transform.position, somColeta.volume);
        }

        spriteRenderer.enabled = false;
        colisor.enabled = false;

        Respawn(false);
    }

    void Respawn(bool calcularNovaPosicaoX = true)
    {
        float novaPosicaoX = transform.position.x;
        if (calcularNovaPosicaoX)
        {
            novaPosicaoX = Random.Range(limiteEsquerdoX, limiteDireitoX);
        }

        transform.position = new Vector3(novaPosicaoX, posicaoRespawnY, transform.position.z);

        spriteRenderer.enabled = true;
        colisor.enabled = true;
    }
}