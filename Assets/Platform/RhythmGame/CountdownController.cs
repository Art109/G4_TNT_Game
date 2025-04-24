using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownController : MonoBehaviour
{
    public int countTime;
    public Text countText;
    public GameObject GOTextPrefab;
    void Start()
    {
       
    }

    public static IEnumerator LoadSceneAfterDelay()
    {
        yield return null; // espera 1 frame
        SceneManager.LoadScene(1);
    }
}
