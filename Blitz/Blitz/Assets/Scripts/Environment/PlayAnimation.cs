using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    GameObject setActive;
    private float timer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            setActive.SetActive(true);
            StartCoroutine(disable());
        }
    }

    private IEnumerator disable()
    {
        yield return new WaitForSeconds(timer);
        setActive.SetActive(false);
    }


}
