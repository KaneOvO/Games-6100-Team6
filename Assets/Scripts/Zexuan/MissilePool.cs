using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePool : MonoBehaviour
{
    public static MissilePool SharedInstance { get; private set; }
    public List<GameObject> pooledObjects;
    public GameObject missileToPool;
    public int amountToPool;

    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject tmp = Instantiate(missileToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
            tmp.transform.parent = transform;
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
    
    public void ResetPool()
    {
        foreach (GameObject obj in pooledObjects)
        {
            obj.SetActive(false);
        }
    }
}
