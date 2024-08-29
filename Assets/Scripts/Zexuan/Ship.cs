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
    public bool isConsumption = false;
    public bool isGrappling = false;
    public bool isShootGrapple = false;
    public GameObject grappleObject;
    public bool isInvincible = false;

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

    public override void TakeDamage(Attack attacker)
    {
        if (attacker.CompareTag("Enemy") || attacker.CompareTag("Alien"))
        {
            if (isGrappling || isInvincible)
            {
                return;
            }
            currenthealth -= attacker.Damage;
            if (currenthealth <= 0)
            {
                Destroy(gameObject);
                GameManager.Instance.GameOver();
                Debug.Log("Ship destroyed");
            }
        }

    }

    void Update()
    {
        if(isInvincible)
        {
            gameObject.GetComponent<Attack>().Damage = 1;
        }
        else
        {
            gameObject.GetComponent<Attack>().Damage = 0;
        }
    }






}
