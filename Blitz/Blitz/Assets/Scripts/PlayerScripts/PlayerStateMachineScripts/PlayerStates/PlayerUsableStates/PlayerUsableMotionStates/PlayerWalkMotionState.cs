using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerWalkMotionState : PlayerBasicMotionState
{
    private PlayerBodyFSM FSM;
    private CharacterController controller;
    private Animator anim;


    public override void initState(stateParams stateParams)
    {
        FSM = stateParams.FSM;
        controller = stateParams.controller;
        anim = stateParams.anim;
    }

    public override void stateUpdate()
    {
       
    }
}
