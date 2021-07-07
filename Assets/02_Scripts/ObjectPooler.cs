using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler objectPoolerInstance;

    public List<ObjectPoolItem> itemsToPool;

    [HideInInspector] public List<GameObject> pooledObjects = new List<GameObject>();

    private void Awake()
    {
        objectPoolerInstance = this;
        CreateInitialPool();
    }

    public void CreateInitialPool()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.initialAmountInPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool, item.poolRoot);

                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }
        return null;
    }
}
