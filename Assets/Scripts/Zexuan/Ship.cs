using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ship : Item
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float linearDrag;
    [SerializeField] ParticleSystem boostParticle;
    [SerializeField] ParticleSystem collisionParticle;
    public static event Action<int> OnPlayerDamaged;
    [SerializeField] float invincibilityDuration = 1.0f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    public float Speed
    {
        get { return movementSpeed; }
    }

    public float Rotation
    {
        get { return rotationSpeed; }
    }

    public float LinearDrag
    {
        get { return linearDrag; }
    }

    public ParticleSystem BoostParticle
    {
        get { return boostParticle; }
    }

    private void Start()
    {
        if (currenthealth <= 0)
        {
            currenthealth = health;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnDestroy()
    {
        
    }

    public override void OnEnemyDamage(Attack attacker)
    {
        if (isInvincible)
        {
            return;
        }

        currenthealth -= attacker.Damage;

        OnPlayerDamaged?.Invoke(currenthealth);

        if (currenthealth <= 0)
        {
            currenthealth = 0;
            GameManager.Instance.GameOver();
            Destroy(gameObject);
            return;
        }
        
        StartCoroutine(InvincibilityCoroutine());
    }

    public override void OnAllyBuff(Attack attacker)
    {
        
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsedTime = 0f;
        float blinkInterval = 0.1f;
        bool visible = true;

        while (elapsedTime < invincibilityDuration)
        {
            if (spriteRenderer != null)
            {
                visible = !visible;
                spriteRenderer.enabled = visible;
            }

            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isInvincible = false;
    }
}
