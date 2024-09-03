using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int damage;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public virtual void TakeDamage(Attack attacker)
    {
        
    }

    void Start()
    {
        currentHealth = health;
    }
}
