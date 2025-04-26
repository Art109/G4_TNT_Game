using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;


public class ControllerManager : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame ||
            Gamepad.current?.allControls.Any(control => control is ButtonControl button && button.wasPressedThisFrame) == true)
        {
            SceneManager.LoadScene("PlataformaPrototipo");
        }
    }
}
