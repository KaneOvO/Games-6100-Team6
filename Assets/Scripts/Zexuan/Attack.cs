using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        other.gameObject.GetComponent<Item>()?.TakeDamage(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Item>()?.TakeDamage(this);
    }

    
}
