using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 5 11 9 10 -> anim only affects these bones
public class RotateSpineWithCamera : MonoBehaviour
{
    [SerializeField] private Transform rotBone;
    [SerializeField] private Animator anim;
    [SerializeField] private CinemachineFreeLook freeLook;



    private void LateUpdate() {
        float angle = 1 - freeLook.m_YAxis.Value;
        anim.Play("SpineRotate", 2, angle);
    }
}

[Serializable]
public class WeightedBones
{
    public float weight = 0;
    public Transform bone;
    internal Quaternion initRot;

}

