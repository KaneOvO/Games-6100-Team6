using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int currenthealth;
    [SerializeField] protected int damage;

    //tag 列表
    [SerializeField] protected List<string> enemyTags;
    [SerializeField] protected List<string> allyTags;

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
        get { return currenthealth; }
        set { currenthealth = value; }
    }

    public void TakeDamage(Attack attacker)
    {
        if (enemyTags.Contains(attacker.tag))
        {
            OnEnemyDamage(attacker);
        }
        else if (allyTags.Contains(attacker.tag))
        {
            OnAllyBuff(attacker);
        }
    }

    public virtual void OnEnemyDamage(Attack attacker)
    {

    }

    public virtual void OnAllyBuff(Attack attacker)
    {

    }
}
