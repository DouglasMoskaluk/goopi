using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletVars bulletVars;
    Rigidbody rb;


    /// <summary>
    /// Checks for errors
    /// </summary>
    void Awake()
    {
        
    }

    /// <summary>
    /// Initializes the bullet
    /// </summary>
    /// <param name="bv">Bullet Variables passed by gun</param>
    internal void Initialize(BulletVars bv, Transform cam)
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No rigidbody on bullet");
        }

        bulletVars = bv;
        Destroy(gameObject, bulletVars.lifeTime);

        RaycastHit hitInfo;
        bool rayHit = Physics.Raycast(cam.position, cam.forward, out hitInfo, 50f);
        Vector3 destination;
        if (rayHit)
            destination = hitInfo.point;
        else
            destination = cam.position + (cam.forward * 50f);

        //calculate the direction the bullet should be thrown in
        Vector3 direction = (destination - transform.position);//find direction from throw arm to raycast point
        //float angleSignCorrection = (cam.forward.y < 0) ? -1 * grenadeThrower.arcAngle : grenadeThrower.arcAngle;//change sign of throw angle if player is looking downwards
        //direction = Quaternion.AngleAxis(angleSignCorrection, cam.right) * direction;//calculate direction
        direction.Normalize();//normalize direciton

        rb.AddForce(direction.normalized * bulletVars.speed, ForceMode.VelocityChange);
    }
}
