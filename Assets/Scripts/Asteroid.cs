using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Item
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] float dropItemProbability = 0.2f;
    enum AsteroidType
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

    public override void OnEnemyDamage(Attack attacker)
    {
        if (hasTakenDamage) return;
        hasTakenDamage = true;
        Debug.Log("Asteroid taking damage");
        if (asteroidType == AsteroidType.Small)
        {
            currenthealth -= attacker.Damage;
            if (currenthealth < 1)
            {
                GameManager.Instance.scoreChange(100);
                Destroy(gameObject);
            }
        }
        else if (asteroidType == AsteroidType.Medium)
        {
            currenthealth -= attacker.Damage;
            if (currenthealth < 1)
            {
                SplitAsteroid(GameManager.Instance.asteroidPrefabs[(int)AsteroidType.Small]);
                GameManager.Instance.scoreChange(150);
                Destroy(gameObject);
            }
        }
        else if (asteroidType == AsteroidType.Large)
        {
            currenthealth -= attacker.Damage;
            if (currenthealth < 1)
            {
                SplitAsteroid(GameManager.Instance.asteroidPrefabs[(int)AsteroidType.Medium]);
                GameManager.Instance.scoreChange(200);
                Destroy(gameObject);
            }
        }

        GenerateDropItem();

        AudioManager.Instance.PlaySound(Random.Range(0, 3));
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

    private void GenerateDropItem()
    {
        if (Random.value > dropItemProbability)
        {
            return;
        }

        int randomIndex = Random.Range(0, 2);

        switch (randomIndex)
        {
            case 0:
                Instantiate(heartPrefab, transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                break;
        }
    }
}
