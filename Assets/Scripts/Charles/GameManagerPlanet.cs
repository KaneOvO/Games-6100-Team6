using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerPlanet : MonoBehaviour
{
    public static GameManagerPlanet Instance { get; private set; }
    public GameObject[] asteroidPrefabs;
    public GameObject[] planetPrefabs;
    public GameObject player;
    public int score;
    //private bool isGameOver = false;
    [SerializeField] float generateCooldown;
    [SerializeField] float planetCooldown;

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
        int asteroidCount = Random.Range(3, 6);
        GenerateAsteroids(asteroidCount);

        InvokeRepeating("GenerateSingleAsteroid", generateCooldown, generateCooldown);
        InvokeRepeating("GenerateSinglePlanet", planetCooldown, planetCooldown);
    }

    // Update is called once per frame
    void Update()
    {

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
    void GeneratePlanet(int generateCount)
    {
        for (int i = 0; i < generateCount; i++)
        {
            GameObject planetPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];
            Vector3 Position = GetRandomOffScreenPosition();
            Instantiate(planetPrefab, Position, Quaternion.identity);
        }
    }

    void GenerateSingleAsteroid()
    {
        int randomIndex = Random.Range(0, asteroidPrefabs.Length);
        GenerateAsteroids(randomIndex);
    }
    void GenerateSinglePlanet()
    {
        GeneratePlanet(1);
    }

    Vector2 GetRandomOffScreenPosition()
    {
        //Get the screen width and height
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Generate a random position on the edge of the screen
        float x = 0;
        float y = 0;
        int edge = Random.Range(0, 4);

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
        //isGameOver = true;
        CancelInvoke("GenerateSingleAsteroid");
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
