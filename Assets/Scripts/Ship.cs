using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ship : Item
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float linearDrag;    public static event Action<int> OnPlayerDamaged;
    [SerializeField] float invincibilityDuration = 1.0f;
    [SerializeField] float shieldInvincibilityDuration = 3.0f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer shieldSprite;

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
    private void Start()
    {
        if (currenthealth <= 0)
        {
            currenthealth = health;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        shieldSprite = transform.Find("shield_white").GetComponent<SpriteRenderer>();
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
        if(attacker.CompareTag("Heart"))
        {
            if(currenthealth < health)
            {
                currenthealth -= attacker.Damage;
            }
            OnPlayerDamaged?.Invoke(currenthealth);
        }
        else if(attacker.CompareTag("Shield"))
        {
            StartCoroutine(ShieldCoroutine());
        }
        PlayPowerUpAudio();
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

    private IEnumerator ShieldCoroutine()
    {
        isInvincible = true;
        shieldSprite.enabled = true;
        yield return new WaitForSeconds(shieldInvincibilityDuration);
        shieldSprite.enabled = false;
        isInvincible = false;
    }

    private void PlayPowerUpAudio()
    {
        AudioManager.Instance.PlaySound(UnityEngine.Random.Range(3, 6));
    }
}
