using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ship : Item
{
    [SerializeField] float speed;
    [SerializeField] float rotation;
    [SerializeField] float linearDrag;
    [SerializeField] ParticleSystem boostParticle;
    [SerializeField] ParticleSystem collisionParticle;
    public SpriteRenderer sprite; //Adding this in order to make the player flash blue while invincible
    public bool isConsumption = false;
    
    public GameObject grappleObject;
    public bool isInvincible = false;

    public float Speed
    {
        get { return speed; }
    }

    public float Rotation
    {
        get { return rotation; }
        set { rotation = value; }
    }

    public float LinearDrag
    {
        get { return linearDrag; }
    }

    public ParticleSystem BoostParticle
    {
        get { return boostParticle; }
    }

    public override void TakeDamage(Attack attacker)
    {
        if (attacker.CompareTag("Enemy") || attacker.CompareTag("Alien") || attacker.CompareTag("AlienMissile") || attacker.CompareTag("Planet"))
        {
            if (GameManager.Instance.isGrappling || isInvincible)
            {
                return;
            }
            currentHealth -= attacker.Damage;
            if (currentHealth <= 0)
            {
                GameManager.Instance.PlayerDeath();   
            }
        }

    }

    public IEnumerator FlashBlue(float invinciblePeriod)
    {
        sprite.color = Color.blue;
        yield return new WaitForSeconds(invinciblePeriod);
        sprite.color = Color.white;
    }

    public void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        
    }

    void Start()
    {
        GameManager.Instance.hook.GetComponent<Hook>().spriteRenderer.enabled = true;
        currentHealth = health;
    }

    void Update()
    {
        // if(isInvincible && !isShowInvincible)
        // {
        //     isShowInvincible = true;
        //     StartCoroutine(FlashBlue());
        // }
    }

    private void OnDestroy()
    {
        // if(grappleObject != null)
        // {
        //     grappleObject.GetComponent<AsteroidMovement>().isFreezen = false;
        // }

        if(GameManager.Instance.hook != null)
        {
            GameManager.Instance.hook.GetComponent<Hook>().spriteRenderer.enabled = false;
        }
    }






}
