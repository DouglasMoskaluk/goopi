using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class PlayerCharSelectAnims : MonoBehaviour
{

    private Vector3[] bonePos;

    private Vector3[] boneRot;

    private Transform[] bones;

    [SerializeField]
    private RuntimeAnimatorController playModeController;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Transform headBone;

    private Rig rigOne;

    private Rig rigTwo;

    private Rig rigThree;

    private CameraShake cam;

    private bool isEnding = false;

    private PlayerEyeBehaviour playerEyeBehaviour;

    // Start is called before the first frame update
    void Awake()
    {
        bones = GetComponent<BoneRenderer>().transforms;

        bonePos = new Vector3[bones.Length];
        boneRot = new Vector3[bones.Length];

        for (int i = 0; i < bones.Length; i++)
        {
            bonePos[i] = bones[i].position;
            boneRot[i] = bones[i].rotation.eulerAngles;
        }
    }

    void Start()
    {
        cam = transform.root.GetComponent<CameraShake>();
        rigOne = transform.GetChild(2).GetComponent<Rig>();
        rigTwo = transform.GetChild(3).GetComponent<Rig>();
        rigThree = transform.GetChild(4).GetComponent<Rig>();
        playerEyeBehaviour = transform.root.GetComponent<PlayerEyeBehaviour>();

    }

    public void SetEyes(int eyeType)
    {
        if(eyeType == 0)
        {
            playerEyeBehaviour.SetDefault();
        }
        else if(eyeType == 1)
        {
            playerEyeBehaviour.SetWince();
        }
        else if (eyeType == 2)
        {
            playerEyeBehaviour.SetHappy();
        }
        else if (eyeType == 3)
        {
            playerEyeBehaviour.SetBlink();
        }
    }

    public void ResetScale()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].localScale = Vector3.one;
        }
    }

    public void StartSquash()
    {
        if(!isEnding)
        {
            playerEyeBehaviour.SetEyeFeatures();
            anim.Play("Base Layer.PlayerSquash", 0, 0);
        }
    }

    public void UnSquash()
    {
        if(!isEnding)
        {
            anim.SetTrigger("UnSquash");
        }
    }

    IEnumerator StartPlayModeCoro()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].position = bonePos[i];
            bones[i].rotation = Quaternion.Euler(boneRot[i].x, boneRot[i].y, boneRot[i].y);
            bones[i].localScale = Vector3.one;
        }

        headBone.localPosition = new Vector3(4.01941223e-11f, 0.00105899118f, -2.79396766e-11f);
        headBone.localRotation = Quaternion.Euler(27.042635f, 356.426666f, 359.632568f);

        Debug.Log("swap anim");

        anim.runtimeAnimatorController = playModeController;

        yield return null;

        rigOne.weight = 1.0f;
        rigTwo.weight = 1.0f;
        rigThree.weight = 0.0f;

        cam.CharSelectRotateCamera(300);

    }

    public void StartPlayMode()
    {
        StartCoroutine(StartPlayModeCoro());


    }

    public void SetBones()
    {
        isEnding = true;
        anim.Play("Ready", 0, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
