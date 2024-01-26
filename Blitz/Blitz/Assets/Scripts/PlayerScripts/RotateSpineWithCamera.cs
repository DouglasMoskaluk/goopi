using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpineWithCamera : MonoBehaviour
{
    [SerializeField] private Transform cam;
    //[SerializeField] private Transform forwardDirectionRot;
    [SerializeField] private float maxRotateValue = 25f;
    [SerializeField] private float minRotateValue = -25f;
    private Vector3 camInitEuler;
    private Quaternion camInitRot;

    [SerializeField] private List<WeightedBones> RotationBones;

    private void Awake()
    {
        camInitEuler = cam.localEulerAngles;
        camInitRot = cam.localRotation;
        SetUpBones();
    }

    private void LateUpdate()
    {
        
        float camXRotDiff = cam.eulerAngles.x - camInitEuler.x;
        Debug.Log(camXRotDiff);
        foreach (var rotBone in RotationBones)
        {
            float dirMod = (camXRotDiff < 0) ? 360 : 0;
            rotBone.bone.localRotation = rotBone.initRot * Quaternion.Euler(dirMod - (camXRotDiff * rotBone.weight), 0, 0);
            //rotBone.bone.localRotation = ClampAngleOnAxis(rotBone.bone.localRotation, (int)ClampAxis.X, minRotateValue, maxRotateValue);
        }
    }

    private void SetUpBones()
    {
        float totalWeight = 0;
        foreach (var rotBone in RotationBones)
        {
            totalWeight += rotBone.weight;
        }

        foreach (var rotBone in RotationBones)
        {
            rotBone.weight = rotBone.weight / totalWeight;
            rotBone.initRot = rotBone.bone.localRotation;
        }
    }
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