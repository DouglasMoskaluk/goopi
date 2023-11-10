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
        Vector3 forwardMotion = input.motionInput;
        forwardMotion.x = Mathf.Clamp(forwardMotion.x, -0.5f, 0.5f);

        basicMovement(forwardMotion, previousVertMotion, SLIDE_SPEED, GRAVITY);
    }

    public override void transitionCheck()
    {
        base.transitionCheck();
    }


}
