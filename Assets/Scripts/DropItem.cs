using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : Item
{
    [SerializeField] float lifeTime = 10f;

    private void Start()
    {
        Invoke(nameof(DestroySelf), lifeTime);
    }

    public override void OnAllyBuff(Attack attacker)
    {
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
