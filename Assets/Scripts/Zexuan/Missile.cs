using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Item
{
    [SerializeField] float speed;
    Rigidbody2D missileRb;
    // Start is called before the first frame update

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
    
    public override void TakeDamage(Attack attacker)
    {
        if (attacker.CompareTag("Enemy") || attacker.CompareTag("Alien"))
        {
            currenthealth -= attacker.Damage;
            if (currenthealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }

    }
}
