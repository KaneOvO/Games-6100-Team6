using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRb;
    AudioSource playerAudio;
    Ship ship;

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
    }

    void MovementInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
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
}