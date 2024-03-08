using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;

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
    private RumbleHandler rumble;
    private CameraShake shake;

    internal enum GunType { NONE, NERF, GOOP, ICE_XBOW, PLUNGER, FISH, BOOMSTICK };


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
        if (gunVars.bulletSpawnPoint == null)
        {
            Debug.LogError("No bullet spawnpoint associated with gun " + gameObject.name);
        }
        if (gunVars.ammo.Length < 2)
        {
            Debug.LogError("Not enough variables in the ammo array on gun " + gameObject.name);
        }
        if (transform.root.tag == "Player")
        {
            bulletVars.owner = transform.root.GetComponent<PlayerBodyFSM>().playerID;
        } else
        {
            //Debug.Log(transform.root.name);
            Debug.LogError("Gun " + gameObject.name + " not a child of a child of a player");
        }
        bulletVars.tailColor = new Color(Random.value, Random.value, Random.value);

        rumble = transform.root.GetComponent<RumbleHandler>();
        shake = transform.root.GetComponent<CameraShake>();
        if (gunVars.crosshair != null) SplitScreenManager.instance.GetPlayers(bulletVars.owner).playerUI.setCrosshair(gunVars.crosshair);

    }


    internal void RicochetEvent(bool modifierActive)
    {
        bulletVars.bounces = modifierActive;
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
            
            for (int i=0; i<gunVars.bulletSpawnPoint.Length; i++) {
                if (gunVars.ammo[0] <= 0) break;
                gunVars.ammo[0]--;
                gunVars.canShoot = false;
                rumble.ShootRumble((int)gunVars.type);
                shake.GunShake((int)gunVars.type);
                //shake.ShakeCamera(0.25f, 0.1f);
                GameObject bul;
                if (gunVars.bulletParent != null)
                    //           Bullet Prefab       Bullet spawnpoint position       camera rotation     holder for bullets
                    bul = Instantiate(gunVars.bullet, gunVars.bulletSpawnPoint[i].position, cam.rotation, gunVars.bulletParent);
                else bul = Instantiate(gunVars.bullet, gunVars.bulletSpawnPoint[i].position, cam.rotation);
                if (gunVars.muzzleFlash != null) gunVars.muzzleFlash.Play();
                if (bul.GetComponent<Bullet>() == null) Debug.LogError("Bullet from gun " + gameObject.name + " doesn't have the Bullet class.");
                else { bul.GetComponent<Bullet>().Initialize(bulletVars, cam); }
                if (gunVars.ammo[0] <=0 )
                {
                    for (int j = 0; j < gunVars.hideObjectWithNoAmmo.Length; j++)
                    {
                        gunVars.hideObjectWithNoAmmo[j].SetActive(false);
                    }
                    reload();
                } else
                {
                    StartCoroutine(shotCooldown());
                }
            }

            switch (gunVars.type)
            {
                case GunType.GOOP:
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.GOOP_SHOOT);
                    break;
                case GunType.ICE_XBOW:
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.ICE_SHOOT);
                    break;
                case GunType.NERF:
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.NERF_SHOOT);
                    break;

                case GunType.PLUNGER:
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLUNGER_SHOOT);
                    break;
                case GunType.FISH:
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.FISH_SHOOT);
                    break;
                case GunType.BOOMSTICK:
                    AudioManager.instance.PlaySound(AudioManager.AudioQueue.MEGA_SHOOT);
                    break;
            }
            gunVars.gunRecoil.applyRecoil();
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
        switch (gunVars.type)
        {
            case GunType.GOOP:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.GOOP_RELOAD);
                break;
            case GunType.ICE_XBOW:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.ICE_RELOAD);
                break;
            case GunType.NERF:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.NERF_RELOAD);
                break;
            case GunType.PLUNGER:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.PLUNGER_RELOAD);
                break;
            case GunType.FISH:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.FISH_RELOAD);
                break;
            case GunType.BOOMSTICK:
                AudioManager.instance.PlaySound(AudioManager.AudioQueue.MEGA_RELOAD);
                break;
        }
        yield return new WaitForSeconds(0.3f);
        for (int j = 0; j < gunVars.hideObjectWithNoAmmo.Length; j++)
        {
            gunVars.hideObjectWithNoAmmo[j].SetActive(true);
        }
        yield return new WaitForSeconds(gunVars.reloadTime);
        instantReload();
    }


    internal void instantReload()
    {
        if (gunVars.ammo.Length > 2) gunVars.ammo[0] = gunVars.ammo[2];
        else gunVars.ammo[0] = gunVars.ammo[1];
        canReload = true;
        gunVars.canShoot = true;
        for (int j = 0; j < gunVars.hideObjectWithNoAmmo.Length; j++)
        {
            gunVars.hideObjectWithNoAmmo[j].SetActive(true);
        }
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
    //[SerializeField]
    //internal int bulletsShot = 1;

    [Header("References")]
    [SerializeField]
    internal Gun.GunType type;
    [SerializeField]
    internal GameObject bullet;
    //[SerializeField]
    internal Transform bulletParent;
    [SerializeField]
    internal Transform[] bulletSpawnPoint;
    [SerializeField]
    internal Recoil gunRecoil;
    [SerializeField]
    internal Sprite crosshair;
    [SerializeField]
    internal GameObject[] hideObjectWithNoAmmo;
    [SerializeField]
    internal VisualEffect muzzleFlash;
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
    internal float minSpeed = 1;
    [SerializeField]
    internal float lifeTime;
    [SerializeField]
    internal Vector2 accuracy;
    [SerializeField]
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
    internal float speedModifier = 1;
    [SerializeField]
    internal bool spawnEveryContact = true;
    [SerializeField]
    internal bool spawnOnDeath = false;
    [SerializeField]
    internal bool attachPlayer = false;
    [SerializeField]
    internal bool snap = false;
    [SerializeField]
    internal bool destroyPlayerHit = true;


    internal int owner;
}
