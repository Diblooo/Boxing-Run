using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public Transform poolRoot;
    public int initialAmountInPool;
    public GameObject objectToPool;
}
