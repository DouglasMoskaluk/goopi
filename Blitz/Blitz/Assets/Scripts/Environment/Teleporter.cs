using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Teleporter teleportingTo; //reference to where this teleporter teleports to
    [SerializeField] private float lineThickness = 1; // how thick to draw the guide lines for where the player is shot out
    [SerializeField] private float lineLength = 1;
    [SerializeField] private Vector3 direction; // the base direction direction the player is shot out from
    [SerializeField] private float arc; // how much deviation from the base direction the player could have
    [SerializeField] private float minThrowVelocity;
    [SerializeField] private float maxThrowVelocity;
    private GameObject lastTeleported;


    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawLine(transform.position, transform.position + direction.normalized * lineLength, lineThickness);

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Quaternion.Euler(0, -arc / 2, 0) * direction.normalized, arc, lineLength, lineThickness) ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject != lastTeleported)
        {
            teleportingTo.TeleportTarget(other.gameObject);
        }
    }

    public void TeleportTarget(GameObject target)
    {
        CharacterController targetController = target.GetComponent<CharacterController>();
        PlayerBodyFSM FSM = targetController.GetComponent<PlayerBodyFSM>();
        targetController.enabled = false;
        target.transform.position = transform.position;
        targetController.enabled = true;
        float throwSpeed = Random.Range(minThrowVelocity, maxThrowVelocity);
        FSM.setKnockBack(direction * throwSpeed);
        FSM.transitionState(PlayerMotionStates.KnockBack);
        lastTeleported = target;
        StartCoroutine(ResetTarget());
    }

    private IEnumerator ResetTarget()
    {
        yield return new WaitForSeconds(0.2f);
        lastTeleported = null;
    }

}
