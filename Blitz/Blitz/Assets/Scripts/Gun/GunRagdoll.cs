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

    void Start()
    {
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    public void InitializeGunRagdoll(int playerGun)
    {
        for(int i = 0; i < gunModels.Length; i++)
        {
            if(i == playerGun)
            {
                gunModels[i].SetActive(true);
                break;
            }
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
