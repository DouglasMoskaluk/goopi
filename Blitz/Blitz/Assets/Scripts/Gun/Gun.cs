using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The gun class. All guns expand on this base class.
/// </summary>
public class Gun : MonoBehaviour
{
    [SerializeField]
    internal BulletVars bulletVars;
    [SerializeField]
    internal GunVars gunVars;
    private bool canReload = true;

    internal enum GunType { NONE, GOOP, NERF, ICE_XBOW };


    [HideInInspector]
    public int Ammo { get { return gunVars.ammo[0]; } }
    public int MaxAmmo { get { return gunVars.ammo[1]; } }


    /// <summary>
    /// Start Function. Anything All guns need to do to be set up will be done here.
    /// </summary>
    void Awake()
    {

        if (gunVars.bullet == null)
        {
            Debug.LogError("No bullet associated with gun " + gameObject.name);
        }
        if (gunVars.bulletParent == null)
        {
            Debug.Log("No folder to hold bullets associated with gun " + gameObject.name);
        }
        if (gunVars.bulletSpawnPoint == null)
        {
            Debug.LogError("No bullet spawnpoint associated with gun " + gameObject.name);
        }
        if (gunVars.ammo.Length < 2)
        {
            Debug.LogError("Not enough variables in the ammo array on gun " + gameObject.name);
        }
        if (transform.parent.parent.tag == "Player")
        {
            bulletVars.owner = transform.parent.parent.gameObject;
        } else
        {
            Debug.LogError("Gun " + gameObject.name + " not a child of a child of a player");
        }
        bulletVars.tailColor = new Color(Random.value, Random.value, Random.value);
    }


    /// <summary>
    /// Shoot function. Shoots the gun.
    /// </summary>
    /// <returns> 0 if the gun shoots, 1 if it doesn't & must reload. </returns>
    internal int shoot(Transform cam)
    {
        if (gunVars.canShoot == false)
        {
            return 1;
        }
        else if (gunVars.ammo[0] <= 0)
        {
            reload();
            return 1;
        }
        else
        {
            gunVars.ammo[0]--;
            
            gunVars.canShoot = false;
            StartCoroutine(shotCooldown());
            GameObject bul;
            if (gunVars.bulletParent != null)
                //           Bullet Prefab       Bullet spawnpoint position       camera rotation     holder for bullets
                bul = Instantiate(gunVars.bullet, gunVars.bulletSpawnPoint.position, cam.rotation, gunVars.bulletParent);
            else bul = Instantiate(gunVars.bullet, gunVars.bulletSpawnPoint.position, cam.rotation);
            if (bul.GetComponent<Bullet>() == null) Debug.LogError("Bullet from gun " + gameObject.name + " doesn't have the Bullet class.");
            else { bul.GetComponent<Bullet>().Initialize(bulletVars, cam); }
            return 0;
        }
        
    }

    private IEnumerator shotCooldown()
    {
        yield return new WaitForSeconds(gunVars.shotCooldown);
        gunVars.canShoot = true;
        
    }


    /// <summary>
    /// Reload function. Reloads the gun.
    /// </summary>
    internal void reload()
    {
        //Tell player reloading
        if (canReload && gunVars.reloadTime >= 0)
        {
            canReload = false;
            StartCoroutine(reloading());

        }
    }


    /// <summary>
    /// Reload coroutine. Waits for a time, then reloads.
    /// </summary>
    IEnumerator reloading()
    {
        yield return new WaitForSeconds(gunVars.reloadTime);
        instantReload();
    }


    internal void instantReload()
    {
        gunVars.ammo[0] = gunVars.ammo[1];
        canReload = true;
    }
}


/// <summary>
/// A simple class containing variables for the gun. Variables all guns have will be in here.
/// </summary>
[System.Serializable]
[SerializeField]
internal class GunVars
{
    [Header("General variables")]
    [Tooltip("[0] = current ammo count, [1] = max ammo")]
    [SerializeField]
    internal int[] ammo; // [0] = current ammo count, [1] = max ammo
    [Tooltip("Seconds")]
    [SerializeField]
    internal float reloadTime;
    [SerializeField]
    internal float shotCooldown;

    [Header("References")]
    [SerializeField]
    internal Gun.GunType type;
    [SerializeField]
    internal GameObject bullet;
    [SerializeField]
    internal Transform bulletParent;
    [SerializeField]
    internal Transform bulletSpawnPoint;

    //private vars
    internal bool canShoot = true;
}


/// <summary>
/// Class to pass to bullet for variables.
/// </summary>
[System.Serializable] 
[SerializeField]
internal class BulletVars
{
    [SerializeField]
    internal int shotDamage;
    [SerializeField]
    internal float speed;
    [SerializeField]
    internal float lifeTime;
    [SerializeField]
    internal Vector2 accuracy;
    [SerializeField]
    [Tooltip("In degrees")]
    internal Vector2 offset;
    [SerializeField]
    internal bool bounces;
    [SerializeField]
    internal Color tailColor;
    [SerializeField]
    internal GameObject[] spawnOnContact;
    [SerializeField]
    internal Vector3 forceApplied;
    [SerializeField]
    internal bool spawnEveryContact = true;
    [SerializeField]
    internal bool spawnOnDeath = false;
    [SerializeField]
    internal bool attachPlayer = false;


    internal GameObject owner;
}
