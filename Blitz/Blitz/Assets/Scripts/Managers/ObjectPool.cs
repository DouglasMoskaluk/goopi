using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;
    public static ObjectPool Instance { get { return instance; } }

    [SerializeField] private GameObject poolableObject;
    [SerializeField] private int pooledAmount = 20;
    [SerializeField] private bool willGrow = true;

    public List<GameObject> pooledObjects;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i=0; i<pooledAmount; i++)
        {
            GameObject obj = Instantiate(poolableObject);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i=0; i<pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            Debug.Log("New object added to the pool for " + poolableObject.name + ", for a total count of "+pooledObjects.Count);
            GameObject obj = Instantiate(poolableObject);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            pooledObjects.Add(obj);
            return obj;
        }

        Debug.LogWarning("Object pool for "+poolableObject.name+" doesn't have enough objects!!");
        return null;
    }

    public void disableAll()
    {
        for (int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
