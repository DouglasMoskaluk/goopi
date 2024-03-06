using UnityEngine;
/// <summary>
/// 
/// </summary>
/// 
//center = 0.8, height = 1.7
public class PlayerSlideMotionState : PlayerBasicMotionState
{
    private Vector3 startSlideDireciton;
    private float speedModifier = 1.0f;
    private float elapsedTime = 0f;

    public override void onStateEnter()
    {
        base.onStateEnter();
        startSlideDireciton = playerBody.forward;
        anim.CrossFadeInFixedTime("Slide", 0.1f, 0);
        anim.CrossFadeInFixedTime("SpineRotateSlide", 0.1f, 2);
        anim.SetLayerWeight(1, 0);
        dustParticles.SetParticleStatus(DustParticleStatus.Slide);

    }

    public override void onStateExit()
    {
        base.onStateExit();
        RotateBodyToCamera();
        input.resetSlide();
        anim.SetLayerWeight(1, 1);
        anim.CrossFadeInFixedTime("SpineRotate", 0.1f, 2);
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        controller.height = Mathf.MoveTowards(controller.height, 1.7f, 0.05f);
        Vector2 center = controller.center;
        center.y = Mathf.MoveTowards(center.y, 0.8f, 0.025f);
        controller.center = center;

        Vector3 forwardMotion = input.motionInput;
        forwardMotion.y = 1f;
        forwardMotion.x = Mathf.Clamp(forwardMotion.x, -stateVariableHolder.slideStrafeMax, stateVariableHolder.slideStrafeMax);
        forwardMotion.Normalize();

        slideMovement(forwardMotion, startSlideDireciton, previousVertMotion, stateVariableHolder.SLIDE_SPEED * speedModifier, stateVariableHolder.GRAVITY);
        elapsedTime += Time.deltaTime;
        speedModifier = Mathf.MoveTowards(speedModifier, 0, stateVariableHolder.slideMoveTowardsValue * Time.deltaTime);

        cine.m_Lens.FieldOfView = Mathf.Lerp(cine.m_Lens.FieldOfView, stateVariableHolder.slideFOV, Time.deltaTime * stateVariableHolder.FOVLerpSpeed);
    }

    public override void transitionCheck()
    {
        base.transitionCheck();
        GroundRayCast groRay = FSM.GetGroundRayCastInfo();
        //if (!controller.isGrounded)
        //{
        //    FSM.transitionState(PlayerMotionStates.Fall);
        //}
        if (input.jumpPressed && groRay.rayHit)
        {
            FSM.transitionState(PlayerMotionStates.Jump);
        }
        else if (!input.toggleSlide || speedModifier * stateVariableHolder.SLIDE_SPEED <= 12)//stateVariableHolder.WALK_SPEED
        {
            if (!groRay.rayHit)
            {
                FSM.transitionState(PlayerMotionStates.Walk);
            }
            else
            {
                FSM.transitionState(PlayerMotionStates.Fall);
            }
            
        }

    }

    protected void slideMovement(Vector2 inputDir, Vector3 direction, Vector3 previousVerticalMotion, float speed, float gravity)
    {
        GroundRayCast groRay = FSM.GetGroundRayCastInfo();
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
        //RaycastHit hitInfo;
        //bool rayHit = Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, Vector3.down, out hitInfo, 0.12f);//raycat to ground

        //Vector3 horizontalMotion = forward * inputDir.y + //forward component of horizontal motion
        //    -Vector3.Cross(forward, Vector3.up) * inputDir.x;//right component of horizontal motio

        Vector3 horizontalMotion = (startSlideDireciton * inputDir.y) + (-Vector3.Cross(startSlideDireciton, Vector3.up) * inputDir.x);
        horizontalMotion.Normalize();
        if (groRay.rayHit)
        {
            horizontalMotion = Vector3.ProjectOnPlane(horizontalMotion, groRay.rayHitResult.normal);//if ray hit ground project hor movemnt onto plane
        }
        horizontalMotion *= speed;//apply player speed
        #endregion

        #region Get vertical motion  
        previousVertMotion = previousVerticalMotion + Vector3.down * gravity * Time.deltaTime;//calc players vertical motion based on previous vertical motion and gravity
        previousVertMotion.y = Mathf.Max(previousVertMotion.y, stateVariableHolder.MAX_GRAVITY_VEL);//makes sure player doesnt fall faster than max fall speed
        if (groRay.rayHit)
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
