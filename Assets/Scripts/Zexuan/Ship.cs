using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Item
{
    [SerializeField] float speed;
    [SerializeField] float rotation;
    [SerializeField] float linearDrag;
    [SerializeField] ParticleSystem boostParticle;
    [SerializeField] ParticleSystem collisionParticle;
    
    // 无敌时间（秒）
    private float invincibilityDuration = 1.0f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    public float Speed
    {
        get { return speed; }
    }

    public float Rotation
    {
        get { return rotation; }
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
        // 初始化生命值
        if (currenthealth <= 0)
        {
            currenthealth = health;
        }
        
        // 获取SpriteRenderer组件（用于无敌时的闪烁效果）
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 订阅玩家伤害事件
        PlayerDamageEvent.OnPlayerDamaged += HandlePlayerDamage;
    }

    private void OnDestroy()
    {
        // 取消订阅事件
        PlayerDamageEvent.OnPlayerDamaged -= HandlePlayerDamage;
    }

    // 处理玩家伤害事件（观察者模式）
    private void HandlePlayerDamage(object sender, PlayerDamageEventArgs e)
    {
        // 如果处于无敌状态，忽略伤害
        if (isInvincible)
        {
            return;
        }

        // 造成伤害
        currenthealth -= e.Damage;
        Debug.Log($"玩家受到 {e.Damage} 点伤害，当前生命值: {currenthealth}/{health}");

        // 检查是否死亡
        if (currenthealth <= 0)
        {
            currenthealth = 0;
            Debug.Log("玩家死亡");
            GameManager.Instance.GameOver();
            Destroy(gameObject);
            return;
        }

        // 启动无敌时间
        StartCoroutine(InvincibilityCoroutine());
    }

    // 无敌时间协程
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsedTime = 0f;
        float blinkInterval = 0.1f; // 闪烁间隔
        bool visible = true;

        while (elapsedTime < invincibilityDuration)
        {
            // 闪烁效果
            if (spriteRenderer != null)
            {
                visible = !visible;
                spriteRenderer.enabled = visible;
            }

            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        // 恢复可见
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isInvincible = false;
    }
}
