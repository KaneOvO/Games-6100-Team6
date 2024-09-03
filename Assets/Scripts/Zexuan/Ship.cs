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
                GameManager.Instance.isGrappling = false;
                GameManager.Instance.isShootGrapple = false;
                GameManager.Instance.hook.GetComponent<Hook>().isHooked = false;
                GameManager.Instance.hook.GetComponent<Hook>().target = null;
                GameManager.Instance.hook.GetComponent<Hook>().launchTarget = null;
                GameManager.Instance.isRetracting = false;

                GameManager.Instance.hook.GetComponent<Hook>().spriteRenderer.enabled = false;
                GameManager.Instance.hook.transform.position = Vector2.zero;

                GameManager.Instance.PlayerDeath();
            }
        }

    }

    public IEnumerator FlashBlue(float invinciblePeriod)
    {
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
        if (isInvincible)
        {
            sprite.color = Color.blue;

        }
        else
        {
            StartCoroutine(FlashBlue(0.1f));
        }

    }

    private void OnDestroy()
    {
       AudioManager.Instance.Play("Explosion_Player");
    }






}
