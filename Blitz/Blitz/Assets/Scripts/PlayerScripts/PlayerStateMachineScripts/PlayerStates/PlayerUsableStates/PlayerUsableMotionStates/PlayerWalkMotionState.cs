using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerWalkMotionState : PlayerBasicMotionState
{

    public override void stateUpdate()
    {
        basicLook(input.lookInput);
    }
}
