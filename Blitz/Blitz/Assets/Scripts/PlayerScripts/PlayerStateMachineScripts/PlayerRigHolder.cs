using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigHolder : MonoBehaviour
{
    private Rig playerRig;

    [SerializeField] private TwoBoneIKConstraint leftArmConstraint;
    [SerializeField] private Transform leftArmIKTarget;
    [SerializeField] private Transform leftArmIKHint;

    [SerializeField] private TwoBoneIKConstraint rightArmConstraint;
    [SerializeField] private Transform rightArmIKTarget;
    [SerializeField] private Transform rightArmIKHint;

    private void Awake()
    {
        playerRig = GetComponent<Rig>();
    }
}
