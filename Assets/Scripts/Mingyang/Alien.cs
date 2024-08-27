using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Item
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] int score;
    public float changeDirectionCooldown;
    public bool isInScene = false;

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
        Debug.Log("Alien taking damage");
        
        currenthealth -= attacker.Damage;
            
        if (currenthealth < 1)
        {
            Destroy(gameObject);
            GameManager.Instance.scoreChange(score);
        }
            
        
    }
}
