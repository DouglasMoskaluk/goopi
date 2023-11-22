/// <summary>
/// 
/// </summary>
public class PlayerIdleActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        if (playerGun != null)
        {
            if (input.shootPressed) playerGun.shoot(cam);
            else if (input.reloadPressed && playerGun.Ammo < playerGun.MaxAmmo)
            {
                playerGun.reload();
            }
        }
        
    }

    public override void onStateExit()
    {
        
    }

    public override void transitionCheck()
    {
        if (input.throwGrenadePressed && grenadeThrower.hasGrenade())//and has grenade
        {
            FSM.transitionState(PlayerActionStates.ThrowGrenade);
        }
        else if (input.dropGrenadePressed && grenadeThrower.hasGrenade())//and has grenade
        {
            FSM.transitionState(PlayerActionStates.DropGrenade);
        }
    }
}
