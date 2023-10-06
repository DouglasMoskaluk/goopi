using System.Diagnostics;
/// <summary>
/// 
/// </summary>

public class PlayerRunMotionState : PlayerBasicMotionState
{
    private const float RUN_SPEED = 16f;//12 for regular walk, 16 for run, 6 for crouch, sliding 18-20

    public override void stateUpdate()
    {
        basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, RUN_SPEED, GRAVITY);
    }

    public override void transitionCheck()
    {
        
        if (!input.toggleSprint)
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
    }
}
