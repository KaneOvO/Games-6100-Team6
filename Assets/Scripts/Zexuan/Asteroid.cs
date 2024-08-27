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
    [SerializeField] bool hasTakenDamage = false;

    public bool isInScene = true;

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
        if(attacker.Damage > 0)
        {
            hasTakenDamage = true;
        }
        Debug.Log("Asteroid taking damage");
        if (attacker.CompareTag("Bullet") || attacker.CompareTag("Player"))
        {
            if (isSmallAsteroid)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    GameManager.Instance.scoreChange(100);
                    Destroy(gameObject);
                }
            }
            else if (isMediumAsteroid)
            {
                Debug.Log("Medium asteroid destroyed");
                currenthealth -= attacker.Damage;

                if (currenthealth < 1)
                {
                    if (attacker.CompareTag("Player") && attacker.GetComponent<Ship>().isConsumption == false)
                    {
                        SplitAsteroid(GameManager.Instance.asteroidPrefabs[indexOfSmallAsteroid]);
                    }

                    GameManager.Instance.scoreChange(150);
                    Destroy(gameObject);
                }
            }
            else if (isLargeAsteroid)
            {
                currenthealth -= attacker.Damage;

                if (currenthealth < 1)
                {
                    Debug.Log("Large asteroid destroyed");
                    
                    if (attacker.CompareTag("Player") && attacker.GetComponent<Ship>().isConsumption == false)
                    {
                        SplitAsteroid(GameManager.Instance.asteroidPrefabs[indexOfMediumAsteroid]);
                    }

                    GameManager.Instance.scoreChange(200);
                    Destroy(gameObject);
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
