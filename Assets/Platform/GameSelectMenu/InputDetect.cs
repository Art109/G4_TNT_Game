using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InputDetect : MonoBehaviour
{
    public enum InputMode { Controller, Keyboard, Touch }
    private InputMode currentMode;
    private InputMode impModeLastFrame;
    public static Action<InputMode> OninputModeChanged;
    void Start()
    {
        currentMode = InputMode.Keyboard;
    }

    // Update is called once per frame
    void Update()
    {
        currentMode = InputChange();
        if (currentMode != impModeLastFrame)
        {
            OninputModeChanged?.Invoke(currentMode);
        }
        impModeLastFrame = InputChange();
    }
    private InputMode InputChange()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return InputMode.Touch;
        }
        if (Input.GetJoystickNames().Length == 0)
        {
            return InputMode.Keyboard;
        }
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button1)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button2)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button3)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button4)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button5)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button6)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button7)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button8)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button9)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button10)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button11)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button12)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button13)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button14)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button15)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button16)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button17)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button18)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.Joystick1Button19)) return InputMode.Controller;
            else return InputMode.Keyboard;
        }
        return currentMode;
    }
}
