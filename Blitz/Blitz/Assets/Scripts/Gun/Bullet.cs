using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletVars bulletVars;
    Rigidbody rb;
    int myBounces = 0;
    bool collideThisFrame = false;
    float spawnTime = 0.05f;
    float bulletIFrames = 0.05f;


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
        //Debug.Log("Player is: " + plr.name);
        RaycastHit hit;

        Debug.DrawRay(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position)- transform.position /*+ (rb.velocity * Time.deltaTime * 2f)*/, Color.yellow, 1);
        if (Physics.Raycast(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position) - transform.position, out hit, (other.ClosestPointOnBounds(transform.position) - transform.position * 1.1f).magnitude))
        {
            //Debug.Log("Trigger Enter");
            collide(hit);
            collideThisFrame = true;
        }
    }


    //
    private void FixedUpdate()
    {
        rb.AddForce(bulletVars.forceApplied);
        spawnTime -= Time.fixedDeltaTime;
    }


    private void LateUpdate()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.magenta, 1);
        if (!collideThisFrame && Physics.Raycast(transform.position, rb.velocity.normalized, out hit, rb.velocity.magnitude * Time.deltaTime))
        {
            Debug.Log("Late Update hit");
            collide(hit);
        }
        collideThisFrame = false;
    }


    private void collide(RaycastHit hit)
    {
        if (spawnTime < 0)
        {
            if (hit.collider.CompareTag("Player"))
            {
                spawnTime = bulletIFrames;
                GameObject plr = hit.collider.gameObject;
                if (hit.collider.attachedRigidbody != null) plr = hit.collider.attachedRigidbody.gameObject;
                plr.GetComponent<PlayerBodyFSM>().damagePlayer(bulletVars.shotDamage, bulletVars.owner);
                onHitPlayerEffect(plr.GetComponent<PlayerBodyFSM>());
                Bounce(hit);
            }
            else if (hit.collider.CompareTag("Map"))
            {
                spawnTime = bulletIFrames;
                onMapHitEffect();
                Bounce(hit);
            }
        }
    }


    private void onMapHitEffect()
    {
        if (bulletVars.spawnOnContact != null && (bulletVars.spawnOnContact || bulletVars.bounces - myBounces < 0)) 
        {
            GameObject go = Instantiate(bulletVars.spawnOnContact, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform.parent);
            if (go.GetComponent<GoopPuddle>() != null)
            {
                go.GetComponent<GoopPuddle>().owner = bulletVars.owner;
            }
        }
    }

    private void onHitPlayerEffect(PlayerBodyFSM plr)
    {
        if (bulletVars.spawnOnContact != null && !bulletVars.spawnOnContact && bulletVars.bounces - myBounces < 0)
        {
            Instantiate(bulletVars.spawnOnContact, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform.parent);
        }
    }


    private void Bounce(RaycastHit hit)
    {
        rb.velocity = Vector3.Reflect(rb.velocity, hit.normal);
        myBounces++;
        if (bulletVars.bounces - myBounces < 0)
        {
            removeBullet(0);
        }
    }

    internal IEnumerator removeBullet(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
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
        removeBullet(bulletVars.lifeTime);

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
        direction = Quaternion.AngleAxis(Random.Range(bulletVars.accuracy.y, bulletVars.accuracy.y) - bulletVars.offset.y, Quaternion.AngleAxis(90, Vector3.up) * direction) * direction;

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
