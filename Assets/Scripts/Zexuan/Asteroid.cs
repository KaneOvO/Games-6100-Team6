using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Item
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] enum AsteroidType
    {
        Small,
        Medium,
        Large
    }
    [SerializeField] AsteroidType asteroidType;
    private bool hasTakenDamage = false;

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
            if (asteroidType == AsteroidType.Small)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    Destroy(gameObject);
                    GameManager.Instance.scoreChange(100);
                }
            }
            else if (asteroidType == AsteroidType.Medium)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    Debug.Log("Medium asteroid destroyed");
                    Destroy(gameObject);
                    SplitAsteroid(GameManager.Instance.asteroidPrefabs[(int)AsteroidType.Small]);
                    GameManager.Instance.scoreChange(150);
                }
            }
            else if (asteroidType == AsteroidType.Large)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    Debug.Log("Large asteroid destroyed");
                    Destroy(gameObject);
                    SplitAsteroid(GameManager.Instance.asteroidPrefabs[(int)AsteroidType.Medium]);
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

        Vector2 direction1 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Vector2 direction2 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        movement1.SetDirection(direction1);
        movement2.SetDirection(direction2);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int damageAmount = damage > 0 ? damage : 1;
            PlayerDamageEvent.TriggerPlayerDamage(damageAmount);
        }
    }
}
