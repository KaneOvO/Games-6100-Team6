using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float launchSpeed;
    [SerializeField] float maxDistance;
    float distance;
    private GameObject hookHolder;
    Rigidbody2D hookRb;
    private bool isRetracting = false;
    private bool moveToTarget = false;
    [SerializeField] GameObject target;
    public GameObject pivot;

    void Awake()
    {
        hookRb = GetComponent<Rigidbody2D>();

        hookHolder = GameObject.FindWithTag("Player");
        pivot = GameObject.FindWithTag("Pivot");
    }

    void Start()
    {
        shootHook();
    }

    void Update()
    {
        distance = Vector3.Distance(pivot.transform.position, transform.position);

        if (distance >= maxDistance || isRetracting)
        {
            retractHook();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isRetracting = true;
        }

        if (moveToTarget)
        {
            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                hookHolder.GetComponent<Ship>().isGrappling = false;
                hookHolder.GetComponent<Ship>().grappleObject = null;
                hookHolder.GetComponent<Attack>().Damage = 0;
                moveToTarget = false;
                isRetracting = true;
                return;
            }

            hookHolder.transform.position = Vector2.Lerp(hookHolder.transform.position, transform.position, Time.deltaTime * launchSpeed);
        }
    }

    void shootHook()
    {
        isRetracting = false;
        hookRb.velocity = transform.up * speed;
    }

    void retractHook()
    {
        isRetracting = true;
        hookRb.velocity = Vector2.zero;
        hookRb.position = Vector2.MoveTowards(hookRb.position, pivot.transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(hookRb.position, pivot.transform.position) < 0.1f)
        {
            isRetracting = false;
            hookHolder.GetComponent<Ship>().isShootGrapple = false;
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        isRetracting = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isRetracting)
        {
            Debug.Log("Enemy hit");
            
            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                hookRb.velocity = Vector2.zero;
                moveToTarget = true;
                hookHolder.GetComponent<Ship>().isGrappling = true;
                target = other.gameObject;
                hookHolder.GetComponent<Ship>().grappleObject = target;
                hookHolder.GetComponent<Attack>().Damage = 1;

                AsteroidMovement asteroidMovement = other.gameObject.GetComponent<AsteroidMovement>();
                if (asteroidMovement != null)
                {
                    asteroidMovement.isFreezen = true;
                }
            }
        }
    }
}