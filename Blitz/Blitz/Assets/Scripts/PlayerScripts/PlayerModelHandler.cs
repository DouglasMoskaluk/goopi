using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelChanger : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer playerSkin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetModel(Mesh newMesh, Material newMaterial)
    {
        playerSkin.sharedMesh = newMesh;
        playerSkin.sharedMaterial = newMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
