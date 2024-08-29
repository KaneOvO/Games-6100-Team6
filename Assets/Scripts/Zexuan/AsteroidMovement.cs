using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    Asteroid asteroid;
    private GameObject player;
    private float speed;
    public Vector2 direction;
    public bool isFreezen = false;

    void Awake()
    {
        asteroid = GetComponent<Asteroid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Randomly set speed
        speed = Random.Range(asteroid.MinSpeed, asteroid.MaxSpeed);

        if (direction == Vector2.zero)
        {
            player = GameManager.Instance.player;

            // Generate a random point on the screen
            Vector2 randomScreenPoint = new Vector2(
                Random.Range(0, Screen.width),
                Random.Range(0, Screen.height)
            );

            // Convert the screen point to world point
            Vector2 randomWorldPoint = Camera.main.ScreenToWorldPoint(randomScreenPoint);

            // Calculate direction towards the random world point
            direction = (randomWorldPoint - (Vector2)transform.position).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (isFreezen) return;
        transform.Translate(direction * speed * Time.deltaTime);
        WrapAroundScreen();
    }

    // void OnBecameInvisible()
    // {
    //     //destroy the asteroid when it goes off screen
    //     asteroid.isInScene = false;
    //     Destroy(gameObject);
    // }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void WrapAroundScreen()
    {

        Vector3 position = transform.position;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        if (asteroid.isInScene == false)
        {
            if (viewportPosition.x < 0) return;
            if (viewportPosition.x > 1) return;
            if (viewportPosition.y < 0) return;
            if (viewportPosition.y > 1) return;
            asteroid.isInScene = true;
            return;
        }
        if (viewportPosition.x < 0)
        {
            viewportPosition.x = 1;
        }
        else if (viewportPosition.x > 1)
        {
            viewportPosition.x = 0;
        }

        if (viewportPosition.y < 0)
        {
            viewportPosition.y = 1;
        }
        else if (viewportPosition.y > 1)
        {
            viewportPosition.y = 0;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition);
    }
}