using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    BulletVars bulletVars;
    Rigidbody rb;
    bool collideThisFrame = false;
    float spawnTime = 0.05f;
    float bulletIFrames = 0.05f;
    float bulletStraightShotDistance = 5;


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

        //Debug.DrawRay(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position)- transform.position /*+ (rb.velocity * Time.deltaTime * 2f)*/, Color.yellow, 10);
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

        //Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.magenta, 1);
        //if (!collideThisFrame && Physics.Raycast(transform.position, rb.velocity.normalized, out hit, rb.velocity.magnitude * Time.deltaTime))
        if (!collideThisFrame && Physics.SphereCast(
                transform.position, 
                GetComponent<SphereCollider>().radius, 
                rb.velocity.normalized, 
                out hit, 
                rb.velocity.magnitude * Time.deltaTime))
        {
            //Debug.Log("Late Update hit");
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
                onHitPlayerEffect(plr.GetComponent<PlayerBodyFSM>(), hit);
                Bounce(hit);
            }
            else if (hit.collider.CompareTag("Map"))
            {
                spawnTime = bulletIFrames;
                onMapHitEffect(hit);
                Bounce(hit);
            }
        }
    }


    private void onMapHitEffect(RaycastHit hit)
    {
        if (bulletVars.spawnOnContact != null && bulletVars.spawnEveryContact) 
        {
            for (int i=0; i< bulletVars.spawnOnContact.Length; i++)
            {
                transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                GameObject go = Instantiate(bulletVars.spawnOnContact[i], transform.position + new Vector3(0, 0.5f, 0), transform.rotation, transform.parent);
                go.GetComponent<SpawnableObject>().init(bulletVars.owner);
                if (bulletVars.attachPlayer)
                {
                    //Snapping
                    go.transform.rotation = Quaternion.LookRotation(-hit.normal);
                    go.transform.position = hit.point;
                }
            }
        }
    }

    private void onHitPlayerEffect(PlayerBodyFSM plr, RaycastHit hit)
    {
        if (bulletVars.spawnOnContact != null && bulletVars.spawnEveryContact)
        {
            for (int i = 0; i < bulletVars.spawnOnContact.Length; i++)
            {
                GameObject go = Instantiate(bulletVars.spawnOnContact[i], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform.parent);
                if (bulletVars.attachPlayer)
                {
                    //Snapping
                    go.transform.parent = plr.transform;
                    go.transform.rotation = Quaternion.LookRotation(-hit.normal);
                    go.transform.position = hit.point;
                }
                go.GetComponent<SpawnableObject>().init(bulletVars.owner);
            }
        }
    }


    private void Bounce(RaycastHit hit)
    {
        rb.velocity = Vector3.Reflect(rb.velocity, hit.normal);
        if (!bulletVars.bounces)
        {
            StartCoroutine(removeBullet(0));
        }
    }

    internal IEnumerator removeBullet(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        if (bulletVars.spawnOnDeath && bulletVars.spawnOnContact != null &&
            (!bulletVars.spawnEveryContact || (bulletVars.spawnEveryContact && bulletIFrames <= 0))) // edgecase catch
        {
            for (int i = 0; i < bulletVars.spawnOnContact.Length; i++)
            {
                GameObject go = Instantiate(bulletVars.spawnOnContact[i], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform.parent);
                go.GetComponent<SpawnableObject>().init(bulletVars.owner);
            }
        }
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
            //Debug.LogError("No rigidbody on bullet");
        }

        bulletVars = bv;
        StartCoroutine(removeBullet(bulletVars.lifeTime));

        RaycastHit hitInfo;
        bool rayHit = Physics.Raycast(cam.position, cam.forward, out hitInfo, 50f);
        Vector3 destination;
        if (rayHit)
            destination = hitInfo.point;
        else
            destination = cam.position + (cam.forward * 50f);

        //calculate the direction the bullet should be thrown in
        Vector3 direction = (destination - transform.position);//find direction from throw arm to raycast point
        if (direction.magnitude < bulletStraightShotDistance)
        {
            direction = transform.forward;
        }
        //float angleSignCorrection = (cam.forward.y < 0) ? -1 * grenadeThrower.arcAngle : grenadeThrower.arcAngle;//change sign of throw angle if player is looking downwards
        //direction = Quaternion.AngleAxis(angleSignCorrection, cam.right) * direction;//calculate direction
        direction.Normalize();//normalize direciton
        direction = Quaternion.AngleAxis(Random.Range(-bulletVars.accuracy.x, bulletVars.accuracy.x) + bulletVars.offset.x, Vector3.up) * direction;
        direction = Quaternion.AngleAxis(Random.Range(-bulletVars.accuracy.y, bulletVars.accuracy.y) - bulletVars.offset.y, Quaternion.AngleAxis(90, Vector3.up) * direction) * direction;

        /*Vector3 offset = new Vector3(
            Random.Range(-bulletVars.accuracy.x + bulletVars.offset.x, bulletVars.accuracy.x + bulletVars.offset.x),
            Random.Range(-bulletVars.accuracy.y + bulletVars.offset.y, bulletVars.accuracy.y + bulletVars.offset.y),
            Random.Range(0, 0));
        //Debug.Log("Offset: " + offset + ", Direction: " + direction);
        direction += offset;*/
        rb.AddForce(direction.normalized * bulletVars.speed, ForceMode.VelocityChange);

        //Debug.Log("Tail Renderer color");
        GetComponent<TrailRenderer>().material.SetColor("_EmissionColor", bulletVars.tailColor);
    }
}
