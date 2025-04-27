using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private GamepadInput GamepadInputComponent;
    public Animator keyboardAnimator;

    private void Awake()
    {
        GamepadInputComponent = FindObjectOfType<GamepadInput>();
    }


    void Update()
    {
        if (GameManager.instance.startplaying && GameManager.instance.menuPause == false)
        {

            // KeyBoard
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
            if (GameManager.instance.missed)
            {
                PlayAnimation("Hurt");
                GameManager.instance.missed = false;
            }

            // GamePad
            if (GamepadInputComponent.onButtonDown["UpArrow"])
            {
                PlayAnimation("Up");
            }
            if (GamepadInputComponent.onButtonDown["DownArrow"])
            {
                PlayAnimation("Down");
            }
            if (GamepadInputComponent.onButtonDown["LeftArrow"])
            {
                PlayAnimation("Left");
            }
            if (GamepadInputComponent.onButtonDown["RightArrow"])
            {
                PlayAnimation("Right");
            }


        }
        void PlayAnimation(string animationName)
        {
            // Força a animação a reiniciar imediatamente

            keyboardAnimator.Play(animationName, 0, 0f);
          ; 
        }
    }
}
