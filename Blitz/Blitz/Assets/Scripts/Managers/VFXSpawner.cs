using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSpawner : MonoBehaviour
{
    public static VFXSpawner instance;
    [SerializeField] private GameObject[] spawnObjects;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public GameObject spawnObject(VFXSpawnerObjects objToSpawn)
    {
        return Instantiate(spawnObjects[(int)objToSpawn]);
    }

}

public enum VFXSpawnerObjects
{
    smoke
}
