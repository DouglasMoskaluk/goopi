using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerKnockBackMotionState : PlayerBasicMotionState
{

    float lateralRightSpeed = 6f;
    float lateralLeftSpeed = 6f;

    float forwardSpeed = 6f;
    float backwardSpeed = 6f;

    float timeForControl = 0.5f;
    float elapsedTime = 0f;
    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("Jump", 0.1f, 0);
    }

    public override void onStateExit()
    {
        base.onStateExit();
        FSM.setKnockBack(Vector3.zero);
    }

    public override void stateUpdate()
    {
        base.stateUpdate();

        elapsedTime += Time.deltaTime;

        //potentially some movement for in air that less strong than normal in air movement, idk yet

        Vector3 inputMotion = Vector3.zero;
        Vector3 lateralMotion = Vector3.zero;
        Vector3 forwardMotion = Vector3.zero;
        if (elapsedTime >= timeForControl)
        {
            lateralMotion = (playerBody.right * input.motionInput.x);
            forwardMotion = (playerBody.forward * input.motionInput.y);

            lateralMotion = (input.motionInput.x < 0) ? lateralMotion * lateralRightSpeed : lateralMotion * lateralLeftSpeed;
            forwardMotion = (input.motionInput.y < 0) ? forwardMotion * forwardSpeed : forwardMotion * backwardSpeed;

            inputMotion = forwardMotion + lateralMotion;
        }
        

        controller.Move( (FSM.getKnockBackVector() + inputMotion) * Time.deltaTime );
       
        RotateBodyToCamera();
        updateKnockBack();
    }

    private void updateKnockBack()
    {
        FSM.addKnockBack(Vector3.down * stateVariableHolder.GRAVITY * Time.deltaTime);

    }

    public override void transitionCheck()
    {
        base.transitionCheck();
        if ((controller.collisionFlags & CollisionFlags.Below) != 0)//if colliding on ground
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
        else if ((controller.collisionFlags & CollisionFlags.Sides) != 0)
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
    }

}
