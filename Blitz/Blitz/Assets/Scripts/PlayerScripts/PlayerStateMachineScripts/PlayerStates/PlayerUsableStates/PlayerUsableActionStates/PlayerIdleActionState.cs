using System.Diagnostics;
/// <summary>
/// 
/// </summary>
public class PlayerIdleActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        //anim.CrossFadeInFixedTime("Idle", 0.1f, 1);
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        if (playerGun != null)
        {
            if (input.shootPressed)
            {
                FSM.logMessage("Tried to shoot");
                playerGun.shoot(cam);
            }
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
