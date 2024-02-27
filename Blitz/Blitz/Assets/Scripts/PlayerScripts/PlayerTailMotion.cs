using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerTailMotion : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler input;

    [SerializeField] private Transform tailIKTarget;

    [SerializeField] private SplineContainer tailIKSpline;

    [SerializeField, Range(0,1)] private float TEST_PlaceTailAt;

    private void LateUpdate()
    {
        float3 tail = tailIKSpline.EvaluatePosition(TEST_PlaceTailAt);
        tailIKTarget.position = new Vector3(tail.x, tail.y, tail.z);
    }

}
