using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float parallaxFactor = 0.5f;
    public float moveSpeed = 2.0f;

    private Vector3 previousPlayerPosition;
    private bool isMovingToZero = false;

    void Start()
    {
        previousPlayerPosition = GameManager.Instance.player.transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.player == null && !isMovingToZero)
        {
            StartCoroutine(MoveToZero());
            return;
        }

        if (GameManager.Instance.player != null)
        {
            Vector3 delta = GameManager.Instance.player.transform.position - previousPlayerPosition;
            transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);
            previousPlayerPosition = GameManager.Instance.player.transform.position;
        }
    }

    IEnumerator MoveToZero()
    {
        isMovingToZero = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = Vector3.zero;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = Vector3.zero;
        isMovingToZero = false;
    }
}