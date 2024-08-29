using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMissile : Item
{
    [SerializeField] float speed;
    Rigidbody2D missileRb;
    // Start is called before the first frame update

    void Awake()
    {
        missileRb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject target = GameManager.Instance.GetRandomAsteroid();
        if (!GameManager.Instance.isGameOver && (Random.Range(0, 2) < 1 || target == null))
        {
            target = GameManager.Instance.GetPlayer();        
        }

        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = target.transform.position;

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.up = direction;
        missileRb.velocity = direction * speed;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
    
    public override void TakeDamage(Attack attacker)
    {
        if (attacker.CompareTag("Enemy") || attacker.CompareTag("Player"))
        {
            currenthealth -= attacker.Damage;
            if (currenthealth <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
