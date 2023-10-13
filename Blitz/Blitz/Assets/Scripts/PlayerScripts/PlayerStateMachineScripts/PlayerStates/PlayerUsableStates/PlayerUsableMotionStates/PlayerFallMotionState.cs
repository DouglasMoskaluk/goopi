/// <summary>
/// 
/// </summary>
public class PlayerFallMotionState : PlayerBasicMotionState
{

    public override void stateUpdate()
    {
        basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, IN_AIR_SPEED, GRAVITY);
    }

    public override void transitionCheck()
    {

        if (controller.isGrounded)
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
    }
}
