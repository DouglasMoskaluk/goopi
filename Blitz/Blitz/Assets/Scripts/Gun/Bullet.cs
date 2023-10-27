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
    void Start()
    {
        
    }

    /// <summary>
    /// Initializes the bullet
    /// </summary>
    /// <param name="bv">Bullet Variables passed by gun</param>
    internal void Initialize(BulletVars bv)
    {
        bulletVars = bv;
        Destroy(gameObject, bulletVars.lifeTime);
        rb.AddForce(transform.rotation * Vector3.zero * bulletVars.speed, ForceMode.VelocityChange);
    }
}
