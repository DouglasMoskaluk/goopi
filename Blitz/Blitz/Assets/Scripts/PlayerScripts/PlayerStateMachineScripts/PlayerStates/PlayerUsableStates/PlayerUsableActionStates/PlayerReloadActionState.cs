using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerReloadActionState : PlayerActionState
{
    private float reloadTime = 0;
    private float elapsedTime = 0;
    private bool reloadFinished = false;

    public override void onStateEnter()
    {
        anim.Play("Reload", 1, 0f);
        reloadTime = playerGun.gunVars.reloadTime;
        uiHandler.setReloadIndicatorVisible(true);
        uiHandler.setReloadIndicatorPercent(0);
        uiHandler.StopReloadFade();
    }

    public override void stateUpdate()
    {
        elapsedTime += Time.deltaTime;
        uiHandler.setReloadIndicatorPercent((elapsedTime) / (reloadTime - 0.2f));
        if (elapsedTime >= reloadTime) 
        {
            playerGun.instantReload();
            reloadFinished = true;
        }
    }

    public override void onStateExit()
    {
        anim.Play("Idle", 1, 0);
        uiHandler.fadeOutReloadIcon();
    }

    public override void transitionCheck()
    {
        base.transitionCheck();
        if (reloadFinished)
        {
            FSM.transitionState(PlayerActionStates.Idle);
        }
    }
}
