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
        dustParticles.SetParticleStatus(DustParticleStatus.Stopped);
    }

    public override void stateUpdate()
    {
        //basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, stateVariableHolder.IN_AIR_SPEED * Mathf.Clamp(input.motionInput.magnitude, 0.4f, 1f), stateVariableHolder.GRAVITY);
        RotateBodyToCamera();
        cine.m_Lens.FieldOfView = Mathf.Lerp(cine.m_Lens.FieldOfView, stateVariableHolder.othersFOV, Time.deltaTime * stateVariableHolder.FOVLerpSpeed);
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
