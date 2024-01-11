using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 
/// </summary>

public class PlayerWalkMotionState : PlayerBasicMotionState
{

    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("Walk", 0.3f, 0);
        
    }

    public override void stateUpdate()
    {
        basicMovement(input.motionInput, previousVertMotion, stateVariableHolder.WALK_SPEED, stateVariableHolder.GRAVITY);
        RotateBodyToCamera();

        Vector3 motionInput = input.motionInput.normalized;

        anim.SetFloat("MotionX", motionInput.x);
        anim.SetFloat("MotionY", motionInput.y);
    }

    public override void transitionCheck()
    {
        if (input.jumpPressed && controller.isGrounded)
        {
            FSM.transitionState(PlayerMotionStates.Jump);
        }
        else if (input.toggleSlide && controller.isGrounded && input.motionInput.y > 0)
        {
            FSM.transitionState(PlayerMotionStates.Slide);
        }

    }
}
