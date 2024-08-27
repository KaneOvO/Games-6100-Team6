using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRb;
    AudioSource playerAudio;
    Ship ship;
    [SerializeField] GameObject missilePrefab;
    private float fireRate = 0.2f;
    private float nextFireTime = 0f;
    public GameObject hook;
    [SerializeField] GameObject hookPovit;

    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<Ship>();
        playerRb = GetComponent<Rigidbody2D>();
        playerRb.drag = ship.LinearDrag;
        //playerAudio = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        RotationInput();
        WrapAroundScreen();
        Fire();
        shootHook();
    }

    void MovementInput()
    {
        if (!ship.isShootGrapple && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            if(ship.isShootGrapple|| ship.isGrappling)
            {
                return;
            }
            StartBoost();
        }
        else
        {
            StopBoost();
        }
    }

    private void StartBoost()
    {
        playerRb.AddRelativeForce(Vector3.up * ship.Speed * Time.deltaTime);
        // if (!ship.BoostParticle.isPlaying)
        // {
        //     ship.BoostParticle.Play();
        // }

        // if (!playerAudio.isPlaying)
        // {
        //     playerAudio.PlayOneShot(ship.FlyAudio, 0.2f);
        // }
    }

    private void StopBoost()
    {
        //playerAudio.Stop();
        //ship.BoostParticle.Stop();
    }

    private void RotationInput()
    {
        if (!ship.isShootGrapple && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            ApplyRotation(Vector3.forward);
        }
        else if (!ship.isShootGrapple && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            ApplyRotation(Vector3.back);
        }
    }

    private void ApplyRotation(Vector3 direction)
    {
        playerRb.freezeRotation = true;
        transform.Rotate(direction * ship.Rotation * Time.deltaTime);
        playerRb.freezeRotation = false;
    }

    private void WrapAroundScreen()
    {
        Vector3 position = transform.position;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        if (viewportPosition.x < 0)
        {
            viewportPosition.x = 1;
        }
        else if (viewportPosition.x > 1)
        {
            viewportPosition.x = 0;
        }

        if (viewportPosition.y < 0)
        {
            viewportPosition.y = 1;
        }
        else if (viewportPosition.y > 1)
        {
            viewportPosition.y = 0;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition);
    }

    private void Fire()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            GameObject missile = MissilePool.SharedInstance.GetPooledObject();
            if (missile != null)
            {
                missile.transform.position = transform.position;
                missile.transform.rotation = transform.rotation;
                missile.SetActive(true);
            }
        }
    }

    void shootHook()
    {
        if(ship.isShootGrapple || ship.isGrappling)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !ship.isShootGrapple)
        {
            playerRb.velocity = Vector2.zero;
            Instantiate(hook, hookPovit.transform.position, hookPovit.transform.rotation);
            ship.isShootGrapple = true;
        }
        
    }
}