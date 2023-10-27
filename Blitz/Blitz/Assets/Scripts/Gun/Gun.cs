using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The gun class. All guns expand on this base class.
/// </summary>
public class Gun : MonoBehaviour
{
    [SerializeField]
    internal bulletVars bulletVars;
    [SerializeField]
    internal GunVars gunVars;


    /// <summary>
    /// Start Function. Anything All guns need to do to be set up will be done here.
    /// </summary>
    void Start()
    {

        if (gunVars.bullet == null)
        {
            Debug.LogError("No bullet associated with gun " + gameObject.name);
        }
        if (gunVars.bulletParent == null)
        {
            Debug.LogError("No folder to hold bullets associated with gun " + gameObject.name);
        }
        if (gunVars.bulletSpawnPoint == null)
        {
            Debug.LogError("No bullet spawnpoint associated with gun " + gameObject.name);
        }
    }


    /// <summary>
    /// Shoot function. Shoots the gun.
    /// </summary>
    /// <returns> 0 if the gun shoots, 1 if it doesn't & must reload. </returns>
    internal int shoot()
    {
        if (gunVars.ammo[0] <= 0)
        {
            reload();
            return 1;
        }
        gunVars.ammo[0]--;
        //       Bullet Prefab         Bullet spawnpoint position            Player rotation        holder for bullets
        GameObject bul = Instantiate(gunVars.bullet, gunVars.bulletSpawnPoint.position, transform.parent.rotation, gunVars.bulletParent);
        return 0;
    }


    /// <summary>
    /// Reload function. Reloads the gun.
    /// </summary>
    internal void reload()
    {
        StartCoroutine(reloading());
    }


    /// <summary>
    /// Reload coroutine. Waits for a time, then reloads.
    /// </summary>
    IEnumerator reloading()
    {
        yield return new WaitForSeconds(gunVars.reloadTime);
        gunVars.ammo[0] = gunVars.ammo[1];
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
    internal int reloadTime;

    [Header("References")]
    [SerializeField]
    internal GameObject bullet;
    [SerializeField]
    internal Transform bulletParent;
    [SerializeField]
    internal Transform bulletSpawnPoint;
}


/// <summary>
/// Class to pass to bullet for variables.
/// </summary>
[System.Serializable] 
[SerializeField]
internal class bulletVars
{
    [SerializeField]
    internal int shotDamage;
    [SerializeField]
    internal float speed;
    [SerializeField]
    internal float lifeTime;
}
