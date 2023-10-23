/// <summary>
/// 
/// </summary>
public class PlayerIdleActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        
    }

    public override void onStateExit()
    {
        
    }

    public override void transitionCheck()
    {
        if (input.throwGrenadePressed)//and has grenade
        {
            FSM.transitionState(PlayerActionStates.ThrowGrenade);
        }
        else if (input.dropGrenadePressed)//and has grenade
        {
            FSM.transitionState(PlayerActionStates.DropGrenade);
        }
    }
}
