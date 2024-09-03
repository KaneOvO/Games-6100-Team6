using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public GameObject gameOverPanel;
    public GameObject choosePanel;
    public GameObject buff1Name, buff1Description, buff1ApplyButton, buff2Name, buff2Description, buff2ApplyButton;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }
    public void UpdateLife(int lives)
    {
        lifeText.text = "Lives: " + GameManager.Instance.lives.ToString();
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
