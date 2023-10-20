using UnityEngine;
/// <summary>
/// 
/// </summary>

public class PlayerWalkMotionState : PlayerBasicMotionState
{

    public override void stateUpdate()
    {
        basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, WALK_SPEED, GRAVITY);
        updateKnockBack();
    }

    public override void transitionCheck()
    {
        if (input.jumpPressed && controller.isGrounded)
        {
            Debug.Log("transition to jump inside walk");
            FSM.transitionState(PlayerMotionStates.Jump);
        }
        else if (input.toggleSprint && controller.isGrounded)
        {
            FSM.transitionState(PlayerMotionStates.Run);
        }

    }
}
