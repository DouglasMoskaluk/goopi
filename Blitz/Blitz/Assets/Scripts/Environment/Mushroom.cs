using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mushroom : MonoBehaviour
{

    bool canBounce = true;

    [SerializeField]
    private GameObject animObject;

    private Animation anim;

    [SerializeField] private GameObject poofVFX;
    private void Awake()
    {
        anim = animObject.transform.GetComponent<Animation>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && canBounce == true)
        {
            canBounce = false;
            anim.Play("MushroomBounce");
            Instantiate(poofVFX, transform.GetChild(0).position, transform.rotation);
            StartCoroutine(bounceCoRo(other));
        }
    }

    IEnumerator bounceCoRo(Collider other)
    {
        PlayerBodyFSM player = other.gameObject.GetComponent<PlayerBodyFSM>();

        Transform newPoint = transform.GetChild(0);

        Vector3 direction = newPoint.position - transform.position;
        Debug.Log(direction);
        //direction.Normalize();

        player.addKnockBack(direction * 20f); ;
        player.transitionState(PlayerMotionStates.KnockBack);

        yield return new WaitForSeconds(1.0f);

        canBounce = true;

    }

}
