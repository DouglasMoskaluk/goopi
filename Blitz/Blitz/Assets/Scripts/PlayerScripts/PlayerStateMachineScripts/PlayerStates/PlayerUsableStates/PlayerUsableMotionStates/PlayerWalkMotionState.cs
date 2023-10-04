using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerWalkMotionState : PlayerBasicMotionState
{
    private const float WALK_SPEED = 4.5f;

    public override void stateUpdate()
    {
        basicLook(input.lookInput);
        basicMovement(input.motionInput, previousVertMotion, WALK_SPEED, GRAVITY);
    }
}
