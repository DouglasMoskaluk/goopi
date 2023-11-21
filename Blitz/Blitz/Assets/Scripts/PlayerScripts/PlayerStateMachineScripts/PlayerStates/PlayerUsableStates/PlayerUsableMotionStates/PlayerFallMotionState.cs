/// <summary>
/// 
/// </summary>
public class PlayerFallMotionState : PlayerBasicMotionState
{

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
