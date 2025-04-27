using UnityEngine;
using UnityEngine.InputSystem;

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

    private PlayerInput carInput;

    private void Start()
    {
        carInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        float velocidadeAtual = CalcularVelocidadeAtual();

        
        transform.Translate(Vector3.down * velocidadeAtual * Time.deltaTime, Space.World);

        
        if (transform.position.y < limiteInferiorY)
        {
            Reposicionar();
        }
    }

    
    float CalcularVelocidadeAtual()
    {
        if (carInput.actions["Freiar"].IsPressed())
        {
            return velocidadeFreio;
        }
        else if (carInput.actions["Acelerar"].IsPressed())
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