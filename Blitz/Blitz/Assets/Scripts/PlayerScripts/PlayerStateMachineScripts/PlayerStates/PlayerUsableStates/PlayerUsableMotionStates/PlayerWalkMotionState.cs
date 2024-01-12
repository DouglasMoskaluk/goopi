using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 
/// </summary>

public class PlayerWalkMotionState : PlayerBasicMotionState
{
    float animLerpAmount = 0.05f;

    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("Walk", 0.1f, 0);
        
    }

    public override void stateUpdate()
    {
        basicMovement(input.motionInput, previousVertMotion, stateVariableHolder.WALK_SPEED, stateVariableHolder.GRAVITY);
        RotateBodyToCamera();

        Vector3 motionInput = input.motionInput.normalized;
        
        anim.SetFloat("MotionX", Mathf.Lerp(motionInput.x, anim.GetFloat("MotionX"), animLerpAmount * Time.deltaTime));
        anim.SetFloat("MotionY", Mathf.Lerp(motionInput.y, anim.GetFloat("MotionY"), animLerpAmount * Time.deltaTime));
    }

    public override void onStateExit()
    {
        base.onStateExit();
        anim.SetFloat("MotionX", 0);
        anim.SetFloat("MotionY", 0);
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
