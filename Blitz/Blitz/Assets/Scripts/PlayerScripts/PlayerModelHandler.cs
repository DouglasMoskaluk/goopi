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

    //[SerializeField]
    //private Material[] characterMaterial;

    [SerializeField]
    private Material[] OtterBodyMaterials;

    [SerializeField]
    private Material[] OtterHeadMaterials;

    [SerializeField]
    private Material[] FoxBodyMaterials;

    [SerializeField]
    private Material[] FoxHeadMaterials;

    [SerializeField]
    private Material[] LizBodyMaterials;

    [SerializeField]
    private Material[] AxoBodyMaterials;

    [SerializeField]
    private Material[] BadgerBodyMaterials;

    [SerializeField]
    private Material[] BadgerHeadMaterials;

    [SerializeField]
    private Material[] WhaleBodyMaterials;

    public int skinNum = 0;

    private void Update()
    {
        
    }

    public void GetSkinNum()
    {

    }

    public void SetModelID(int featureNum)
    {
        transform.GetComponent<PlayerBodyFSM>().modelID = featureNum;
    }

    public void SetModel(int featureNum)
    {

        for (int i = 0;i < headFeatures.Length;i++)
        {
            headFeatures[i].SetActive(false);
        }

        headFeatures[featureNum].SetActive(true);
        playerSkin.sharedMesh = characterMesh[featureNum];


        switch (featureNum)
        {
            case 0:
                playerSkin.sharedMaterial = OtterBodyMaterials[skinNum];
                headFeatures[featureNum].transform.GetChild(3).GetComponent<MeshRenderer>().sharedMaterial = OtterHeadMaterials[skinNum];
                break;
            case 1:
                playerSkin.sharedMaterial = FoxBodyMaterials[skinNum];
                headFeatures[featureNum].transform.GetChild(3).GetComponent<MeshRenderer>().sharedMaterial = FoxHeadMaterials[skinNum];
                break;
            case 2:
                playerSkin.sharedMaterial = LizBodyMaterials[skinNum];
                break;
            ; case 3:
                playerSkin.sharedMaterial = AxoBodyMaterials[skinNum];
                break;
            case 4:
                playerSkin.sharedMaterial = BadgerBodyMaterials[skinNum];
                headFeatures[featureNum].transform.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial = BadgerHeadMaterials[skinNum];
                break;
            case 5:
                playerSkin.sharedMaterial = WhaleBodyMaterials[skinNum];
                break;

        }

        //for(int i = 0; i < headFeatures.Length; i++)
        //{

        //    if (transform.GetComponent<PlayerBodyFSM>())
        //    {
        //        transform.GetComponent<PlayerBodyFSM>().modelID = featureNum;
        //    }

        //    if (featureNum == i)
        //    {
        //        headFeatures[i].SetActive(true);
        //        playerSkin.sharedMaterial = characterMaterial[i];
        //    }
        //    else
        //    {
        //        headFeatures[i].SetActive(false);
        //    }
        //}

    }
}
