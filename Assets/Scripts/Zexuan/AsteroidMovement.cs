using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    Asteroid asteroid;
    private GameObject player;
    private float speed;
    public Vector2 direction;

    void Awake()
    {
        asteroid = GetComponent<Asteroid>();
    }

    void Start()
    {
        speed = Random.Range(asteroid.MinSpeed, asteroid.MaxSpeed);

        if (direction == Vector2.zero)
        {
            player = GameManager.Instance.player;

            Vector2 randomScreenPoint = new(
                Random.Range(0, Screen.width),
                Random.Range(0, Screen.height)
            );

            Vector2 randomWorldPoint = Camera.main.ScreenToWorldPoint(randomScreenPoint);

            direction = (randomWorldPoint - (Vector2)transform.position).normalized;
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
}