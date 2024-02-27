using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRagdoll : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float lifetime = 10.0f;

    //private int gunNum;

    [SerializeField]
    private GameObject[] gunModels;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    public void InitializeGunRagdoll(int playerGun, Vector3 gunVelocity)
    {

        if(playerGun == 5)
        {
            Destroy(gameObject);
        }

        for(int i = 0; i < gunModels.Length; i++)
        {
            if(i == playerGun)
            {
                gunModels[i].SetActive(true);
                break;
            }
        }

        rb.velocity = gunVelocity;

    }

    public void RemoveSelf(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
