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
    float viewRadius = 10;
    [SerializeField]
    float viewDistance = 30;
    [SerializeField]
    float coneAngle = 10;
    [SerializeField]
    float rotationAngle = 55;

    float timeCurveChecks = 0;
    float RaycastDelay = 0.05f;

    float RaycastCamDistance = 6;

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
            Debug.DrawRay(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position)- transform.position , Color.yellow, 10);
            if (Physics.Raycast(transform.position - rb.velocity.normalized, other.ClosestPointOnBounds(transform.position) - transform.position, out hit, (other.ClosestPointOnBounds(transform.position) - transform.position * 1.1f).magnitude))
            {
                if ((hit.collider.CompareTag("Map") || hit.collider.CompareTag("Target") || hit.collider.CompareTag("Crate") || hit.collider.CompareTag("Player") || hit.collider.CompareTag("Dome")))
                {
                    Debug.Log("Staying trigger");//May need debug.logs...
                    collide(hit);
                    collideThisFrame = true;
                }
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
        RaycastHit[] hit = Physics.SphereCastAll(
                transform.position,
                GetComponent<SphereCollider>().radius,
                rb.velocity.normalized,
                rb.velocity.magnitude * Time.fixedDeltaTime);
        for (int i=0; i<hit.Length; i++)
        {
            if (!collideThisFrame && hit[i].collider.gameObject != gameObject/*(hit[i].collider.CompareTag("Map") || hit[i].collider.CompareTag("Target") || hit[i].collider.CompareTag("Crate") || hit[i].collider.CompareTag("Player"))*/)
            {
                Debug.Log("Genral late update collision");
                if ((hit[i].collider.CompareTag("Map") || hit[i].collider.CompareTag("Target") || hit[i].collider.CompareTag("Crate") || hit[i].collider.CompareTag("Player") || hit[i].collider.CompareTag("Dome")))
                {
                    collideThisFrame = true;
                    Debug.Log("Late Update hit ");//May need debug.logs...
                    transform.position = hit[i].point;
                    collide(hit[i]);
                    break;
                }
            }
        }

        //Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.magenta, 1);
        //if (!collideThisFrame && Physics.Raycast(transform.position, rb.velocity.normalized, out hit, rb.velocity.magnitude * Time.deltaTime))
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
                plrFSM.damagePlayer(bulletVars.shotDamage, bulletVars.owner, GetComponent<Rigidbody>().velocity, transform.position);
                onHitPlayerEffect(plr.GetComponent<PlayerBodyFSM>(), hit);
            }
            Bounce(hit);
        }
        else if (hit.collider.CompareTag("Map") || hit.collider.CompareTag("Crate"))
        {
            Debug.Log("I hit a map object!");//May need debug.logs...
            onMapHitEffect(hit);
            Bounce(hit);
        } else if (hit.collider.CompareTag("Dome"))
        {
            Bounce(hit);
        }
        else if (hit.collider.CompareTag("Target"))
        {
            hit.transform.gameObject.GetComponent<Target>().BulletHit(bulletVars.owner);
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
                if (!bulletVars.spawnOnContact[i].GetComponent<BulletDecal>())
                {
                    transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                    GameObject go = Instantiate(bulletVars.spawnOnContact[i], transform.position + new Vector3(0, 0.5f, 0), transform.rotation, transform.parent);
                    Debug.Log(go.name);
                    if (hit.transform.CompareTag("Crate") && bulletVars.snap)
                    {
                        go.transform.parent = hit.transform;
                    }
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
                else
                {
                    transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                    GameObject go = Instantiate(bulletVars.spawnOnContact[i], transform.position + new Vector3(0, 0.5f, 0), transform.rotation, hit.transform);
                    go.transform.rotation = Quaternion.LookRotation(-hit.normal);
                    go.transform.position = hit.point;
                    //go.transform.parent = hit.transform;
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
                if (go.GetComponent<SpawnableObject>() != null) go.GetComponent<SpawnableObject>().init(bulletVars.owner);
            }
            plr.playerUI.bulletCollision(SplitScreenManager.instance.GetPlayers(bulletVars.owner).transform, bulletVars.owner);
        }
    }


    private void Bounce(RaycastHit hit)
    {
        if (bulletVars.bounces)
        {
            rb.velocity = Vector3.Reflect(rb.velocity, hit.normal);
            bounced = true;
        }
        if (!bulletVars.bounces && !(!bulletVars.destroyPlayerHit && hit.collider.tag == "Player"))
        {
            //StartCoroutine(removeBullet(0));
            Destroy(gameObject);
        }
    }

    internal IEnumerator removeBullet(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        if (bulletVars.spawnOnDeath && bulletVars.spawnOnContact != null &&
            (!bulletVars.spawnEveryContact || (bulletVars.spawnEveryContact && !collideThisFrame))) // edgecase catch
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
                if (hit.collider.tag == "Player" /*|| hit.collider.GetComponent<Target>() != null*/)
                {
                    playerPos = hit.collider.transform;
                    Rigidbody rb = GetComponent<Rigidbody>();
                    Vector3 velocity = Vector3.RotateTowards(rb.velocity, playerPos.position - transform.position + Vector3.up, Mathf.Deg2Rad * rotationAngle * timeCurveChecks, 1).normalized * rb.velocity.magnitude;
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
        Debug.DrawRay(cam.position + cam.forward * RaycastCamDistance, cam.forward * 50f, Color.blue, 3);
        bool rayHit = Physics.Raycast(cam.position + cam.forward * RaycastCamDistance, cam.forward, out hitInfo, 50f);
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
        if (GetComponent<TrailRenderer>() != null) GetComponent<TrailRenderer>().material.SetColor("_EmissionColor", bulletVars.tailColor);
        StartCoroutine("BulletCurve");
    }
}
