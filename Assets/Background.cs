using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float parallaxFactor = 0.5f;

    private Vector3 previousPlayerPosition;

    void Start()
    {
        previousPlayerPosition = GameManager.Instance.player.transform.position;
    }

    void Update()
    {
        if(GameManager.Instance.player == null)
        {
            transform.position = new Vector3(0, 0, 0);
            return;
        }
        Vector3 delta = GameManager.Instance.player.transform.position - previousPlayerPosition;

        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);

        previousPlayerPosition = GameManager.Instance.player.transform.position;
    }
}
