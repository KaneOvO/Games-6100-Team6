using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AlienMovement : MonoBehaviour
{
    Alien alien;
    private GameObject player;
    public float speed;
    public Vector2 direction;
    public bool towardsOne;
    private float sinceChangeDirection;
    public bool isFreezen = false; 

    void Awake()
    {
        alien = GetComponent<Alien>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Randomly set speed
        speed = Random.Range(alien.MinSpeed, alien.MaxSpeed);

        if (direction == Vector2.zero)
        {            
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

            Vector3 viewportDirection = Camera.main.WorldToViewportPoint(direction);
            if (viewportDirection.x < 0.5)
            {
                towardsOne = true;
            }
            else
            {
                towardsOne = false;
            }

        }
        sinceChangeDirection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (isFreezen) return;
        transform.Translate(direction * speed * Time.deltaTime);
        WrapAroundScreenY();
        ChangeDirection();
    }

    void OnBecameInvisible()
    {
        //destroy the asteroid when it goes off screen
        alien.isInScene = false;
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void ChangeDirection()
    {
        sinceChangeDirection += Time.deltaTime;
        if (sinceChangeDirection < alien.changeDirectionCooldown) return;
        if (Random.Range(0, 20) > 5) return;
        direction.y = -direction.y;
        sinceChangeDirection = 0;
    }
    private void WrapAroundScreenY()
    {
        
        Vector3 position = transform.position;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        if (!alien.isInScene)
        {
            if (viewportPosition.x < 0 + GameManager.Instance.xBorderOffset) return;
            if (viewportPosition.x > 1 - GameManager.Instance.xBorderOffset) return;
            if (viewportPosition.y < 0 + GameManager.Instance.yBorderOffset) return;
            if (viewportPosition.y > 1 - GameManager.Instance.yBorderOffset) return;
            alien.isInScene = true;
            return;
        }

        Vector3 newPosition = viewportPosition;
        newPosition.y = GameManager.Instance.getWorldSceneY(position);

        transform.position = Camera.main.ViewportToWorldPoint(newPosition);
    }
}