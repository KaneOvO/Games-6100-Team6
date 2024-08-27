using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ArcadeTextEffect : MonoBehaviour
{
    public TextMeshProUGUI tmpText;                 // Reference to the TextMeshPro text
    public string gameSceneName = "GameScene";      // Name of the game scene to load

    public float flashDuration = 0.5f;              // Duration of one flash cycle
    public int quickFlashes = 3;                    // Number of quick flashes after key press
    public float quickFlashDuration = 0.1f;         // Duration of each quick flash
    public float delayBeforeSceneSwitch = 0.5f;     // Delay before switching to the game scene

    private bool isFlashing = true;

    void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(FlashingText());
    }

    void Update()
    {
        // Detect any key press
        if (Input.anyKeyDown && isFlashing)
        {
            isFlashing = false;
            StopCoroutine(FlashingText());
            StartCoroutine(QuickFlashAndSwitchScene());
        }
    }

    IEnumerator FlashingText()
    {
        while (isFlashing)
        {
            tmpText.enabled = !tmpText.enabled;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    IEnumerator QuickFlashAndSwitchScene()
    {
        for (int i = 0; i < quickFlashes; i++)
        {
            tmpText.enabled = true;
            yield return new WaitForSeconds(quickFlashDuration);
            tmpText.enabled = false;
            yield return new WaitForSeconds(quickFlashDuration);
        }

        // Ensure the text is visible before switching
        tmpText.enabled = true;

        // Wait for a brief moment before switching the scene
        yield return new WaitForSeconds(delayBeforeSceneSwitch);

        // Switch to the game scene
        SceneManager.LoadScene(gameSceneName);
    }
}

