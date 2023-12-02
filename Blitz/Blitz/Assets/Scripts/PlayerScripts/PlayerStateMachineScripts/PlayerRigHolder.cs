using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigHolder : MonoBehaviour
{
    private Rig playerRig;

    [SerializeField] internal TwoBoneIKConstraint leftArmConstraint;
    [SerializeField] internal Transform leftArmIKTarget;
    [SerializeField] internal Transform leftArmIKHint;

    [SerializeField] internal TwoBoneIKConstraint rightArmConstraint;
    [SerializeField] internal Transform rightArmIKTarget;
    [SerializeField] internal Transform rightArmIKHint;

    private void Awake()
    {
        playerRig = GetComponent<Rig>();
    }
}
