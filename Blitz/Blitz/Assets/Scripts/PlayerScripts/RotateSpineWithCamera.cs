using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpineWithCamera : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform rotBone;
    [SerializeField] private Transform forwardDirectionTransform;


    private Vector3 initCamRot;

    private void Start()
    {
        initCamRot = cam.localEulerAngles;
    }

    private void LateUpdate()
    {
        float XCamRotDiff = cam.eulerAngles.x - initCamRot.x;

        Vector3 newRot = rotBone.eulerAngles;
        newRot.x = initCamRot.x + XCamRotDiff;
        rotBone.localEulerAngles = newRot;
    }

    //[SerializeField] private Transform cam;
    ////[SerializeField] private Transform forwardDirectionRot;
    //[SerializeField] private float upClamp = 25f;
    //[SerializeField] private float downClamp = 25f;
    //private Vector3 camInitEuler;
    //private Quaternion camInitRot;

    //[SerializeField] private List<WeightedBones> RotationBones;

    //private void Awake()
    //{
    //    camInitEuler = cam.localEulerAngles;
    //    camInitRot = cam.localRotation;
    //    SetUpBones();
    //}

    //private void LateUpdate()
    //{

    //    float camXRotDiff = cam.eulerAngles.x - camInitEuler.x;
    //    Debug.Log(camXRotDiff);
    //    foreach (var rotBone in RotationBones)
    //    {
    //        rotBone.bone.localRotation = rotBone.initRot * Quaternion.Euler((camXRotDiff * rotBone.weight), 0, 0);
    //        rotBone.bone.localEulerAngles = ClampX(rotBone.bone.localEulerAngles, downClamp, upClamp);
    //    }
    //}

    //private void SetUpBones()
    //{
    //    float totalWeight = 0;
    //    foreach (var rotBone in RotationBones)
    //    {
    //        totalWeight += rotBone.weight;
    //    }

    //    foreach (var rotBone in RotationBones)
    //    {
    //        rotBone.weight = rotBone.weight / totalWeight;
    //        rotBone.initRot = rotBone.bone.localRotation;
    //    }
    //}

    //private Vector3 ClampX(Vector3 euler, float downClamp, float upClamp)
    //{
    //    if (euler.x > upClamp) { euler.x = upClamp; }

    //    return euler;
    //}
}

public enum ClampAxis
{
    X = 0,
    Y = 1,
    Z = 2
}

[Serializable]
public class WeightedBones
{
    public float weight = 0;
    public Transform bone;
    internal Quaternion initRot;

}