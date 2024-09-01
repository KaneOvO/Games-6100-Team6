using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] public float launchSpeed;
    [SerializeField] public float maxDistance;
    [SerializeField] public float invinciblePeriod = 1f;
    float distance;
    public GameObject hookHolder;
    public Rigidbody2D hookRb;
    //private bool isRetracting = false;
    public bool isHooked = false;
    //private bool moveToTarget = false;
    [SerializeField] GameObject target;
    public GameObject pivot;
    public Rigidbody2D playerRb;
    public Renderer spriteRenderer;
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

    void Update()
    {
        if (GameManager.Instance.isShootGrapple && !GameManager.Instance.inWorldScene(transform.position))
        {
            GameManager.Instance.isRetracting = true;
        }
        if (pivot != null)
        {
            distance = Vector3.Distance(pivot.transform.position, transform.position);
        }
        else
        {
            hideHook();
            return;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!isHooked)
            {
                GameManager.Instance.isRetracting = true;
            }
            else
            {
                if (target == null)
                {

                }
                else
                {
                    Debug.Log("launch player");
                    LaunchPlayer();
                    GameManager.Instance.moveToTarget = true;
                    isHooked = false;
                    GameManager.Instance.isGrappling = true;
                    hookHolder.GetComponent<Ship>().isInvincible = true;
                    StartCoroutine(hookHolder.GetComponent<Ship>().FlashBlue(invinciblePeriod));
                }
            }

        }


        if ((distance >= maxDistance && GameManager.Instance.isUsingHook()) || GameManager.Instance.isRetracting)
        {
            retractHook();
        }

        // if (Input.GetKeyUp(KeyCode.Space))
        // {
        //     if (hookHolder == null)
        //     {
        //         return;
        //     }
        //     Debug.Log("Down Arrow Up");
        //     if (hookHolder.GetComponent<Ship>().isShootGrapple && !hookHolder.GetComponent<Ship>().isGrappling)
        //     {
        //         Debug.Log("Down Arrow Up, and isShootGrapple is true, and isGrappling is false");
        //         GameManager.Instance.isRetracting = true;
        //         return;
        //     }
        //     else if (hookHolder.GetComponent<Ship>().isShootGrapple && hookHolder.GetComponent<Ship>().isGrappling)
        //     {
        //         Debug.Log("Down Arrow Up, and isShootGrapple is true, and isGrappling is true");
        //         hookHolder.GetComponent<Ship>().grappleObject.GetComponent<AsteroidMovement>().isFreezen = false;
        //         hookHolder.GetComponent<Ship>().isGrappling = false;
        //         hookHolder.GetComponent<Ship>().grappleObject = null;
        //         playerRb.velocity = Vector2.zero;
        //         moveToTarget = false;
        //         hookHolder.GetComponent<Ship>().isInvincible = false;
        //         GameManager.Instance.isRetracting = true;

        //         return;
        //     }
        // }

        // if(Input.GetKeyDown(KeyCode.Space) && hookHolder.GetComponent<Ship>().isShootGrapple)
        // {
        //     Debug.Log("Space key down");
        //     GameManager.Instance.isRetracting = true;
        // }

        if (GameManager.Instance.moveToTarget)
        {
            Debug.Log("Moving to target");
            Debug.Log("Grapple object: " + hookHolder.GetComponent<Ship>().grappleObject);
            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                Debug.Log("Grapple object is null");
                GameManager.Instance.isGrappling = false;
                hookHolder.GetComponent<Ship>().grappleObject = null;
                GameManager.Instance.moveToTarget = false;
                StartCoroutine(CallFunctionWithDelay(invinciblePeriod));
                GameManager.Instance.isRetracting = true;
                return;
            }
        }
        

    }

    public void shootHook()
    {
        GameManager.Instance.isRetracting = false;
        hookRb.velocity = transform.up * speed;
    }

    void retractHook()
    {
        if (hookHolder == null)
        {
            return;
        }

        GameManager.Instance.isRetracting = true;
        hookRb.velocity = Vector2.zero;
        hookRb.position = Vector2.MoveTowards(hookRb.position, pivot.transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(hookRb.position, pivot.transform.position) < 0.1f)
        {
            GameManager.Instance.isRetracting = false;
            GameManager.Instance.isShootGrapple = false;
            hideHook();
        }
    }

    //void OnBecameInvisible()
    //{
    //    GameManager.Instance.isRetracting = true;
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.Instance.isRetracting && (other.CompareTag("Enemy") || other.CompareTag("Alien")))
        {
            if (hookHolder == null)
            {
                return;
            }
            distanceToTarget = Vector2.Distance(hookHolder.transform.position, other.transform.position);
            DetermineLaunchSpeed();
            Debug.Log("Distance to target: " + distanceToTarget);
            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                hookRb.velocity = Vector2.zero;
                isHooked = true;
                target = other.gameObject;
                hookHolder.GetComponent<Ship>().grappleObject = target;

                if (target.CompareTag("Alien"))
                {
                    if (target.GetComponent<AlienMovement>() != null)
                    {
                        target.GetComponent<AlienMovement>().isFreezen = true;
                    }

                }
                else if (target.CompareTag("Enemy"))
                {
                    AsteroidMovement asteroidMovement = target.GetComponent<AsteroidMovement>();
                    if (asteroidMovement != null)
                    {
                        asteroidMovement.isFreezen = true;
                    }
                }
            }
        }
    }

    void LaunchPlayer()
    {
        Vector2 launchDirection = (transform.position - hookHolder.transform.position).normalized;

        Rigidbody2D playerRb = hookHolder.GetComponent<Rigidbody2D>();
        playerRb.velocity = launchDirection * launchSpeed;

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

    void SetInvincible()
    {
        if (hookHolder == null)
        {
            return;
        }
        hookHolder.GetComponent<Ship>().isInvincible = false;
    }

    IEnumerator CallFunctionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetInvincible();
    }



}