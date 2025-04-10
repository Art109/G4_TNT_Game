using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToBePressed;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToBePressed))
        {
            if (canBePressed) 
            {
                gameObject.SetActive(false);
                GameManager.instance.NoteHit();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Activator")
        {
            canBePressed = true;
     
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
            GameManager.instance.NoteMiss();
        }
    }
}
