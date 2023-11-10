using UnityEngine;
/// <summary>
/// 
/// </summary>
public class PlayerSlideMotionState : PlayerBasicMotionState
{
    private Vector3 startSlideDireciton;

    public override void onStateEnter()
    {
        base.onStateEnter();
        startSlideDireciton = playerBody.forward;
        

    }

    public override void onStateExit()
    {
        base.onStateExit();
        RotateBodyToCamera();
        input.resetSlide();
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        Vector3 forwardMotion = input.motionInput;
        forwardMotion.y = 1f;
        forwardMotion.x = Mathf.Clamp(forwardMotion.x, -0.3f, 0.3f);
        forwardMotion.Normalize();

        slideMovement(forwardMotion, startSlideDireciton, previousVertMotion, SLIDE_SPEED, GRAVITY);
        
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

    protected void slideMovement(Vector2 inputDir, Vector3 direction,Vector3 previousVerticalMotion, float speed, float gravity)
    {
        #region Get camera relative forward direction
        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();
        if (FSM.DisplayDebugMessages)
        {
            Debug.DrawRay(playerTransform.position + controller.center, forward * 2, Color.magenta);
        }
        #endregion

        #region get horizontal motion
        RaycastHit hitInfo;
        bool rayHit = Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, Vector3.down, out hitInfo, 0.12f);//raycat to ground
        //Vector3 horizontalMotion = forward * inputDir.y + //forward component of horizontal motion
        //    -Vector3.Cross(forward, Vector3.up) * inputDir.x;//right component of horizontal motio

        Vector3 horizontalMotion = (startSlideDireciton * inputDir.y) + (-Vector3.Cross(startSlideDireciton, Vector3.up) * inputDir.x);
        horizontalMotion.Normalize();
        if (rayHit)
        {
            horizontalMotion = Vector3.ProjectOnPlane(horizontalMotion, hitInfo.normal);//if ray hit ground project hor movemnt onto plane
        }
        horizontalMotion *= speed;//apply player speed
        #endregion

        #region Get vertical motion  
        previousVertMotion = previousVerticalMotion + Vector3.down * gravity * Time.deltaTime;//calc players vertical motion based on previous vertical motion and gravity
        previousVertMotion.y = Mathf.Max(previousVertMotion.y, MAX_GRAVITY_VEL);//makes sure player doesnt fall faster than max fall speed
        if (controller.isGrounded)
        { //alter vert motion when grounded so player isnt "falling super fast" when theyre on the ground
            previousVertMotion = Vector3.down * gravity * 0.15f;//change vertical motion to %15 of gravity so that it stays on the ground over slight height variation
        }
        #endregion

        #region consolidate vert and hor motion
        Vector3 motion = (horizontalMotion + previousVertMotion) * Time.deltaTime;//create joined vert and hor motion
        #endregion

        if (FSM.DisplayDebugMessages)//debug ray drawing
        {
            Debug.DrawRay(playerTransform.position + controller.center, horizontalMotion / speed, Color.blue);
            Debug.DrawRay(playerTransform.position + controller.center, horizontalMotion + previousVerticalMotion, Color.red);
        }

        controller.Move(motion);//apply motion
    }
}
