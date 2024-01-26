using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpineWithCamera : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform rotBone;
    [SerializeField] private Transform forwardDirectionTransform;
    [SerializeField] private Animator anim;

    private Vector3 initCamRot;
    [Range(0,1)] public float angleTest = 0.5f;

    private void Awake()
    {
        initCamRot = cam.localEulerAngles;
    }

    private void LateUpdate() { 
    
        anim.Play("SpineRotate", 2, angleTest);
    }
}

[Serializable]
public class WeightedBones
{
    public float weight = 0;
    public Transform bone;
    internal Quaternion initRot;

}