using UnityEngine;

public class ParticleFollow : MonoBehaviour
{
    public Rigidbody2D targetRigidbody;  // The Rigidbody2D of the target object
    public ParticleSystem particleSystem; // The separated particle system
    public float maxDistance = 2f;        // The maximum allowed distance, exceeding this will disable the particle system

    private Vector3 lastPosition;         // The last recorded position

    private void Start()
    {
        if (targetRigidbody != null)
        {
            lastPosition = targetRigidbody.transform.position;
        }
    }

    private void Update()
    {
        if (targetRigidbody != null && particleSystem != null)
        {
            // Update the particle system's position
            particleSystem.transform.position = targetRigidbody.transform.position;

            // Calculate the distance the object has moved since the last recorded position
            float distanceMoved = Vector3.Distance(lastPosition, targetRigidbody.transform.position);

            // If the distance moved exceeds the maximum allowed distance, stop the particle system
            if (distanceMoved > maxDistance)
            {
                particleSystem.Stop();
            }
            else
            {
                // If the distance is within the allowed range and the particle system is not playing, restart it
                if (!particleSystem.isPlaying)
                {
                    particleSystem.Play();
                }
            }

            // Update the last recorded position
            lastPosition = targetRigidbody.transform.position;
        }
    }
}
