using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static bool isChangingScene = false;

    public void LoadNewScene()
    {
        isChangingScene = true;
        SceneManager.LoadScene(1);
    }
}
