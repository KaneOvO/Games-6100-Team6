using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Item
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] int score;
    [SerializeField] GameObject alienMissilePrefab;
    [SerializeField] float fireCooldown;
    private float realFireCooldown;
    public float changeDirectionCooldown;
    public bool isInScene = false; 
    private float sinceFire;
    private bool allowFire;
    
       

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
        sinceFire = 0;
        realFireCooldown = fireCooldown;
        allowFire = true;
        currentHealth = health;
    }

    void Update()
    {
        if (allowFire)
        {
            Fire();

        }
    }

    private void Fire()
    {
        sinceFire += Time.deltaTime;
        if (sinceFire < fireCooldown) return;
        
        Instantiate(alienMissilePrefab, transform.position, Quaternion.identity);
        sinceFire = 0;
        realFireCooldown = fireCooldown * Random.Range(7, 13) / 10;
    }

    public override void TakeDamage(Attack attacker)
    {
        //Debug.Log("Alien taking damage");
        
        
        if (attacker.CompareTag("Enemy") || attacker.CompareTag("Player") || attacker.CompareTag("Bullet") || attacker.CompareTag("Planet"))
        {
            currentHealth -= attacker.Damage;
            if (currentHealth < 1)
            {
                Destroy(gameObject);
                if (attacker.CompareTag("Player"))
                {
                    GameManager.Instance.scoreChange(score);
                }
            }
        }   
        else if (attacker.CompareTag("Hook"))
        {
            allowFire = false;
        }
        
    }

    public void OnDestroy()
    {
        AudioManager.Instance.Play("Explosion_Alien");
    }
}
