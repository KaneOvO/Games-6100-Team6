using UnityEngine;

public class RestartOnSpace : MonoBehaviour
{
    void Update()
    {
        // Check if the player presses the spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call the RestartGame method from the GameManager
            GameManager.Instance.RestartGame();
        }
    }
}
