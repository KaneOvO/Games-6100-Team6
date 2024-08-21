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

        if (attacker.CompareTag("Bullet") || attacker.CompareTag("Player"))
        {
            if (isSmallAsteroid)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    Destroy(gameObject);
                }
            }
            else if (isMediumAsteroid)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth < 1)
                {
                    Destroy(gameObject);
                    Instantiate(GameManager.Instance.asteroidPrefabs[0], transform.position, Quaternion.identity);
                }
            }
            else if (isLargeAsteroid)
            {
                currenthealth -= attacker.Damage;
                if (currenthealth <= 0)
                {
                    Instantiate(GameManager.Instance.asteroidPrefabs[1], transform.position, Quaternion.identity);
                }
            }
        }

    }


}
