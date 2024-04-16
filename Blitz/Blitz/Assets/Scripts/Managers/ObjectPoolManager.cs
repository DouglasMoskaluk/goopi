using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public enum PoolTypes { TNTRain }

    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance { get { return instance; } }

    [SerializeField]
    private ObjectPool[] pools;

    public GameObject GetPooledObject(PoolTypes type)
    {
        return pools[(int)type].GetPooledObject();
    }

    public void DisablePool(PoolTypes type)
    {
        pools[(int)type].disableAll();
    }

    private void Awake()
    {
        instance = this;
    }
}
