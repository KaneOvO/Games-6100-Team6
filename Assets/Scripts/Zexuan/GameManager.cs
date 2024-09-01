using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject[] asteroidPrefabs;
    public GameObject player;
    public GameObject playerPrefab;
    public GameObject alienPrefab;
    public GameObject hook;
    // GameObject[] asteroids;
    // GameObject[] aliens;
    private int score;
    [SerializeField] public int lives = 3;
    [SerializeField] int respawnInvinciblePeriod;
    public bool isGameOver = false;
    public float xBorderOffset;
    public float yBorderOffset;
    [SerializeField] float generateCooldown;
    [SerializeField] int asteroidsPerBatch;
    [SerializeField] int indexSmallAsteroid;
    [SerializeField] int indexMediumAsteroid;
    [SerializeField] int indexLargeAsteroid;
    public int ropeBlockCount;
    public bool isRetracting = false;
    public bool moveToTarget = false;
    public bool isGrappling = false;
    public bool isShootGrapple = false;
    [SerializeField] float edgeSafeOffset;



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

        hook = GameObject.FindWithTag("Hook");
    }

    void Start()
    {
        GenerateBatchAsteroids();
        // int asteroidCount = Random.Range(3, 6);
        // GenerateAsteroids(asteroidCount);

        InvokeRepeating("GenerateAlien", generateCooldown, generateCooldown);
        InvokeRepeating("GeneratePlanet", generateCooldown, generateCooldown);
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

    void GeneratePlanet()
    {
        GameObject planetPrefab = asteroidPrefabs[3];
        Vector3 Position = GetRandomOffScreenPosition();
        Instantiate(planetPrefab, Position, Quaternion.identity);
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

    Vector2 GetRandomOffScreenPosition(bool isAlien = false)
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

    public void PlayerDeath()
    {
        Instance.lives--;

        if (Instance.lives <= 0)
        {
            Instance.GameOver();
            Destroy(player);
        }
        else
        {
            StartCoroutine(Destory());
            Destroy(player);
        }
    }

    public IEnumerator Destory()
    {
        yield return new WaitForSeconds(2);
        Destroy(player);
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        hook.GetComponent<Hook>().hookHolder = player;
        hook.GetComponent<Hook>().pivot = player.transform.Find("Pivot").gameObject;
        player.GetComponent<Ship>().isInvincible = true;
        StartCoroutine(player.GetComponent<Ship>().FlashBlue(respawnInvinciblePeriod));
        StartCoroutine(StopRespawnInvincible(respawnInvinciblePeriod));
    }

    public void GameOver()
    {
        Destroy(player);
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

    public bool isUsingHook()
    {
        return isGrappling || isShootGrapple || isRetracting || moveToTarget;
    }


    public float getWorldSceneX(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        if (viewportPosition.x < 0 + GameManager.Instance.xBorderOffset)
        {
            return 1 - GameManager.Instance.xBorderOffset - edgeSafeOffset;
        }
        else if (viewportPosition.x > 1 - GameManager.Instance.xBorderOffset)
        {
            return 0 + GameManager.Instance.xBorderOffset + edgeSafeOffset;
        }
        return viewportPosition.x;
    }

    public float getWorldSceneY(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        if (viewportPosition.y < 0 + GameManager.Instance.yBorderOffset)
        {
            return 1 - GameManager.Instance.yBorderOffset - edgeSafeOffset;
        }
        else if (viewportPosition.y > 1 - GameManager.Instance.yBorderOffset)
        {
            return 0 + GameManager.Instance.yBorderOffset + edgeSafeOffset;
        }
        return viewportPosition.y;
    }

    public bool inWorldScene(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        if (viewportPosition.y < 0 + GameManager.Instance.yBorderOffset)
        {
            return false;
        }
        else if (viewportPosition.y > 1 - GameManager.Instance.yBorderOffset)
        {
            return false;
        }
        if (viewportPosition.x < 0 + GameManager.Instance.xBorderOffset)
        {
            return false;
        }
        else if (viewportPosition.x > 1 - GameManager.Instance.xBorderOffset)
        {
            return false;
        }
        return true;
    }

    public Vector3 GetWorldScenePosition(Vector3 position)
    {
        return new Vector3(getWorldSceneX(position), getWorldSceneY(position), 0);
    }

    IEnumerator StopRespawnInvincible(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.GetComponent<Ship>().isInvincible = false;
    }

    public void applyBuff(Buff buff1, Buff buff2)
    {
        Time.timeScale = 0;
        UIManager.Instance.choosePanel.SetActive(true);
        UIManager.Instance.buff1Name.GetComponent<TextMeshProUGUI>().text = buff1.name;
        UIManager.Instance.buff1Description.GetComponent<TextMeshProUGUI>().text = buff1.description;
        UIManager.Instance.buff1ApplyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        UIManager.Instance.buff1ApplyButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuffManager.Instance.AddBuff(buff1);
            UIManager.Instance.choosePanel.SetActive(false);
            Time.timeScale = 1;
        });

        UIManager.Instance.buff2Name.GetComponent<TextMeshProUGUI>().text = buff2.name;
        UIManager.Instance.buff2Description.GetComponent<TextMeshProUGUI>().text = buff2.description;
        UIManager.Instance.buff2ApplyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        UIManager.Instance.buff2ApplyButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuffManager.Instance.AddBuff(buff2);
            UIManager.Instance.choosePanel.SetActive(false);
            Time.timeScale = 1;
        });

    }
}
