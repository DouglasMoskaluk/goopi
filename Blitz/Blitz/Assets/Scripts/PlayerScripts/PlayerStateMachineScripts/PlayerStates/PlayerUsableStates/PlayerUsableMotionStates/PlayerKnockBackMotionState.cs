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

    float lerpAmount = 1f;
    float maxTurnDelta = 0.1f;

    //player input rotates knockback vector towards input after time delay

    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("Fall", 0.1f, 0);
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

        Vector3 lateralMotion = (playerBody.right * input.motionInput.x);
        Vector3 forwardMotion = (playerBody.forward * input.motionInput.y);
        //Debug.Log("for " + forwardMotion);

        Vector3 soleInput = forwardMotion + lateralMotion;

        lateralMotion = (input.motionInput.x < 0) ? lateralMotion * lateralRightSpeed : lateralMotion * lateralLeftSpeed;
        forwardMotion = (input.motionInput.y < 0) ? forwardMotion * forwardSpeed : forwardMotion * backwardSpeed;

        Vector3 inputMotion = forwardMotion + lateralMotion;

        //Debug.DrawRay(playerTransform.position + Vector3.up, FSM.getKnockBackVector(), Color.red, 0.1f);

        //AlterKnockbackBasedOnInput(soleInput);

        //Debug.DrawRay(playerTransform.position + Vector3.up, FSM.getKnockBackVector(), Color.blue, 0.1f);

        controller.Move( (FSM.getKnockBackVector()) * Time.deltaTime );

        //Debug.DrawRay(playerTransform.position + Vector3.up, inputMotion.normalized, Color.yellow, 0.1f);

        RotateBodyToCamera();
        updateKnockBack();
        //Debug.Break();
    }

    //not used rn
    private void AlterKnockbackBasedOnInput(Vector3 plrInput)
    {
        //if (plrInput.magnitude == 0) { Debug.Log("no player input"); return; }
        Debug.Log(plrInput);
        //Debug.DrawRay(playerTransform.position + Vector3.up, plrInput, Color.red, 0.1f);

        Vector3 newKnockBack = FSM.getKnockBackVector();
        //Debug.DrawRay(playerTransform.position + Vector3.up, newKnockBack, Color.yellow, 0.1f);
        float knockBackVertical = newKnockBack.y;
        newKnockBack.y = 0;
        //Debug.DrawRay(playerTransform.position + Vector3.up, newKnockBack.normalized, Color.red, 0.1f);
        //Debug.DrawRay(playerTransform.position + Vector3.up, plrInput.normalized, Color.magenta, 0.1f);

        Vector3 finalDir = Vector3.RotateTowards(newKnockBack.normalized, plrInput, maxTurnDelta, 0);
        //Debug.DrawRay(playerTransform.position + Vector3.up, finalDir.normalized, Color.green, 0.1f);
        finalDir.y = knockBackVertical;
        //Debug.DrawRay(playerTransform.position + Vector3.up, finalDir, Color.cyan, 0.1f);
        FSM.setKnockBack(finalDir);
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

    public void OnDrawGizmos()
    {
        //UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(playerTransform.position, Vector3.up, 1);
    }
}
