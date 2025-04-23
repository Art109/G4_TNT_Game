using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer SR;
    public Sprite defaultImage;
    public Sprite pressedImage;
    public GameObject buttons;

    public KeyCode keyToPress;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (GameManager.instance.startplaying && GameManager.instance.menuPause == false)
        {
            buttons.SetActive(true);
   

            if (Input.GetKeyDown(keyToPress))
            {
                SR.sprite = pressedImage;
            }
            if (Input.GetKeyUp(keyToPress))
            {
                SR.sprite = defaultImage;
            }
        }
    }

}
