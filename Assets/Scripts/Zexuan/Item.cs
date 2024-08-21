using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int currenthealth;
    [SerializeField] protected int damage;

    public int Health
    {
        get { return Health; }
        set { Health = value; }
    }

    public int Damage
    {
        get { return Damage; }
        set { Damage = value; }
    }

    public int CurrentHealth
    {
        get { return CurrentHealth; }
        set { CurrentHealth = value; }
    }

    public virtual void TakeDamage(Attack attacker)
    {

    }
    
}
