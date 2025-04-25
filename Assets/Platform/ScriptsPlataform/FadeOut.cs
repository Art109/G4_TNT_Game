using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float fadeTimer;
    private bool isFading = false;

    public float startDelay = 0.1f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Start()
    {
        Invoke(nameof(StartFade), startDelay);
    }

    public void StartFade()
    {
        fadeTimer = fadeDuration;
        isFading = true;
    }

    void Update()
    {
        if (isFading)
        {
            fadeTimer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            if (alpha <= 0f)
            {
                isFading = false;
                gameObject.SetActive(false);
            }
        }
    }
}
