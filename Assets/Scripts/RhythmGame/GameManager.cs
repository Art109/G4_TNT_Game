using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource audSrc;
    public bool startplaying;
    public NoteScroll noteScroll;
    public static GameManager instance;
    void Start()
    {
        instance = this;
    }


    void Update()
    {
        if (!startplaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startplaying = true;
                noteScroll.hasStarted = true;
                audSrc.Play();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Note Hit!");
    }
    public void NoteMiss()
    {
        Debug.Log("Note Miss");
    }
}
