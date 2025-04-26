using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private float dotSpeed = 0.5f;

    private void OnEnable()
    {
        StartCoroutine(AnimateDots());
    }

    private IEnumerator AnimateDots()
    {
        string baseText = "Carregando";
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount + 1) % 4; // 0, 1, 2, 3
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(dotSpeed);
        }
    }
}