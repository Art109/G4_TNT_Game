using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    [Tooltip("Nome exato do arquivo da cena do Menu Principal (ex: MainMenu, MenuScene)")]
    public string nomeCenaMenu = "MainMenu";

    
    public void CarregarMenuPrincipal()
    {
        Debug.Log($"Tentando carregar cena: {nomeCenaMenu}");

        
        Time.timeScale = 1f;

        
        if (!string.IsNullOrEmpty(nomeCenaMenu))
        {
            
            SceneManager.LoadScene(nomeCenaMenu);
        }
        else
        {
            Debug.LogError("Nome da Cena do Menu não definido no SceneLoader!");
        }
    }

    // Opcional: Função para carregar pelo índice (se preferir)
    public void CarregarCenaPorIndice(int indice)
    {
        Debug.Log($"Tentando carregar cena pelo índice: {indice}");
        Time.timeScale = 1f;
        SceneManager.LoadScene(indice);
    }

    // Opcional: Função para reiniciar a cena atual
    public void ReiniciarCenaAtual()
    {
        Debug.Log("Reiniciando cena atual...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}