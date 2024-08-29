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
    public Rigidbody2D hookRb;
    private bool isRetracting = false;
    private bool moveToTarget = false;
    [SerializeField] GameObject target;
    public GameObject pivot;
    public Rigidbody2D playerRb;
    Renderer spriteRenderer;
    Collider2D hookCollider;
    float distanceToTarget;

    void Awake()
    {
        spriteRenderer = GetComponent<Renderer>();
        hookCollider = GetComponent<Collider2D>();
        hookRb = GetComponent<Rigidbody2D>();
        hookHolder = GameObject.FindWithTag("Player");
        pivot = GameObject.FindWithTag("Pivot");
        playerRb = hookHolder.GetComponent<Rigidbody2D>();
        hideHook();
        
    }

    // void Update()
    // {
    //     distance = Vector3.Distance(pivot.transform.position, transform.position);

    //     if (distance >= maxDistance || isRetracting)
    //     {
    //         retractHook();
    //     }

    //     if (Input.GetKeyUp(KeyCode.DownArrow))
    //     {
    //         if (hookHolder.GetComponent<Ship>().isShootGrapple && !hookHolder.GetComponent<Ship>().isGrappling)
    //         {
    //             isRetracting = true;
    //         }
    //         else if (hookHolder.GetComponent<Ship>().isShootGrapple && hookHolder.GetComponent<Ship>().isGrappling)
    //         {
    //             hookHolder.GetComponent<Ship>().grappleObject.GetComponent<AsteroidMovement>().isFreezen = false;
    //             hookHolder.GetComponent<Ship>().isGrappling = false;
    //             hookHolder.GetComponent<Ship>().grappleObject = null;
    //             hookHolder.GetComponent<Attack>().Damage = 0;
    //             moveToTarget = false;
    //             isRetracting = true;
    //             return;
    //         }

    //     }

    //     if (moveToTarget)
    //     {
    //         if (hookHolder.GetComponent<Ship>().grappleObject == null)
    //         {
    //             hookHolder.GetComponent<Ship>().isGrappling = false;
    //             hookHolder.GetComponent<Ship>().grappleObject = null;
    //             hookHolder.GetComponent<Attack>().Damage = 0;
    //             moveToTarget = false;
    //             isRetracting = true;
    //             return;
    //         }

    //         hookHolder.transform.position = Vector2.Lerp(hookHolder.transform.position, transform.position, Time.deltaTime * launchSpeed);
    //     }
    // }

    void Update()
    {
        distance = Vector3.Distance(pivot.transform.position, transform.position);

        if (distance >= maxDistance || isRetracting)
        {
            retractHook();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Debug.Log("Down Arrow Up");
            if (hookHolder.GetComponent<Ship>().isShootGrapple && !hookHolder.GetComponent<Ship>().isGrappling)
            {
                Debug.Log("Down Arrow Up, and isShootGrapple is true, and isGrappling is false");
                isRetracting = true;
                return;
            }
            else if (hookHolder.GetComponent<Ship>().isShootGrapple && hookHolder.GetComponent<Ship>().isGrappling)
            {
                Debug.Log("Down Arrow Up, and isShootGrapple is true, and isGrappling is true");
                hookHolder.GetComponent<Ship>().grappleObject.GetComponent<AsteroidMovement>().isFreezen = false;
                hookHolder.GetComponent<Ship>().isGrappling = false;
                hookHolder.GetComponent<Ship>().grappleObject = null;
                playerRb.velocity = Vector2.zero;
                moveToTarget = false;
                
                return;
            }
        }

        if (moveToTarget)
        {
            Debug.Log("Moving to target");
            Debug.Log("Grapple object: " + hookHolder.GetComponent<Ship>().grappleObject);
            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                Debug.Log("Grapple object is null");
                hookHolder.GetComponent<Ship>().isGrappling = false;
                hookHolder.GetComponent<Ship>().grappleObject = null;
                moveToTarget = false;
                return;
            }
        }


    }

    public void shootHook()
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
            hideHook();
        }
    }

    void OnBecameInvisible()
    {
        isRetracting = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isRetracting)
        {
            distanceToTarget = Vector2.Distance(hookHolder.transform.position, other.transform.position);
            DetermineLaunchSpeed();
            Debug.Log("Distance to target: " + distanceToTarget);
            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                hookRb.velocity = Vector2.zero;
                moveToTarget = true;
                hookHolder.GetComponent<Ship>().isGrappling = true;
                target = other.gameObject;
                hookHolder.GetComponent<Ship>().grappleObject = target;

                AsteroidMovement asteroidMovement = other.gameObject.GetComponent<AsteroidMovement>();
                if (asteroidMovement != null)
                {
                    asteroidMovement.isFreezen = true;
                }

                LaunchPlayer();
            }
        }
    }

    void LaunchPlayer()
    {
        Vector2 launchDirection = (transform.position - hookHolder.transform.position).normalized;

        Rigidbody2D playerRb = hookHolder.GetComponent<Rigidbody2D>();
        playerRb.velocity = launchDirection * launchSpeed;
        isRetracting = true;
        
    }

    void hideHook()
    {
        spriteRenderer.enabled = false;
        hookCollider.enabled = false;
    }

    public void showHook()
    {
        spriteRenderer.enabled = true;
        hookCollider.enabled = true;
        shootHook();
    }

    void DetermineLaunchSpeed()
    {
        launchSpeed = distanceToTarget + 1;
    }


}