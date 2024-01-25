using System;
/// <summary>
/// 
/// </summary>
public class PlayerFallMotionState : PlayerBasicMotionState
{
    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("Fall", 0.2f, 0);
    }

    public override void stateUpdate()
    {
        //basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, stateVariableHolder.IN_AIR_SPEED, stateVariableHolder.GRAVITY);
        RotateBodyToCamera();
    }

    public override void transitionCheck()
    {
        GroundRayCast groRay = FSM.GetGroundRayCastInfo();

        if (groRay.rayHit)
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
    }
}
