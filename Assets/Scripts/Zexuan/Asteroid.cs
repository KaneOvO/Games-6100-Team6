using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Item
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] bool isSmallAsteroid;
    [SerializeField] bool isMediumAsteroid;
    [SerializeField] bool isLargeAsteroid;
    private bool hasTakenDamage = false;
    
    int indexOfSmallAsteroid = 0;
    int indexOfMediumAsteroid = 1;

    public float MinSpeed
    {
        get { return minSpeed; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
    }

    public override void TakeDamage(Attack attacker)
    {
        if (hasTakenDamage) return;
        hasTakenDamage = true;
        Debug.Log("Asteroid taking damage");
        if (attacker.CompareTag("Bullet") || attacker.CompareTag("Player"))
        {
            if (isSmallAsteroid)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    Destroy(gameObject);
                    GameManager.Instance.scoreChange(100);
                }
            }
            else if (isMediumAsteroid)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    Debug.Log("Medium asteroid destroyed");
                    Destroy(gameObject);
                    SplitAsteroid(GameManager.Instance.asteroidPrefabs[indexOfSmallAsteroid]);
                    GameManager.Instance.scoreChange(150);
                }
            }
            else if (isLargeAsteroid)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    Debug.Log("Large asteroid destroyed");
                    Destroy(gameObject);
                    SplitAsteroid(GameManager.Instance.asteroidPrefabs[indexOfMediumAsteroid]);
                    GameManager.Instance.scoreChange(200);
                }
            }
        }
    }

    private void SplitAsteroid(GameObject asteroidPrefab)
    {
        GameObject newAsteroid1 = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
        GameObject newAsteroid2 = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
        AsteroidMovement movement1 = newAsteroid1.GetComponent<AsteroidMovement>();
        AsteroidMovement movement2 = newAsteroid2.GetComponent<AsteroidMovement>();

        // Set random directions for the new asteroids
        Vector2 direction1 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Vector2 direction2 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        movement1.SetDirection(direction1);
        movement2.SetDirection(direction2);
    }
}
