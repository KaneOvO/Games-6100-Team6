using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    Asteroid asteroid;
    private GameObject player;
    public float speed;
    public Vector2 direction;
    public bool isFreezen = false;
    public bool isWrapping = false;
    [SerializeField] private float spwanOffset = 0.2f;

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
        //Debug.Log("Direction: " + direction);
        //Debug.Log("New Direction: " + (Vector3)direction * spwanOffset);
        transform.position += (Vector3)direction * spwanOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (isFreezen) return;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        WrapAroundScreen();
    }

    // void OnBecameInvisible()
    // {
    //     if(gameObject.GetComponent<Asteroid>().isPlanet)
    //     {
    //         Debug.Log("Planet is invisible");
    //     }
    //     asteroid.isInScene = false;
    // }

    // void OnBecameVisible()
    // {
    //     if(gameObject.GetComponent<Asteroid>().isPlanet)
    //     {
    //         Debug.Log("Planet is visible");
    //     }
    //     asteroid.isInScene = true;
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
            if (viewportPosition.x < 0 + GameManager.Instance.xBorderOffset) return;
            if (viewportPosition.x > 1 - GameManager.Instance.xBorderOffset) return;
            if (viewportPosition.y < 0 + GameManager.Instance.yBorderOffset) return;
            if (viewportPosition.y > 1 - GameManager.Instance.yBorderOffset) return;
            return;
        }

        Vector3 newPosition = viewportPosition;
        newPosition.x = GameManager.Instance.getWorldSceneX(gameObject);
        newPosition.y = GameManager.Instance.getWorldSceneY(gameObject);

        transform.position = Camera.main.ViewportToWorldPoint(newPosition);
        if (isWrapping)
        {
            Invoke("stopIsWrapping", 0.5f);
        }


    }



    void stopIsWrapping()
    {
        isWrapping = false;
    }
}