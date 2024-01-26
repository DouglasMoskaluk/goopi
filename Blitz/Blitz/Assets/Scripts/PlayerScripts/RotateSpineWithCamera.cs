using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpineWithCamera : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform forwardDirectionRot;
    private Vector3 camInitEuler;

    [SerializeField] private List<WeightedBones> RotationBones;

    private void Awake()
    {
        camInitEuler = cam.eulerAngles;
        SetUpBones();
    }

    private void LateUpdate()
    {
        Debug.DrawRay(RotationBones[0].bone.position, forwardDirectionRot.right * 0.25f, Color.red, 0.1f);
        float camXRotDiff = cam.eulerAngles.x - camInitEuler.x; 
        foreach (var rotBone in RotationBones)
        {
            rotBone.bone.localRotation = rotBone.initRot * Quaternion.Euler(camXRotDiff * rotBone.weight, 0, 0);
            
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

[Serializable]
public class WeightedBones
{
    public float weight = 0;
    public Transform bone;
    internal Quaternion initRot;
}