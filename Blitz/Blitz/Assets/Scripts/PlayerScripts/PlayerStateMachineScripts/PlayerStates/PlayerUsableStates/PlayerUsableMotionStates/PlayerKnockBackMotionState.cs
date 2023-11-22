using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBackMotionState : PlayerBasicMotionState
{
    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFade("Jump", 0.2f, 0);
    }

    public override void onStateExit()
    {
        base.onStateExit();
        FSM.setKnockBack(Vector3.zero);
    }

    public override void stateUpdate()
    {
        base.stateUpdate();

        //potentially some movement for in air that less strong than normal in air movement, idk yet

        controller.Move(FSM.getKnockBackVector() * Time.deltaTime);
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
