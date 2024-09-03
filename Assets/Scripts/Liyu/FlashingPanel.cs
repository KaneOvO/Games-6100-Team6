using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlashingPanel : MonoBehaviour
{
    public GameObject panel;                // The panel GameObject
    public float fadeDuration = 1f;         // Duration for one fade cycle (in and out)
    public int fadeRepeats = 3;             // Number of fade in-out cycles
    public float finalFadeDelay = 0.5f;     // Delay before the panel disappears

    private CanvasGroup panelCanvasGroup;   // CanvasGroup to control panel transparency
    private Image[] images;                 // Images within the panel
    private TextMeshProUGUI[] textMeshes;   // TextMeshPro components within the panel

    void Start()
    {
        // Get CanvasGroup component (add it if it's not present)
        panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
        {
            panelCanvasGroup = panel.AddComponent<CanvasGroup>();
        }

        // Get all Image and TextMeshPro components within the panel
        images = panel.GetComponentsInChildren<Image>();
        textMeshes = panel.GetComponentsInChildren<TextMeshProUGUI>();

        // Start the fade effect coroutine
        StartCoroutine(FadeEffect());
    }

    IEnumerator FadeEffect()
    {
        // Repeat the fade in and out process multiple times
        for (int i = 0; i < fadeRepeats; i++)
        {
            // Fade out
            yield return StartCoroutine(Fade(1f, 0f, fadeDuration / 2));

            // Fade in
            yield return StartCoroutine(Fade(0f, 1f, fadeDuration / 2));
        }

        // Wait for a moment before hiding the panel
        yield return new WaitForSeconds(finalFadeDelay);

        // Hide the panel
        panel.SetActive(false);
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            // Apply the alpha value to all images and text elements, excluding the panel itself
            foreach (var image in images)
            {
                if (image.gameObject != panel)
                {
                    Color color = image.color;
                    color.a = alpha;
                    image.color = color;
                }
            }

            foreach (var textMesh in textMeshes)
            {
                Color color = textMesh.color;
                color.a = alpha;
                textMesh.color = color;
            }

            yield return null;
        }

        // Ensure the final alpha value is set
        foreach (var image in images)
        {
            if (image.gameObject != panel)
            {
                Color color = image.color;
                color.a = endAlpha;
                image.color = color;
            }
        }

        foreach (var textMesh in textMeshes)
        {
            Color color = textMesh.color;
            color.a = endAlpha;
            textMesh.color = color;
        }
    }
}