using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 
/// </summary>

public class PlayerWalkMotionState : PlayerBasicMotionState
{
    float animLerpAmount = 0.07f;
    float animLerpRestingAmount = 0.12f;

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

        //anim.SetFloat("MotionX", Mathf.Lerp(input.motionInput.x, anim.GetFloat("MotionX"), 0.0001f));
        //anim.SetFloat("MotionY", Mathf.Lerp(input.motionInput.y, anim.GetFloat("MotionY"), 0.0001f));

        float lerpAmountX = (input.motionInput.x == 0) ? animLerpRestingAmount : animLerpAmount;
        float animXValue = Mathf.Lerp(anim.GetFloat("MotionX"), input.motionInput.x, lerpAmountX);
        anim.SetFloat("MotionX", animXValue);

        float lerpAmountY = (input.motionInput.x == 0) ? animLerpRestingAmount : animLerpAmount;
        float animYValue = Mathf.Lerp(anim.GetFloat("MotionY"), input.motionInput.y, lerpAmountY);
        anim.SetFloat("MotionY", animYValue);
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
