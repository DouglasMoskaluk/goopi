using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerSlideMotionState : PlayerBasicMotionState
{
    private Vector3 horizontalDirection;

    public override void onStateEnter()
    {
        base.onStateEnter();
        horizontalDirection = playerBody.forward;
        

    }

    public override void onStateExit()
    {
        base.onStateExit();
        RotateBodyToCamera();
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        Vector3 forwardMotion = input.motionInput;
        forwardMotion.y = 1f;
        forwardMotion.x = Mathf.Clamp(forwardMotion.x, -0.3f, 0.3f);
        forwardMotion.Normalize();

        basicMovement(forwardMotion, previousVertMotion, SLIDE_SPEED, GRAVITY);
        
        
    }

    public override void transitionCheck()
    {
        base.transitionCheck();
       
        if (!controller.isGrounded)
        {
            FSM.transitionState(PlayerMotionStates.Fall);
        }
        else if (input.jumpPressed)
        {
            FSM.transitionState(PlayerMotionStates.Jump);
        }
        else if (!input.toggleSlide)
        {
            FSM.transitionState(PlayerMotionStates.Walk);
        }
       
    }


}
