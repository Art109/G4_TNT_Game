using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Actives : MonoBehaviour
{
    private string CurrentMode;
    public GameObject KImg;
    public GameObject CImg;

    // Update is called once per frame
    void Update()
    {
        if (CurrentMode == "Keyboard")
        {
            CImg.SetActive(false);
            KImg.SetActive(true);
        }
        if (CurrentMode == "Controller")
        {
            KImg.SetActive(false);
            CImg.SetActive(true);
        }
    }
    private void UpdateText(InputDetect.InputMode mode)
    {
        CurrentMode += mode;
    }
    private void OnEnable()
    {
        InputDetect.OninputModeChanged += UpdateText;
    }
    private void OnDisable()
    {
        InputDetect.OninputModeChanged -= UpdateText;
    }
}
