using UnityEngine;

public class PlayerDropGrenadeActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        Vector3 throwDirection = cam.forward;
        grenadeThrower.DropGrenade(throwDirection);
    }

    public override void transitionCheck()
    {
        FSM.transitionState(PlayerActionStates.Idle);
    }
}
