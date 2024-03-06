using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform endPoint;

    [SerializeField]
    private float timeInSeconds = 6;

    [SerializeField]
    private AnimationCurve curve;

    void Start()
    {
        StartCoroutine("PointToPoint");
    }

    IEnumerator PointToPoint()
    {

        float Time = timeInSeconds * 100;

        transform.position = startPoint.position;

        Transform newStartPoint = startPoint;
        Transform newEndPoint = endPoint;

        float elapsedTime = 0;
        float curveSpot = 0;

        while (true)
        {

            if (elapsedTime >= Time)
            {
                elapsedTime = 0;

                if (newStartPoint == startPoint)
                {
                    newStartPoint = endPoint;
                    newEndPoint = startPoint;
                }
                else
                {
                    newStartPoint = startPoint;
                    newEndPoint = endPoint;
                }

                yield return new WaitForSeconds(0.01f);

            }

            //float ratio = (float)elapsedTime / Time;

            float ratio = (float)elapsedTime / Time;

            curveSpot = curve.Evaluate(ratio);
            transform.position = Vector3.Lerp(newStartPoint.position, newEndPoint.position, curveSpot);

            elapsedTime++;

            yield return new WaitForSeconds(0.01f);

        }
    }



    internal void resetCoroutine()
    {
        StopCoroutine("PointToPoint");
        StartCoroutine("PointToPoint");
    }
}
