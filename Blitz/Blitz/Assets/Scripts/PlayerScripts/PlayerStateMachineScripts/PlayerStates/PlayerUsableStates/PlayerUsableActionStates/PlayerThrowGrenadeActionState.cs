using UnityEngine;

public class PlayerThrowGrenadeActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        Transform cam = camHolder.GetChild(0);
        RaycastHit hitInfo;
        bool rayHit = Physics.Raycast(cam.position, cam.forward, out hitInfo, 50f);
        Vector3 destination;
        if (rayHit)
        {
            destination = hitInfo.point;
        }
        else
        {
            destination = cam.position + (cam.forward * 50f);
            
        }
        Vector3 direction = (destination - throwFrom.position).normalized;
        grenadeThrower.ThrowGrenade(direction);
    }

    public override void transitionCheck()
    {
        FSM.transitionState(PlayerActionStates.Idle);
    }
}
