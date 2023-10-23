using UnityEngine;

public class PlayerDropGrenadeActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        grenadeThrower.DropGrenade();
    }

    public override void transitionCheck()
    {
        FSM.transitionState(PlayerActionStates.Idle);
    }
}
