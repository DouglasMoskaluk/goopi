using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigHolder : MonoBehaviour
{
    [SerializeField] internal TwoBoneIKConstraint leftArmConstraint;
    [SerializeField] internal Transform leftArmIKTarget;
    [SerializeField] internal Transform leftArmIKHint;

    [SerializeField] internal TwoBoneIKConstraint rightArmConstraint;
    [SerializeField] internal Transform rightArmIKTarget;
    [SerializeField] internal Transform rightArmIKHint;

    public void disableRig()
    {
        leftArmConstraint.weight = 0f;
        rightArmConstraint.weight = 0f;
    }

    public void enableRig()
    {
        leftArmConstraint.weight = 1f;
        rightArmConstraint.weight = 1f;
    }
}
