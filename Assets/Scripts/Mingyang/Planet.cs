using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Item
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
        if (attacker.Damage > 0)
        {
            hasTakenDamage = true;
        }
        if (attacker.CompareTag("Bullet") || attacker.CompareTag("Player"))
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
