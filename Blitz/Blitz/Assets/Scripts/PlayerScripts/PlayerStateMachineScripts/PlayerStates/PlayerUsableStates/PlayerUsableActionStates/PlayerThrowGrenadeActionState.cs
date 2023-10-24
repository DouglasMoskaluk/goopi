using UnityEngine;

public class PlayerThrowGrenadeActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        Vector3 throwDirection = camHolder.GetChild(0).forward;
        grenadeThrower.ThrowGrenade(throwDirection);
    }

    public override void transitionCheck()
    {
        FSM.transitionState(PlayerActionStates.Idle);
    }
}
