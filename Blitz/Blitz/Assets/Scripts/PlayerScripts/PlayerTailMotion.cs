using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerTailMotion : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler input;
    [SerializeField] private PlayerBodyFSM fsm;
    [SerializeField] private Transform tailIKTarget;
    [SerializeField] private SplineContainer tailIKSpline;
    [SerializeField] private SplineContainer tailIKSplineSlide;

    [Header("Motion Settings")]
    [SerializeField] private float maxVertAmp = .12f;
    [SerializeField] private float minVertAmp = 0.06f;
    [SerializeField] private float vertAmpDelta = 0.01f;
    private float verticalAmplitude = 0.02f;

    [SerializeField] private float maxVertFre = 10f;
    [SerializeField] private float minVertFre = 3f;
    [SerializeField] private float vertFreDelta = 0.3f;
    private float verticalFrequency = 1f;
    [SerializeField] private float tailMoveSpeed = 1f;

    [SerializeField, Range(0,1)] private float placeTailAt;
    [SerializeField] private List<Vector2Value> dirMap;
    private Vector2DotComparator compare;

    private void Start()
    {
        verticalAmplitude = minVertAmp;
        verticalFrequency = minVertFre;
        compare = new Vector2DotComparator(input.motionInput);
    }

    private void LateUpdate()
    {
        //tailIKTarget.position = tailIKSplineSlide.EvaluatePosition(placeTailAt);
        CalcPlaceAt();
        CalcVertFrequency();
        Vector3 finalPos = EvaluateTailPos();
        tailIKTarget.position = Vector3.MoveTowards(tailIKTarget.position, finalPos, tailMoveSpeed * Time.deltaTime);
    }

    public void CalcPlaceAt()
    {
        if (fsm.currentMotionStateFlag == PlayerMotionStates.Slide)
        {
            placeTailAt = Mathf.Clamp01((0.5f * input.motionInput.x + 0.5f) * 1.3f);
            
        }
        else
        {
            if (input.motionInput.magnitude == 0)
            {
                placeTailAt = 0.45f;
            }
            else
            {
                compare.SetDotWith(input.motionInput);
                dirMap.Sort(compare);
                placeTailAt = Mathf.MoveTowards(placeTailAt, dirMap[0].value, tailMoveSpeed / 2 * Time.deltaTime);
            }
        }
        
      
    }

    public Vector3 EvaluateTailPos()
    {
        if (fsm.currentMotionStateFlag == PlayerMotionStates.Slide)//is sliding
        {
            return (Vector3)tailIKSplineSlide.EvaluatePosition(placeTailAt);
        }
        else//walking
        {
            return (Vector3)tailIKSpline.EvaluatePosition(placeTailAt) + (Vector3.up * Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude);
        }
    }

    public void CalcVertFrequency()
    {

        if (input.motionInput.magnitude == 0)
        {
            verticalFrequency = Mathf.MoveTowards(verticalFrequency, minVertFre, vertFreDelta * Time.deltaTime);
            verticalAmplitude = Mathf.MoveTowards(verticalAmplitude, minVertAmp, vertAmpDelta * Time.deltaTime);
        }
        else
        {
            verticalFrequency = Mathf.MoveTowards(verticalFrequency, maxVertFre, vertFreDelta * Time.deltaTime);
            verticalAmplitude = Mathf.MoveTowards(verticalAmplitude, maxVertAmp, vertAmpDelta * Time.deltaTime);
        }
    }

}

[Serializable]
public class Vector2Value
{
    public Vector2 vec;
    public float value;

    public Vector2Value(Vector2 v, float f)
    {
        vec = v;
        value = f;
    }
}

public class Vector2DotComparator : IComparer<Vector2Value>
{
    Vector3 dotWith;
    public Vector2DotComparator(Vector3 dot)
    {
        dotWith = dot.normalized;
    }

    public int Compare(Vector2Value x, Vector2Value y)
    {
        float xDot = Vector2.Dot(x.vec.normalized, dotWith);
        float yDot = Vector2.Dot(y.vec.normalized, dotWith);

        if ( xDot > yDot)
        {
            return 1;
        }
        else if (xDot < yDot)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public void SetDotWith(Vector2 newDotWith)
    {
        dotWith = newDotWith;
    }
}
