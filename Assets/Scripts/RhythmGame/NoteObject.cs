using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToBePressed;

    // Effects
    public GameObject hitEffect, goodHitEffect, perfectHitEffect, missHitEffect;

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
                //GameManager.instance.NoteHit();

                if (Mathf.Abs(transform.position.y) > 0.5f)
                {
                    GameManager.instance.BadHit();
                    Debug.Log("Bad Hit");
                    Instantiate(hitEffect, hitEffect.transform.position, hitEffect.transform.rotation);
                }
                else if (Mathf.Abs(transform.position.y) > 0.25f)
                {
                    GameManager.instance.GoodHit();
                    Debug.Log("Hit");
                    Instantiate(goodHitEffect, goodHitEffect.transform.position, goodHitEffect.transform.rotation);
                }
                else
                {
                    GameManager.instance.PerfectHit();
                    Debug.Log("Perfect");
                    Instantiate(perfectHitEffect, perfectHitEffect.transform.position, perfectHitEffect.transform.rotation);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.activeSelf)
        {
            if (other.tag == "Activator")
            {
                GameManager.instance.NoteMiss();
                GameManager.instance.Punishment();
                canBePressed = false;
                if (gameObject.scene.isLoaded && missHitEffect != null)
                {
                    Instantiate(missHitEffect, missHitEffect.transform.position, missHitEffect.transform.rotation);
                }


            }

        }
        
    }
}

