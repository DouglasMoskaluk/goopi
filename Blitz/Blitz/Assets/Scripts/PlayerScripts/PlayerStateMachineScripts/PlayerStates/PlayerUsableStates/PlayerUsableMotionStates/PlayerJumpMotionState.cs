using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerJumpMotionState : PlayerBasicMotionState
{
    private const float JUMP_FORCE = 10f;//for applied upwards when entering jump state

    public override void onStateEnter()
    {
        anim.CrossFadeInFixedTime("Jump", 0.1f, 0);
        previousVertMotion = Vector3.up * stateVariableHolder.JUMP_FORCE;
        controller.Move(Vector3.up * Time.deltaTime  * stateVariableHolder.JUMP_FORCE * 2);//make the player not on the ground so that basic move works well with is grounded and wanting to jump
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
        if (previousVertMotion.y <= 0)
        {
            FSM.transitionState(PlayerMotionStates.Fall);
        }
    }
}
