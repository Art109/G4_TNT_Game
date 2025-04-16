using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Elementos de UI Durante a Corrida")]
    public TextMeshProUGUI textoVelocidade;
    public TextMeshProUGUI textoTempo;
    public TextMeshProUGUI textoLatas;

    [Header("Elementos de UI Fim de Corrida")]
    public GameObject painelPontuacaoFinal;
    public TextMeshProUGUI textoPontuacaoFinal;
    public GameObject painelGameOver;


    private float tempoDecorrido = 0f;
    private bool cronometroAtivo = false;

    public float velocidadeNormalBase = 5f;
    public float velocidadeBoostBase = 10f;

    void Start()
    {
        // Garante que ambos os painéis finais comecem escondidos
        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false);
        if (painelGameOver != null) painelGameOver.SetActive(false); // <<< GARANTIR QUE ESTEJA DESATIVADO

        // Opcional: reativar UI da corrida caso esteja reiniciando a cena
        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(true);
        if (textoTempo != null) textoTempo.gameObject.SetActive(true);
        if (textoLatas != null) textoLatas.gameObject.SetActive(true);

        IniciarCronometro();
    }

    void Update()
    {
        // Só atualiza a UI da corrida se nenhum painel final estiver ativo
        if ((painelPontuacaoFinal == null || !painelPontuacaoFinal.activeSelf) &&
            (painelGameOver == null || !painelGameOver.activeSelf))
        {
            AtualizarVelocidadeUI();
            AtualizarTempoUI();
            AtualizarLatasUI();
        }
    }

    void AtualizarVelocidadeUI()
    {
        if (textoVelocidade == null) return;

        float velocidadeAtualExibida;
        
        if (Input.GetKey(KeyCode.Space))
        {
            velocidadeAtualExibida = velocidadeBoostBase;
        }
        else
        {
            velocidadeAtualExibida = velocidadeNormalBase;
        }

        textoVelocidade.text = $"Velocidade: {velocidadeAtualExibida.ToString("F0")} km/h";
    }

    void AtualizarTempoUI()
    {
        if (textoTempo == null) return;

        if (cronometroAtivo)
        {
            tempoDecorrido += Time.deltaTime;
        }

        int minutos = Mathf.FloorToInt(tempoDecorrido / 60F);
        int segundos = Mathf.FloorToInt(tempoDecorrido - minutos * 60);
        textoTempo.text = $"Tempo: {minutos:00}:{segundos:00}";
    }

    void AtualizarLatasUI()
    {
        if (textoLatas == null) return;

        textoLatas.text = $"Latas: {PlayerCarro.latasColetadas}";
    }

    public void IniciarCronometro()
    {
        tempoDecorrido = 0f;
        cronometroAtivo = true;
    }

    public void PararCronometro()
    {
        cronometroAtivo = false;
    }

    public float GetTempoFinal()
    {
        return tempoDecorrido;
    }

    // Método modificado para incluir "VOCÊ VENCEU"
    public void MostrarPontuacaoFinal(int pontuacaoMapeada)
    {
        PararCronometro();
        EsconderUIDaCorrida(); // Esconde UI da corrida

        // Garante que o outro painel esteja escondido
        if (painelGameOver != null) painelGameOver.SetActive(false);

        if (painelPontuacaoFinal != null && textoPontuacaoFinal != null)
        {
            // <<< TEXTO DE VITÓRIA ATUALIZADO >>>
            textoPontuacaoFinal.text = $"VOCÊ VENCEU!\nPontuação: {pontuacaoMapeada} / 20";
            painelPontuacaoFinal.SetActive(true);
        }
    }

    // <<< NOVO MÉTODO para mostrar Game Over >>>
    public void MostrarGameOver()
    {
        PararCronometro();
        EsconderUIDaCorrida(); // Esconde UI da corrida

        // Garante que o outro painel esteja escondido
        if (painelPontuacaoFinal != null) painelPontuacaoFinal.SetActive(false);

        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
            // O texto já está definido no editor, então só precisamos ativar o painel.
        }
    }

    private void EsconderUIDaCorrida()
    {
        if (textoVelocidade != null) textoVelocidade.gameObject.SetActive(false);
        if (textoTempo != null) textoTempo.gameObject.SetActive(false);
        if (textoLatas != null) textoLatas.gameObject.SetActive(false);
    }
}
