using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnCollision : MonoBehaviour
{
    public float delayBeforeSceneChange = 2f; // Delay in seconds before changing the scene
    public string sceneToLoad = "NextScene"; // Name of the scene to load
    private bool playerCollided = false; // To track if the player has collided

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if GameManager's isGrappling is true
            if (GameManager.Instance.isGrappling)
            {
                // Start the coroutine to change the scene after a delay
                StartCoroutine(ChangeSceneAfterDelay());
            }
        }
    }

    private IEnumerator ChangeSceneAfterDelay()
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // Change the scene to the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}