using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownRagdollSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject playerCrown;

    void Start()
    {
        
    }

    public void CrownSpawn()
    {
        if(playerCrown.activeInHierarchy)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
