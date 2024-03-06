using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathMotionState : PlayerBasicMotionState
{
    public override void onStateEnter()
    {
        base.onStateEnter();
        dustParticles.SetParticleStatus(DustParticleStatus.Stopped);
    }

    public override void onStateExit()
    {
        base.onStateEnter();
    }

    public override void stateUpdate()
    {
        base.onStateEnter();
    }

    public override void transitionCheck()
    {
        base.transitionCheck();
    }
}
