using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Item
{
    [SerializeField] float speed;
    Rigidbody2D missileRb;

    void Awake()
    {
        missileRb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        
        missileRb.velocity = transform.up * speed;
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
    
    public override void OnEnemyDamage(Attack attacker)
    {
        currenthealth -= attacker.Damage;
        if (currenthealth <= 0)
        {
            gameObject.SetActive(false);
        }

    }
}
