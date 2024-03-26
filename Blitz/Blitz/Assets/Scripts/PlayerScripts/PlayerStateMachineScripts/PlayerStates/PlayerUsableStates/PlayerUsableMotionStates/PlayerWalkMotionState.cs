using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 
/// </summary>



//center = 0.95, height = 2
public class PlayerWalkMotionState : PlayerBasicMotionState
{
    float animLerpAmount = 0.07f;
    float animLerpRestingAmount = 0.2f;

    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("Walk", 0.1f, 0);

    }
     
    public override void stateUpdate()
    {
        cine.m_Lens.FieldOfView = Mathf.Lerp(cine.m_Lens.FieldOfView, stateVariableHolder.othersFOV, Time.deltaTime * stateVariableHolder.FOVLerpSpeed);

        controller.height = Mathf.MoveTowards(controller.height, 2, 0.05f);
        Vector2 center = controller.center;
        center.y = Mathf.MoveTowards(center.y, 0.95f, 0.025f);
        controller.center = center;

        basicMovement(input.motionInput, previousVertMotion, stateVariableHolder.WALK_SPEED * Mathf.Clamp(input.motionInput.magnitude, 0.4f, 1f), stateVariableHolder.GRAVITY);
        RotateBodyToCamera();

        float lerpAmountX = (input.motionInput.x == 0) ? animLerpRestingAmount : animLerpAmount;
        float lerpToX = (input.motionInput.x > 0) ? 1 : -1;
        if (input.motionInput.x == 0) lerpToX = 0;
        float animXValue = Mathf.Lerp(anim.GetFloat("MotionX"), lerpToX, lerpAmountX);
        anim.SetFloat("MotionX", animXValue);

        float lerpAmountY = (input.motionInput.x == 0) ? animLerpRestingAmount : animLerpAmount;
        float lerpToY = (input.motionInput.y > 0) ? 1 : -1;
        if (input.motionInput.y == 0) lerpToY = 0;
        float animYValue = Mathf.Lerp(anim.GetFloat("MotionY"), lerpToY, lerpAmountY);
        anim.SetFloat("MotionY", animYValue);

        anim.SetFloat("WalkAnimSpeedMultiplier", Mathf.Clamp(input.motionInput.magnitude, 0.4f, 1f));
         
        //dust particles
        if (input.motionInput.sqrMagnitude == 0)
        {
            dustParticles.SetParticleStatus(DustParticleStatus.Stopped);
        }
        else
        {
            dustParticles.SetParticleStatus(DustParticleStatus.Walk);
        }
    }

    public override void onStateExit()
    {
        base.onStateExit();
        anim.SetFloat("MotionX", 0);
        anim.SetFloat("MotionY", 0);
    }

    public override void transitionCheck()
    {
        GroundRayCast groRay = FSM.GetGroundRayCastInfo();
        if (input.jumpPressed && groRay.rayHit)
        {
            FSM.transitionState(PlayerMotionStates.Jump);
        }
        else if (input.toggleSlide && groRay.rayHit && input.motionInput.y == 0)
        {
            FSM.transitionState(PlayerMotionStates.Slide);
        }
        else if (!groRay.rayHit)
        {
            FSM.transitionState(PlayerMotionStates.Fall);
        }

    }
}
