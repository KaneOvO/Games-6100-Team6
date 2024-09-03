using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    Planet planet;
    private GameObject player;
    public float speed;
    public Vector2 direction;
    public bool isFreezen = false;

    void Awake()
    {
        planet = GetComponent<Planet>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Randomly set speed
        speed = Random.Range(planet.MinSpeed, planet.MaxSpeed);

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
    }

    void OnBecameInvisible()
    {
        //destroy the asteroid when it goes off screen
        planet.isInScene = false;
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    
}