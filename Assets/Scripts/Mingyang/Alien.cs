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
    public float changeDirectionCooldown;
    public bool isInScene = false; 
    private float sinceFire;
    
       

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
    }

    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        sinceFire += Time.deltaTime;
        if (sinceFire < fireCooldown) return;
        if (Random.Range(0, 20) > 5) return;
        
        Instantiate(alienMissilePrefab, transform.position, Quaternion.identity);
        sinceFire = 0;
    }

    public override void TakeDamage(Attack attacker)
    {
        Debug.Log("Alien taking damage");
        
        currentHealth -= attacker.Damage;
        if (attacker.CompareTag("Enemy") || attacker.CompareTag("Player") || attacker.CompareTag("Bullet") || attacker.CompareTag("Planet"))
        {
            if (currentHealth < 1)
            {
                Destroy(gameObject);
                GameManager.Instance.scoreChange(score);
            }
        }    
        
    }

    public void OnDestroy()
    {
        AudioManager.Instance.Play("Explosion_Alien");
    }
}
