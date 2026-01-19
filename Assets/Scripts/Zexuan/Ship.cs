using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Item
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float linearDrag;
    [SerializeField] ParticleSystem boostParticle;
    [SerializeField] ParticleSystem collisionParticle;

    private float invincibilityDuration = 1.0f;
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

        PlayerDamageEvent.OnPlayerDamaged += HandlePlayerDamage;
    }

    private void OnDestroy()
    {
        PlayerDamageEvent.OnPlayerDamaged -= HandlePlayerDamage;
    }

    private void HandlePlayerDamage(object sender, PlayerDamageEventArgs e)
    {
        if (isInvincible)
        {
            return;
        }

        currenthealth -= e.Damage;

        if (currenthealth <= 0)
        {
            currenthealth = 0;
            Debug.Log("玩家死亡");
            GameManager.Instance.GameOver();
            Destroy(gameObject);
            return;
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    // 无敌时间协程
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
