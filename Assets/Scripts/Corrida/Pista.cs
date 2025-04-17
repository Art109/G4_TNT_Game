using UnityEngine;

public class Pista : MonoBehaviour
{
    [Header("Velocidades")]
    public float velocidadeNormal = 5f;
    public float velocidadeBoost = 10f;
    public float velocidadeFreio = 2f;

    [Header("Reposição")]
    public float alturaSegmento = 20f;
    public float limiteInferiorY = -15f;

    
    public KeyCode teclaFreio = KeyCode.LeftControl;
    public KeyCode teclaBoost = KeyCode.LeftShift;

    void Update()
    {
        
        float velocidadeAtual = CalcularVelocidadeAtual();

        
        transform.Translate(Vector3.down * velocidadeAtual * Time.deltaTime, Space.World);

        
        if (transform.position.y < limiteInferiorY)
        {
            Reposicionar();
        }
    }

    
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

    
    void Reposicionar()
    {
        
        float distanciaSalto = alturaSegmento * 2f;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + distanciaSalto,
            transform.position.z
        );
        
    }
}