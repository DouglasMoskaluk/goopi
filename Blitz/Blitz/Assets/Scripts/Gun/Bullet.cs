using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletVars bulletVars;


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
    }

    // Update is called once per frame
    void Update()
    {
        //TODO Move bullet
    }
}
