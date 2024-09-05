using System.Collections;
using System.Collections.Generic;
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
    int invincibleCount = 0;
    [SerializeField] public float reactionForce = 1f;
    string targetTag;



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
                        var alienMovement = target.GetComponent<AlienMovement>();
                        if (alienMovement != null)
                        {
                            alienMovement.direction = direction;
                        }
                    }
                    else if (target.CompareTag("Enemy"))
                    {
                        var asteroidMovement = target.GetComponent<AsteroidMovement>();
                        if (asteroidMovement != null)
                        {
                            asteroidMovement.direction = direction;
                        }

                    }
                    else if (target.CompareTag("Planet"))
                    {
                        var planetMovement = target.GetComponent<PlanetMovement>();
                        if (planetMovement != null)
                        {
                            planetMovement.direction = direction;
                        }
                    }


                    distanceToTarget = Vector2.Distance(hookHolder.transform.position, launchTarget.transform.position);
                    DetermineLaunchSpeed();
                    //Debug.Log("Distance to target: " + distanceToTarget);
                    //Debug.Log("launch player");
                    LaunchPlayer();
                    GameManager.Instance.moveToTarget = true;
                    GameManager.Instance.playerAnimator.SetBool("IsEating", false);
                    GameManager.Instance.isGrappling = true;
                    SetInvincible(true);
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
            if (hookHolder.GetComponent<Ship>().grappleObject == null)
            {
                target = null;
                Debug.Log("Grapple object is null");
                GameManager.Instance.isGrappling = false;
                isHooked = false;
                hookHolder.GetComponent<Ship>().grappleObject = null;
                GameManager.Instance.moveToTarget = false;
                StartCoroutine(CallFunctionWithDelay(invinciblePeriod));
                GameManager.Instance.isRetracting = true;
                Debug.Log(targetTag);
                if(GameManager.Instance.player.GetComponent<Rigidbody2D>() != null && targetTag == "Enemy")
                {
                    GameManager.Instance.player.GetComponent<Rigidbody2D>().velocity = GameManager.Instance.player.GetComponent<Rigidbody2D>().velocity.normalized * reactionForce;
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
        if (!GameManager.Instance.isRetracting && (other.CompareTag("Enemy") || other.CompareTag("Alien") || other.CompareTag("Planet")))
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
                targetTag = target.tag;
                launchTarget = smallSphere;
                Debug.Log("fly target position: " + launchTarget.transform.position);


                hookRb.velocity = Vector2.zero;
                isHooked = true;
                hookHolder.GetComponent<Ship>().grappleObject = target;

                if (other.CompareTag("Alien"))
                {
                    if (other.GetComponent<AlienMovement>() != null)
                    {
                        other.GetComponent<AlienMovement>().speed /= 4;
                    }

                }
                else if (other.CompareTag("Enemy"))
                {
                    if (other.GetComponent<AsteroidMovement>() != null)
                    {
                        other.GetComponent<AsteroidMovement>().speed /= 2;
                    }

                }
                else if (other.CompareTag("Planet"))
                {
                    if (other.GetComponent<PlanetMovement>() != null)
                    {
                        other.GetComponent<PlanetMovement>().speed /= 2;
                    }

                    GameManager.Instance.playerAnimator.SetBool("IsEating", true);
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

    void SetInvincible(bool state)
    {
        if (state)
        {
            invincibleCount++;
            if (hookHolder != null)
            {
                hookHolder.GetComponent<Ship>().isInvincible = true;
            }
        }
        else
        {
            invincibleCount--;
            if (invincibleCount <= 0)
            {
                if (hookHolder != null)
                {
                    hookHolder.GetComponent<Ship>().isInvincible = false;
                }
                invincibleCount = 0;
            }
        }
    }

    IEnumerator CallFunctionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetInvincible(false);
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