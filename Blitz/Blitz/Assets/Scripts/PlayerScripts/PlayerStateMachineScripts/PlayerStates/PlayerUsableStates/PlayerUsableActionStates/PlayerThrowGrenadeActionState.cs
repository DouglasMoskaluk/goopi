using UnityEngine;

public class PlayerThrowGrenadeActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        grenadeThrower.ThrowGrenade();
    }

    public override void transitionCheck()
    {
        FSM.transitionState(PlayerActionStates.Idle);
    }
}
