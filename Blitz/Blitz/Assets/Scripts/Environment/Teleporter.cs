using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private List<Teleporter> teleportingTo; //reference to where this teleporter teleports to
    [SerializeField] private float lineThickness = 1; // how thick to draw the guide lines for where the player is shot out
    [SerializeField] private float lineLength = 1;
    [SerializeField] private Vector3 direction; // the base direction direction the player is shot out from
    [SerializeField] private float arc; // how much deviation from the base direction the player could have
    [SerializeField] private float minThrowVelocity;
    [SerializeField] private float maxThrowVelocity;
    private GameObject lastTeleported;

    //math for varying arc angles
    //Vector3 vec = Vector3.Cross(-Vector3.Cross(transform.up, direction.normalized), direction.normalized);
    //Vector3 dir = Quaternion.Euler(vec * arc / 2) * direction.normalized;


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawLine(transform.position, transform.position + direction.normalized * lineLength, lineThickness);

        Handles.color = Color.green;
        Handles.DrawWireArc(transform.position, Vector3.up, Quaternion.Euler(0, -arc / 2, 0) * direction.normalized, arc, lineLength, lineThickness) ;
    }

    private void Update()
    {
        Vector3 dir = Vector3.Cross(direction.normalized, transform.right);
        Debug.DrawRay(transform.position, dir, Color.magenta, 0.1f);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject != lastTeleported)
        {


            GetTeleportTo().TeleportTarget(other.gameObject);
        }
    }


    private Teleporter GetTeleportTo()
    {
        return teleportingTo[Random.Range(0, teleportingTo.Count - 1)];
    }

    public void TeleportTarget(GameObject target)
    {
        CharacterController targetController = target.GetComponent<CharacterController>();
        PlayerBodyFSM FSM = targetController.GetComponent<PlayerBodyFSM>();
        targetController.enabled = false;
        target.transform.position = transform.position;
        targetController.enabled = true;
        FSM.setKnockBack(CalcDirection() * Random.Range(minThrowVelocity, maxThrowVelocity));
        FSM.transitionState(PlayerMotionStates.KnockBack);
        lastTeleported = target;
        StartCoroutine(ResetTarget());
    }

    private Vector3 CalcDirection()
    {
        float rand = Random.Range(- arc / 2, arc / 2);
        Vector3 vec = Vector3.Cross(-Vector3.Cross(transform.up, direction.normalized), direction.normalized);
        return Quaternion.Euler(vec * rand) * direction.normalized;
    }

    private IEnumerator ResetTarget()
    {
        yield return new WaitForSeconds(0.2f);
        lastTeleported = null;
    }

}
