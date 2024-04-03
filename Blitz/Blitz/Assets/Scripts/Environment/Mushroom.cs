using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mushroom : MonoBehaviour
{

    bool canBounce = true;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && canBounce == true)
        {
            canBounce = false;
            StartCoroutine(bounceCoRo(other));
        }
    }

    IEnumerator bounceCoRo(Collider other)
    {
        PlayerBodyFSM player = other.gameObject.GetComponent<PlayerBodyFSM>();

        Transform newPoint = transform.GetChild(0);

        Vector3 direction = transform.position - newPoint.position;
        direction.Normalize();

        player.addKnockBack(direction * 50f); ;
        player.transitionState(PlayerMotionStates.KnockBack);

        yield return new WaitForSeconds(1.0f);

        canBounce = true;

    }

}
