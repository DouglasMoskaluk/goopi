using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerGrenadeArcRenderer : MonoBehaviour
{
    [SerializeField] private PlayerGrenadeThrower thrower;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Transform cam;
    [SerializeField] private int amountOfPoints = 20;
    [SerializeField] private float timeBetweenPoints = 0.1f;
    [SerializeField] private float xOffset = 0.02f;

    private RaycastHit hitInfo;
    private bool render = false;

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void RenderArc()
    {
        Vector3 startPos = throwPoint.position;
        Vector3 startVel = thrower.throwSpeed * cam.forward;

        lr.positionCount = Mathf.CeilToInt(amountOfPoints / timeBetweenPoints) + 1;
        int i = 0;
        lr.SetPosition(i, startPos);
        for (float t = 0; t < amountOfPoints; t += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPos + t * startVel;
            point.y = startPos.y + startVel.y * t + (Physics.gravity.y / 2f * t * t);
            //point += transform.right * xOffset * i;
            lr.SetPosition(i, point);
            Vector3 lastPos = lr.GetPosition(i - 1);

            if (Physics.Raycast(lastPos, (point - lastPos).normalized, out RaycastHit hit, (point - lastPos).magnitude))
            {
                lr.SetPosition(i, hit.point);
                lr.positionCount = i + 1;
                return;
            }

        }
        
    }

    private void Update()
    {
        if (render) RenderArc();  
    }

    public void EnableRendering()
    {
        lr.enabled = true;
        render = true;

    }

    public void DisableRendering()
    {
        lr.enabled = false;
        render = false;
    }

}
