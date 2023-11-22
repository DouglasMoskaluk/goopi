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

    void Start()
    {
        transform.position = startPoint.position;
        StartCoroutine("PointToPoint");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PointToPoint()
    {

        float Time = timeInSeconds * 100;

        Transform newStartPoint = startPoint;
        Transform newEndPoint = endPoint;

        int elapsedTime = 0;
        
        while(true)
        {

            if (elapsedTime >= Time)
            {
                elapsedTime = 0;

                if(newStartPoint == startPoint)
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

            float ratio = (float)elapsedTime / Time;

            transform.position = Vector3.Lerp(newStartPoint.position, newEndPoint.position, ratio);

            elapsedTime++;
            
            yield return new WaitForSeconds(0.01f);

        }
    }

}
