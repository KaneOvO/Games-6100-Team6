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
    public bool isWrapping = false;
    //private bool moveToTarget = false;
    [SerializeField] public GameObject target;
    [SerializeField] public GameObject launchTarget;
    public GameObject pivot;
    public Rigidbody2D playerRb;
    public Renderer spriteRenderer;
    Collider2D hookCollider;
    float distanceToTarget;
    [SerializeField] float rotationSpeed = 360f;
    public float distanceOffset = 0.5f;
    public GameObject circle;



    void Awake()
    {


    }

    void Start()
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

        string targetTag = target != null ? target.tag : "null";

        isWrapping = false;
        if (hookHolder != null && target != null && target.GetComponent<AsteroidMovement>() != null)
        {
            isWrapping = target.GetComponent<AsteroidMovement>().isWrapping;
        }
        else if (hookHolder != null && target != null && target.GetComponent<PlanetMovement>() != null)
        {
            isWrapping = !GameManager.Instance.inWorldScene(target.transform.position);
        }

        if (hookHolder != null && target != null && !isWrapping)
        {
            //Debug.Log("Move with: " + target);
            hookRb.position = Vector2.MoveTowards(hookRb.position, launchTarget.transform.position, speed * Time.deltaTime);
            RotateShipTowardsTarget();
        }
        else if (hookHolder != null && target != null && isWrapping)
        {
            Debug.Log("Target is not in scene");
            target = null;
            launchTarget = null;
            hookHolder.GetComponent<Ship>().grappleObject = null;
            isHooked = false;
            GameManager.Instance.isRetracting = true;
        }

        if (isHooked && target == null)
        {
            hookHolder.GetComponent<Ship>().grappleObject = null;
            isHooked = false;
            GameManager.Instance.isRetracting = true;
        }
        if (isHooked && GameManager.Instance.player == null)
        {
            isHooked = false;
            target = null;
            launchTarget = null;
            GameManager.Instance.moveToTarget = false;
            GameManager.Instance.isShootGrapple = false;
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!isHooked)
            {
                GameManager.Instance.playerAnimator.SetBool("IsEating", false);
                GameManager.Instance.isRetracting = true;
            }
            else
            {
                if (target == null)
                {
                    hookHolder.GetComponent<Ship>().grappleObject = null;
                    isHooked = false;
                    GameManager.Instance.playerAnimator.SetBool("IsEating", false);
                    GameManager.Instance.isRetracting = true;
                    return;
                }
                else
                {
                    Vector2 direction = (GameManager.Instance.player.transform.position - target.transform.position).normalized;
                    if (target.CompareTag("Alien"))
                    {

                        target.GetComponent<AlienMovement>().direction = direction;
                    }
                    else if (target.CompareTag("Enemy"))
                    {
                        target.GetComponent<AsteroidMovement>().direction = direction;
                    }
                    else if (target.CompareTag("Planet"))
                    {
                        target.GetComponent<PlanetMovement>().direction = direction;
                    }


                    distanceToTarget = Vector2.Distance(hookHolder.transform.position, launchTarget.transform.position);
                    DetermineLaunchSpeed();
                    //Debug.Log("Distance to target: " + distanceToTarget);
                    //Debug.Log("launch player");
                    LaunchPlayer();
                    GameManager.Instance.moveToTarget = true;
                    GameManager.Instance.playerAnimator.SetBool("IsEating", false);
                    GameManager.Instance.isGrappling = true;
                    hookHolder.GetComponent<Ship>().isInvincible = true;
                    AudioManager.Instance.Play("Launch");
                }
            }

        }

        if (!GameManager.Instance.isShootGrapple)
        {
            if (GameManager.Instance.player != null)
            {
                transform.position = hookHolder.transform.position + (Vector3)hookHolder.transform.up * distanceOffset;
                transform.rotation = hookHolder.transform.rotation;
            }


        }


        if ((distance >= maxDistance && GameManager.Instance.isShootGrapple && !isHooked) || GameManager.Instance.isRetracting)
        {
            retractHook();
        }

        if (GameManager.Instance.moveToTarget)
        {
            //Debug.Log("Grapple object: " + hookHolder.GetComponent<Ship>().grappleObject);
            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                Debug.Log("Grapple object is null");
                GameManager.Instance.isGrappling = false;
                isHooked = false;
                hookHolder.GetComponent<Ship>().grappleObject = null;
                GameManager.Instance.moveToTarget = false;
                StartCoroutine(CallFunctionWithDelay(invinciblePeriod));
                GameManager.Instance.isRetracting = true;
                if (targetTag == "Plannet")
                {
                    GameManager.Instance.playerAnimator.SetTrigger("ToCloseMouth");
                }
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
        if (!GameManager.Instance.isRetracting && (other.CompareTag("Enemy") || other.CompareTag("Alien") || other.CompareTag("Planet")) || other.CompareTag("StartUI"))
        {
            if (hookHolder == null)
            {
                return;
            }

            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                Vector2 worldPosition = transform.position;
                Vector2 localPosition = other.transform.InverseTransformPoint(worldPosition);

                GameObject smallSphere = Instantiate(circle, localPosition, Quaternion.identity);
                smallSphere.transform.SetParent(other.transform);
                smallSphere.transform.localPosition = localPosition;

                target = other.gameObject;
                launchTarget = smallSphere;
                Debug.Log("fly target position: " + launchTarget.transform.position);


                hookRb.velocity = Vector2.zero;
                isHooked = true;
                hookHolder.GetComponent<Ship>().grappleObject = target;

                if (other.CompareTag("Alien"))
                {
                    var alienMovement = other.GetComponent<AlienMovement>();

                    if (alienMovement != null)
                    {
                        alienMovement.speed /= 2;
                    }

                }
                else if (other.CompareTag("Enemy"))
                {
                    var asteroidMovement = other.GetComponent<AsteroidMovement>();

                    if (asteroidMovement != null)
                    {
                        asteroidMovement.speed /= 2;
                    }

                }
                else if (other.CompareTag("Planet"))
                {
                    var planetMovement = other.GetComponent<PlanetMovement>();

                    if (planetMovement != null)
                    {
                        planetMovement.speed /= 2;
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
        //spriteRenderer.enabled = false;
        hookCollider.enabled = false;
    }

    public void showHook()
    {
        //spriteRenderer.enabled = true;
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

    void RotateShipTowardsTarget()
    {
        if (hookHolder == null || target == null)
            return;

        Vector2 direction = (launchTarget.transform.position - hookHolder.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float currentAngle = hookHolder.transform.eulerAngles.z;

        float targetAngle = Mathf.MoveTowardsAngle(currentAngle, angle, rotationSpeed * Time.deltaTime);

        hookHolder.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }



}