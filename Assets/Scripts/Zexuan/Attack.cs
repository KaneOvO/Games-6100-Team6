using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Item>()?.TakeDamage(this);
    }
}
