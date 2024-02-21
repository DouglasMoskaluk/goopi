using System.Diagnostics;
/// <summary>
/// 
/// </summary>
public class PlayerIdleActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        anim.CrossFadeInFixedTime("Idle", 0.1f, 1);
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        if (playerGun != null)
        {
            if (input.shootPressed)
            {
                if (playerGun.Ammo >= 1) playerGun.shoot(cam);
                else { 
                    if ((playerGun.gunVars.type != Gun.GunType.NERF && playerGun.gunVars.type != Gun.GunType.BOOMSTICK))
                    {
                        if (playerGun) FSM.transitionState(PlayerActionStates.Reload);
                        return;
                    }
                }
            }
            //else if (input.reloadPressed && playerGun.Ammo < playerGun.MaxAmmo)
            //{
            //    playerGun.reload();
            //}
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
        else if (playerGun != null && input.reloadPressed && playerGun.Ammo < playerGun.MaxAmmo &&
            ((playerGun.gunVars.type != Gun.GunType.NERF && playerGun.gunVars.type != Gun.GunType.BOOMSTICK)))
        {
            FSM.transitionState(PlayerActionStates.Reload);
        }

    }
}
