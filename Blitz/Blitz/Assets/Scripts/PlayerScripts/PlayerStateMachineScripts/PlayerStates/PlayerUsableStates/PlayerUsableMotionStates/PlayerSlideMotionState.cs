using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerSlideMotionState : PlayerBasicMotionState
{
    private Vector3 horizontalDirection;

    public override void onStateEnter()
    {
        base.onStateEnter();
        horizontalDirection = playerBody.forward;

    }

    public override void onStateExit()
    {
        base.onStateExit();
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        basicMovement(input.motionInput, previousVertMotion, SLIDE_SPEED, GRAVITY);
    }

    public override void transitionCheck()
    {
        base.transitionCheck();
    }


}
