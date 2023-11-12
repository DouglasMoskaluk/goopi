using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletVars bulletVars;
    Rigidbody rb;
    int myBounces = 0;
    bool collideThisFrame = false;


    /// <summary>
    /// Checks for errors
    /// </summary>
    void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject plr = other.gameObject;
        //Debug.Log("Bullet collided with gameObject " + other.name);
        //Debug.Log(other.attachedRigidbody.name);
        if (other.attachedRigidbody != null) plr = other.attachedRigidbody.gameObject;
        //Debug.Log("Player is: " + plr.name);
        RaycastHit hit;
        if (Physics.Raycast(transform.position - rb.velocity.normalized, (plr.transform.position - transform.position).normalized, out hit, rb.velocity.magnitude * Time.deltaTime))
        {
            //Debug.Log("Trigger Enter");
            collide(hit);
            collideThisFrame = true;
        }
    }


    private void LateUpdate()
    {
        RaycastHit hit;

        //Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.magenta, 1);
        if (!collideThisFrame && Physics.Raycast(transform.position, rb.velocity.normalized, out hit, rb.velocity.magnitude * Time.deltaTime))
        {
            //Debug.Log("Late Update");
            collide(hit);
        }
        collideThisFrame = false;
    }


    private void collide(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            GameObject plr = hit.collider.gameObject;
            if (hit.collider.attachedRigidbody != null) plr = hit.collider.attachedRigidbody.gameObject;
            plr.GetComponent<PlayerBodyFSM>().damagePlayer(bulletVars.shotDamage, bulletVars.owner);
            Bounce(hit);
            onHitPlayerEffect(plr.GetComponent<PlayerBodyFSM>());
        }
        else if (hit.collider.CompareTag("Map"))
        {
            Bounce(hit);
            onMapHitEffect();
        }
    }


    private void onMapHitEffect()
    {
        if (bulletVars.spawnOnContact != null)
        {
            Instantiate(bulletVars.spawnOnContact, transform.position, Quaternion.identity);
        }
    }

    private void onHitPlayerEffect(PlayerBodyFSM plr)
    {

    }


    private void Bounce(RaycastHit hit)
    {
        rb.velocity = Vector3.Reflect(rb.velocity, hit.normal);
        myBounces++;
        if (bulletVars.bounces - myBounces < 0)
        {
            Destroy(gameObject);
        }
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
        direction = Quaternion.AngleAxis(Random.Range(-bulletVars.accuracy.x, bulletVars.accuracy.x) + bulletVars.offset.x, Vector3.up) * direction;
        direction = Quaternion.AngleAxis(Random.Range(bulletVars.accuracy.y, bulletVars.accuracy.y) - bulletVars.offset.y, Vector3.right) * direction;

        /*Vector3 offset = new Vector3(
            Random.Range(-bulletVars.accuracy.x + bulletVars.offset.x, bulletVars.accuracy.x + bulletVars.offset.x),
            Random.Range(-bulletVars.accuracy.y + bulletVars.offset.y, bulletVars.accuracy.y + bulletVars.offset.y),
            Random.Range(0, 0));
        Debug.Log("Offset: " + offset + ", Direction: " + direction);
        direction += offset;*/
        rb.AddForce(direction.normalized * bulletVars.speed, ForceMode.VelocityChange);

        Debug.Log("Tail Renderer color");
        GetComponent<TrailRenderer>().material.SetColor("_EmissionColor", bulletVars.tailColor);
    }
}
