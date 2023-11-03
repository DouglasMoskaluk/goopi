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

    private void OnTriggerEnter(Collider other)
    {
        GameObject plr = other.gameObject;
        Debug.Log("Bullet collided with gameObject " + other.name);
        //Debug.Log(other.attachedRigidbody.name);
        if (other.attachedRigidbody != null) plr = other.attachedRigidbody.gameObject;
        Debug.Log("Player is: " + plr.name);
        if (plr.tag == "Player")
        {
            Debug.Log("Bullet says: Damage Player " + other.name + " by " + bulletVars.owner + " for " + bulletVars.shotDamage + " damage");
            plr.GetComponent<PlayerBodyFSM>().damagePlayer(bulletVars.shotDamage, bulletVars.owner);
        }
        Destroy(this);
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
        if (direction.magnitude < 5)
        {
            direction = transform.forward;
        }
        //float angleSignCorrection = (cam.forward.y < 0) ? -1 * grenadeThrower.arcAngle : grenadeThrower.arcAngle;//change sign of throw angle if player is looking downwards
        //direction = Quaternion.AngleAxis(angleSignCorrection, cam.right) * direction;//calculate direction
        direction.Normalize();//normalize direciton
        Vector3 offset =
            new Vector3(Random.Range(-bulletVars.accuracy.x, bulletVars.accuracy.x),
            Random.Range(-bulletVars.accuracy.y, bulletVars.accuracy.y));
        direction += offset;
        rb.AddForce(direction.normalized * bulletVars.speed, ForceMode.VelocityChange);
    }
}
