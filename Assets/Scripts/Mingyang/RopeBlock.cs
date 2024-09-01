using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBlock : Item
{
    [SerializeField] float ropeGap;
    [SerializeField] float hookRopePadding;
    [SerializeField] float shipRopePadding;
    private int index;
    private GameObject ship;
    private GameObject hook;
    Renderer spriteRenderer;
    public float ropeLength;
    public float test;

    // Start is called before the first frame update

    void Awake()
    {
        spriteRenderer = GetComponent<Renderer>();
        spriteRenderer.enabled = false;
        ship = GameManager.Instance.player;
        hook = GameManager.Instance.hook;
        index = GameManager.Instance.ropeBlockCount;
        ropeLength = RopeLength();
        UpdateTransform(ropeLength);
    }

    void Start()
    {
    }

    private void Update()
    {
        if (!GameManager.Instance.isUsingHook())
        {
            spriteRenderer.enabled = false;
            return;
        }
        ropeLength = RopeLength();
        if (ropeLength / ropeGap < index)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        UpdateTransform(ropeLength);
    }

    private void UpdateTransform(float ropeLength)
    {
        float padding = 0;
        if (GameManager.Instance.moveToTarget)
        {
            transform.SetPositionAndRotation(hook.transform.position, ship.transform.rotation);
            transform.up = -transform.up;
            padding = hookRopePadding;
        }
        else
        {
            transform.SetPositionAndRotation(ship.transform.position, ship.transform.rotation);
            padding = shipRopePadding;
        }
        transform.position += transform.up * (index * ropeGap + padding);
    }

    private float RopeLength()
    {
        float distance = Vector3.Distance(hook.transform.position, ship.transform.position);
        return distance - shipRopePadding - hookRopePadding;
    }

    private float ExtraPadding(float ropeLength)
    {
        return ropeLength % ropeGap;
    }

}
