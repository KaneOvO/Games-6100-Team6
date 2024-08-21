using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject[] asteroidPrefabs;
    public GameObject player;
    public int score;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

        InvokeRepeating("GenerateSingleAsteroid", 3.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            CancelInvoke("GenerateSingleAsteroid");
        }
    }

    void GenerateAsteroids(int generateCount = 1)
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
        GenerateAsteroids(1);
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
}
