using UnityEngine;
/// <summary>
/// 
/// </summary>

public class PlayerWalkMotionState : PlayerBasicMotionState
{
    private const float WALK_SPEED = 12f;//12 for regular walk, 16 for run, 6 for crouch, sliding 18-20

    public override void stateUpdate()
    {
        basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, WALK_SPEED, GRAVITY);
    }

    public override void transitionCheck()
    {
        if (input.toggleSprint)
        {
            FSM.transitionState(PlayerMotionStates.Run);
        }
    }
}
