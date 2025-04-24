using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroll : MonoBehaviour
{
    public float beatTime;
    public bool hasStarted;
    void Start()
    {
        beatTime = beatTime / 60f;
    }


    void Update()
    {
        if (!hasStarted)
        {
           /* if (Input.GetKeyDown(KeyCode.Space))
            {
                hasStarted = true;
            }*/
        }
        else
        {
            transform.position -= new Vector3(0f, beatTime * Time.deltaTime, 0f);
        }
    }
}
