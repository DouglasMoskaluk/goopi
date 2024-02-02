using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    internal BulletVars bulletVars;
    protected Rigidbody rb;
    protected bool collideThisFrame = false;
    bool bounced = false;
    float bulletStraightShotDistance = 3;

    [Header("Bullet Curving Variables")]
    [SerializeField]
    float viewRadius = 5;
    float viewDistance = 25;
    [SerializeField]
    float coneAngle = 30;
    float rotationAngle = 15;

    float timeCurveChecks = 0;
    float RaycastDelay = 0.2f;


    /// <summary>
    /// Checks for errors
    /// </summary>
    void Awake()
    {
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        GameObject plr = other.gameObject;
        if (other.attachedRigidbody != null) plr = other.attachedRigidbody.gameObject;
        RaycastHit hit;
        //Debug.DrawRay(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position)- transform.position , Color.yellow, 10);
        if (Physics.Raycast(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position) - transform.position, out hit, (other.ClosestPointOnBounds(transform.position) - transform.position * 1.1f).magnitude))
        {
            //Debug.Log("Trigger Enter");
            collide(hit);
            collideThisFrame = true;
        }
    }*/

    protected void OnTriggerStay(Collider other)
    {
        if (Vector3.Distance(transform.position, other.ClosestPointOnBounds(transform.position)) < GetComponent<SphereCollider>().radius)
        {
            GameObject plr = other.gameObject;
            if (other.attachedRigidbody != null) plr = other.attachedRigidbody.gameObject;
            RaycastHit hit;
            //Debug.DrawRay(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position)- transform.position , Color.yellow, 10);
            if (Physics.Raycast(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position) - transform.position, out hit, (other.ClosestPointOnBounds(transform.position) - transform.position * 1.1f).magnitude))
            {
                //Debug.Log("Trigger Enter");
                collide(hit);
                collideThisFrame = true;
            }
        }
    }


    //
    private void FixedUpdate()
    {
        rb.AddForce(bulletVars.forceApplied);
        rb.velocity = rb.velocity * (bulletVars.speedModifier);
        if (rb.velocity.magnitude < bulletVars.minSpeed)
        {
            rb.velocity = rb.velocity.normalized * bulletVars.minSpeed;
        }
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


    protected void collide(RaycastHit hit)
    {
        switch (SplitScreenManager.instance.GetPlayers(bulletVars.owner).playerGun.gunVars.type)
        {
            case Gun.GunType.GOOP:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.GOOP_IMPACT);
                break;
            case Gun.GunType.ICE_XBOW:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.ICE_IMPACT);
                break;
            case Gun.GunType.NERF:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.NERF_IMPACT);
                break;

            case Gun.GunType.PLUNGER:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLUNGER_IMPACT);
                break;
            case Gun.GunType.FISH:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.FISH_IMPACT);
                break;
            case Gun.GunType.BOOMSTICK:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.MEGA_IMPACT);
                break;
        }
        if (hit.collider.CompareTag("Player") && (hit.collider.GetComponent<PlayerBodyFSM>().playerID != bulletVars.owner || bounced))
        {
            GameObject plr = hit.collider.gameObject;
            if (hit.collider.attachedRigidbody != null) plr = hit.collider.attachedRigidbody.gameObject;
            PlayerBodyFSM plrFSM = plr.GetComponent<PlayerBodyFSM>();
            if (plrFSM != null) {
                plrFSM.damagePlayer(bulletVars.shotDamage, bulletVars.owner);
                onHitPlayerEffect(plr.GetComponent<PlayerBodyFSM>(), hit);
            }
            Bounce(hit);
        }
        else if (hit.collider.CompareTag("Map"))
        {
            onMapHitEffect(hit);
            Bounce(hit);
        }
        else if (hit.collider.CompareTag("Target"))
        {
            hit.transform.gameObject.GetComponent<Target>().BulletHit();
            onMapHitEffect(hit);
            Bounce(hit);
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
                Debug.Log(go.name);
                go.GetComponent<SpawnableObject>().init(bulletVars.owner);
                if (bulletVars.snap)
                {
                    //Snapping
                    go.transform.rotation = Quaternion.LookRotation(-hit.normal);
                    go.transform.position = hit.point;
                } 
                if (bulletVars.attachPlayer)
                {
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
                    go.transform.parent = plr.transform;
                    go.transform.position = hit.point;
                    go.transform.rotation = transform.rotation;
                    if (bulletVars.snap)
                    {
                        go.transform.rotation = Quaternion.LookRotation(-hit.normal);
                        go.transform.position = hit.point;
                    }
                } else if (bulletVars.snap)
                {
                    RaycastHit floor;
                    if (Physics.Raycast(transform.position, Vector3.down, out floor, 5f))
                    {
                        //Snapping
                        go.transform.rotation = Quaternion.LookRotation(-floor.normal);
                        go.transform.position = floor.point;
                    } else
                    {
                        //Snapping
                        go.transform.rotation = Quaternion.LookRotation(-hit.normal);
                        go.transform.position = hit.point;
                    }
                    
                } 
                go.GetComponent<SpawnableObject>().init(bulletVars.owner);
            }
        }
    }


    private void Bounce(RaycastHit hit)
    {
        rb.velocity = Vector3.Reflect(rb.velocity, hit.normal);
        bounced = true;
        if (!bulletVars.bounces)
        {
            StartCoroutine(removeBullet(0));
        }
    }

    internal IEnumerator removeBullet(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        if (bulletVars.spawnOnDeath && bulletVars.spawnOnContact != null &&
            (!bulletVars.spawnEveryContact || (bulletVars.spawnEveryContact))) // edgecase catch
        {
            for (int i = 0; i < bulletVars.spawnOnContact.Length; i++)
            {
                GameObject go = Instantiate(bulletVars.spawnOnContact[i], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform.parent);
                go.GetComponent<SpawnableObject>().init(bulletVars.owner);
            }
        }
        Destroy(gameObject);
    }


    internal IEnumerator BulletCurve()
    {
        while (true)
        {
            timeCurveChecks = 0;
            yield return new WaitForSeconds(RaycastDelay);
            Physics physics = new Physics();
            RaycastHit[] coneHits = physics.ConeCastAll(transform.position, viewRadius, transform.forward, viewDistance, coneAngle);
            Transform playerPos;
            foreach (RaycastHit hit in coneHits)
            {
                if (hit.collider.tag == "Player")
                {
                    playerPos = hit.collider.transform;
                    Rigidbody rb = GetComponent<Rigidbody>();
                    Vector3 velocity = Vector3.RotateTowards(rb.velocity, playerPos.position - transform.position, Mathf.Deg2Rad * rotationAngle * timeCurveChecks, 1).normalized * rb.velocity.magnitude;
                    //Debug.DrawRay(transform.position, velocity, Color.green, 1f);
                    rb.velocity = velocity;
                    break;
                }
                //hit.collider.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
            }
        }
    }


    private void Update()
    {
        timeCurveChecks += Time.deltaTime;
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
        StartCoroutine("BulletCurve");
    }
}
