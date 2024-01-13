/// <summary>
/// 
/// </summary>
public class PlayerFallMotionState : PlayerBasicMotionState
{
    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("Fall", 0.1f, 0);
    }

    public override void stateUpdate()
    {
        //basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, stateVariableHolder.IN_AIR_SPEED, stateVariableHolder.GRAVITY);
        RotateBodyToCamera();
    }

    public override void transitionCheck()
    {

        if (controller.isGrounded)
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
    }
}
