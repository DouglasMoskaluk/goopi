using System;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerFallMotionState : PlayerBasicMotionState
{

    float timeForMaxSmokeEffect = 1.2f;
    float elapsedTime = 0;
    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("Fall", 0.2f, 0);
    }

    public override void stateUpdate()
    {
        //basicLook(input.lookInput);
        elapsedTime += Time.deltaTime;
        basicMovement(input.motionInput, previousVertMotion, stateVariableHolder.IN_AIR_SPEED * Mathf.Clamp(input.motionInput.magnitude, 0.4f, 1f), stateVariableHolder.GRAVITY);
        RotateBodyToCamera();
    }

    public override void transitionCheck()
    {
        GroundRayCast groRay = FSM.GetGroundRayCastInfo();

        if (groRay.rayHit)
        {
            float timePercent = Mathf.Clamp(elapsedTime / timeForMaxSmokeEffect, 0.4f, 1);
            GameObject smoke = VFXSpawner.instance.spawnObject(VFXSpawnerObjects.smoke);
            smoke.transform.position = playerTransform.position + Vector3.up * 0.02f;
            smoke.GetComponent<SmokeEffectSetUp>().setSmokeAmountAndSize(Mathf.CeilToInt(32 * timePercent), timePercent);
            

            FSM.transitionState(PlayerMotionStates.Walk);
        }
    }
}
