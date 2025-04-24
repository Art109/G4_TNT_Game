using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{


    public Animator animator;
    void Start()
    {
        
    }


    void Update()
    {
        if (GameManager.instance.startplaying && GameManager.instance.menuPause == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PlayAnimation("Up");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PlayAnimation("Down");
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PlayAnimation("Left");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PlayAnimation("Right");
            }
            if(GameManager.instance.missed)
            {
                PlayAnimation("Hurt");
                GameManager.instance.missed = false;
            }
        }

    }
    void PlayAnimation(string animationName)
    {
        animator.Play(animationName, 0, 0f); // Força a animação a reiniciar imediatamente
    }
}
