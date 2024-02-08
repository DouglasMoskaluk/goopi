using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerModelHandler : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer playerSkin;

    [SerializeField]
    private GameObject[] headFeatures;

    [SerializeField]
    private Mesh[] characterMesh;

    [SerializeField]
    private Material[] characterMaterial;

    [SerializeField]
    private Material[] OtterMaterials;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetModel(int featureNum)
    {

        for(int i = 0; i < headFeatures.Length; i++)
        {

            if (transform.GetComponent<PlayerBodyFSM>())
            {
                transform.GetComponent<PlayerBodyFSM>().modelID = featureNum;
            }

            if (featureNum == i)
            {
                headFeatures[i].SetActive(true);
                playerSkin.sharedMesh = characterMesh[i];
                playerSkin.sharedMaterial = characterMaterial[i];
            }
            else
            {
                headFeatures[i].SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
