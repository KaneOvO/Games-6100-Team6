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
    public bool isGrappling = false;
    public GameObject grappleObject;

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
        if (attacker.CompareTag("Enemy"))
        {
            if (isGrappling)
            {
                if (attacker.gameObject == grappleObject)
                {
                    Destroy(grappleObject);
                    isGrappling = false;
                    grappleObject = null;
                    return;
                }
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






}
