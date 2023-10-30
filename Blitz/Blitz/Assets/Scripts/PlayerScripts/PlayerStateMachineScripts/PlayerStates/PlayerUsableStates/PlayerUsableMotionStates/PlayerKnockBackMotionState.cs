using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBackMotionState : PlayerBasicMotionState
{
    public override void onStateEnter()
    {
        base.onStateEnter();
        FSM.logMessage("inside knock back state");
    }

    public override void onStateExit()
    {
        base.onStateExit();
        FSM.setKnockBack(Vector3.zero);
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        controller.Move(FSM.getKnockBackVector() * Time.deltaTime);
        updateKnockBack();
    }

    private void updateKnockBack()
    {
        FSM.addKnockBack(Vector3.down * GRAVITY * Time.deltaTime);
    }

    public override void transitionCheck()
    {
        base.transitionCheck();
        if ((controller.collisionFlags & CollisionFlags.Below) != 0)//if colliding on ground
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
    }
}
