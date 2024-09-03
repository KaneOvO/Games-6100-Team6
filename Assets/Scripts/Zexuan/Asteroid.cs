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
    [SerializeField] public bool isPlanet;
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

    void Start()
    {
        StartCoroutine(CallFunctionWithDelay(1f));
    }

    public override void TakeDamage(Attack attacker)
    {
        if (hasTakenDamage) return;
        if (attacker.Damage > 0)
        {
            hasTakenDamage = true;
        }
        if (attacker.CompareTag("Bullet") || attacker.CompareTag("Player") || attacker.CompareTag("Alien") || attacker.CompareTag("AlienMissile"))
        {
            if (isSmallAsteroid)
            {
                currentHealth -= attacker.Damage;
                if (currentHealth < 1)
                {
                    GameManager.Instance.scoreChange(100);
                    Destroy(gameObject);
                }
            }
            else if (isMediumAsteroid)
            {
                currentHealth-= attacker.Damage;

                if (currentHealth < 1)
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
                currentHealth -= attacker.Damage;

                if (currentHealth < 1)
                {

                    if (attacker.CompareTag("Player") && attacker.GetComponent<Ship>().isConsumption == false)
                    {
                        SplitAsteroid(GameManager.Instance.asteroidPrefabs[indexOfMediumAsteroid]);
                    }

                    GameManager.Instance.scoreChange(200);
                    Destroy(gameObject);
                }
            }
            else if (isPlanet)
            {
                currentHealth -= attacker.Damage;

                if (currentHealth < 1)
                {
                    if (attacker.CompareTag("Player") && GameManager.Instance.isGrappling)
                    {
                        GameManager.Instance.scoreChange(500);
                        Buff buff1 = BuffContainer.Instance.GetRandomBuff();
                        Buff buff2 = BuffContainer.Instance.GetRandomBuff();
                        while (buff1.name == buff2.name)
                        {
                            buff2 = BuffContainer.Instance.GetRandomBuff();
                        }
                        GameManager.Instance.applyBuff(buff1, buff2);
                    }
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

    IEnumerator CallFunctionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetDamage();

    }

    void SetDamage()
    {
        GetComponent<Attack>().Damage = 1;
    }
}
