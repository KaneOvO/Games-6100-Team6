using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharlesAsteroidMovement : MonoBehaviour
{
    Asteroid asteroid;
    private GameObject player;
    private float speed;
    public Vector2 direction;

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
            player = GameManagerPlanet.Instance.player;

            // // Calculate direction towards player
            // if (player != null)
            // {
            //     direction = (player.transform.position - transform.position).normalized;
            // }
            // else
            // {
            //     Debug.LogWarning("Player not set for asteroid movement.");
            // }

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
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        //destroy the asteroid when it goes off screen
        asteroid.isInScene = false;
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
}