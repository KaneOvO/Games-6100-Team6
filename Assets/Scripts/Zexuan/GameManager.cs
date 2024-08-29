using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject[] asteroidPrefabs;
    public GameObject player;
    public GameObject alienPrefab;
    // GameObject[] asteroids;
    // GameObject[] aliens;
    public int score;
    public bool isGameOver = false;
    [SerializeField] float generateCooldown;
    [SerializeField] int asteroidsPerBatch;
    [SerializeField] int indexSmallAsteroid;
    [SerializeField] int indexMediumAsteroid;
    [SerializeField] int indexLargeAsteroid;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    void Start()
    {
        GenerateBatchAsteroids();
        // int asteroidCount = Random.Range(3, 6);
        // GenerateAsteroids(asteroidCount);

        InvokeRepeating("GenerateAlien", generateCooldown, generateCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            GenerateBatchAsteroids();
        }
    }

    void GenerateAlien()
    {
        Vector3 Position = GetRandomOffScreenPosition(true);
        Instantiate(alienPrefab, Position, Quaternion.identity);
        
    }

    void GenerateBatchAsteroids()
    {
        for (int i = 0; i < asteroidsPerBatch; i++)
        {
            GameObject asteroidPrefab = asteroidPrefabs[indexLargeAsteroid];
            Vector3 Position = GetRandomOffScreenPosition();
            Instantiate(asteroidPrefab, Position, Quaternion.identity);
        }
    }

    void GenerateAsteroids(int generateCount)
    {
        for (int i = 0; i < generateCount; i++)
        {
            GameObject asteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            Vector3 Position = GetRandomOffScreenPosition();
            Instantiate(asteroidPrefab, Position, Quaternion.identity);
        }
    }

    void GenerateSingleAsteroid()
    {
        int randomIndex = Random.Range(0, 3);
        GenerateAsteroids(randomIndex);
    }

    public GameObject GetRandomAsteroid()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Enemy");
        if (asteroids.Length == 0)
        {
            return null;
        }
        GameObject asteroid = asteroids[Random.Range(0, asteroids.Length)];
        return asteroid;
    }
    public GameObject GetPlayer()
    {
        return player;
    }

    Vector2 GetRandomOffScreenPosition(bool isAlien=false)
    {
        //Get the screen width and height
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Generate a random position on the edge of the screen
        float x = 0;
        float y = 0;
        int edge = Random.Range(0, 4);
        if (isAlien)
        {
            edge = Random.Range(0, 2);
        }

        switch (edge)
        {
            case 0:
                x = -10;
                y = Random.Range(0, screenHeight);
                break;
            case 1:
                x = screenWidth + 10;
                y = Random.Range(0, screenHeight);
                break;
            case 2:
                x = Random.Range(0, screenWidth);
                y = screenHeight + 10;
                break;
            case 3:
                x = Random.Range(0, screenWidth);
                y = -10;
                break;
        }

        // Convert screen position to world position
        return Camera.main.ScreenToWorldPoint(new Vector2(x, y));
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void GameOver()
    {
        isGameOver = true;
        // CancelInvoke("GenerateSingleAsteroid");
        UIManager.Instance.GameOver();
    }

    public void scoreChange(int change)
    {
        score += change;
        UIManager.Instance.UpdateScore(score);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
