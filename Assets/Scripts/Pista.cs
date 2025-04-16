using UnityEngine;

public class Pista : MonoBehaviour
{
    [Header("Velocidades")]
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float velocidadeFreio = 2f; // Velocidade ao frear

    [Header("Reposição")]
    public float alturaSegmento = 20f; // Distância Y de um segmento (AJUSTE SE NECESSÁRIO)
    public float limiteInferiorY = -15f; // Ponto Y onde o segmento é reposicionado

    // Use as teclas que definiu: LeftControl para Freio, LeftShift para Boost
    public KeyCode teclaFreio = KeyCode.LeftControl;
    public KeyCode teclaBoost = KeyCode.LeftShift;

    void Update()
    {
        // Calcula a velocidade atual baseada no input
        float velocidadeAtual = CalcularVelocidadeAtual();

        // Move o segmento da pista para baixo
        transform.Translate(Vector3.down * velocidadeAtual * Time.deltaTime, Space.World);

        // Verifica se atingiu o limite para reposicionar
        if (transform.position.y < limiteInferiorY)
        {
            Reposicionar();
        }
    }

    // Calcula a velocidade baseada nas teclas pressionadas
    float CalcularVelocidadeAtual()
    {
        if (Input.GetKey(teclaFreio))
        {
            return velocidadeFreio;
        }
        else if (Input.GetKey(teclaBoost))
        {
            return velocidadeBoost;
        }
        else
        {
            return velocidadeNormal;
        }
    }

    // Reposiciona o segmento da pista no topo
    void Reposicionar()
    {
        // Assume que há pelo menos dois segmentos para o efeito contínuo
        // Move este segmento para cima pela altura de dois segmentos
        float distanciaSalto = alturaSegmento * 2f;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + distanciaSalto,
            transform.position.z
        );
        //Debug.Log(gameObject.name + " reposicionado para Y: " + transform.position.y); // Log útil para debug
    }
}