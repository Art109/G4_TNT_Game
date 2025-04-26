using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ExitGame : MonoBehaviour
{
    
    
        
    private void Update()
    {
        EndGame();
    }
    // Update is called once per frame

    private void EndGame()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Exit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Aplication.OpenURL("https://tntenergydrink.com.br/");
#else
            Application.Quit();
#endif
        }


    }

}
