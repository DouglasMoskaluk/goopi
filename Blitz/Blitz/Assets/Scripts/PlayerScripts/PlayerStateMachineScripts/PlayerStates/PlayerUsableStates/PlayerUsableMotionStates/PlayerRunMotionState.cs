using UnityEngine;
/// <summary>
/// 
/// </summary>

public class PlayerRunMotionState : PlayerBasicMotionState
{


    public override void stateUpdate()
    {
        basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, RUN_SPEED, GRAVITY);
    }

    public override void transitionCheck()
    {
        if (input.jumpPressed)
        {
            FSM.transitionState(PlayerMotionStates.Jump);
        }
        else if (!input.toggleSprint || input.motionInput == Vector2.zero || input.motionInput.y < 0)
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
    }
}
