using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetC : Item
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] bool hasTakenDamage = false;

    public bool isInScene = true;


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
        if(attacker.Damage > 0)
        {
            hasTakenDamage = true;
        }
        if (attacker.CompareTag("Bullet") || attacker.CompareTag("Player") || attacker.CompareTag("Alien") || attacker.CompareTag("AlienMissile"))
        {
            
                currentHealth -= attacker.Damage;
                if (currentHealth < 1)
                {
                    GameManager.Instance.scoreChange(100);
                    Destroy(gameObject);
                }
           
        }
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
