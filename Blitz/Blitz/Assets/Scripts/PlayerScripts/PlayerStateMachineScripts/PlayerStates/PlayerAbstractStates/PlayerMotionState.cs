using UnityEngine;

/// <summary>
/// abstract motion state that holds functionality for basic movement controls 
/// that can be utilized by other motion states that require basic movement
/// </summary>
public class PlayerMotionState : PlayerState
{
    protected Transform camHolder;
    private const float CAMERA_UPPER_BOUNDS = 40f;
    private const float CAMERA_LOWER_BOUNDS = 30f;
    private const float MAX_GRAVITY_VEL = -35f;
    protected const float GRAVITY = 9.8f;
    protected Vector3 previousVertMotion;

    public override void initState(stateParams stateParams)
    {
        base.initState(stateParams);
        camHolder = stateParams.camholder;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"> the input direction vector of the player</param>
    /// <param name="previousVerticalMotion"> the player's previous vertical motion</param>
    /// <param name="speed"> the player's move speed</param>
    /// <param name="gravity"> the absolute value of gravity</param>
    protected void basicMovement(Vector2 inputDir, Vector3 previousVerticalMotion, float speed, float gravity)
    {
        #region Get camera relative forward direction
        Vector3 forward = camHolder.forward;
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
        Vector3 horizontalMotion = forward * inputDir.y + //forward component of horizontal motion
            -Vector3.Cross(forward, Vector3.up) * inputDir.x;//right component of horizontal motio
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
        if (controller.isGrounded) { //alter vert motion when grounded so player isnt "falling super fast" when theyre on the ground
            previousVertMotion = Vector3.down * GRAVITY * 0.15f;//change vertical motion to %15 of gravity so that it stays on the ground over slight height variation
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

    protected void basicLook(Vector2 lookDelta)
    {
        Vector3 newRot = camHolder.localEulerAngles + new Vector3(-lookDelta.y, lookDelta.x, 0);
        newRot = ClampCameraXRot(newRot, CAMERA_UPPER_BOUNDS, CAMERA_LOWER_BOUNDS);
        camHolder.rotation = Quaternion.Euler(newRot);
    }

    protected Vector3 ClampCameraXRot(Vector3 vec, float upperBounds, float lowerBounds)
    {
        if (vec.x > upperBounds && vec.x < 180)
        {
            vec.x = upperBounds;
        }
        else if (vec.x < 360 - lowerBounds && vec.x > 181)
        {
            vec.x = 360 - lowerBounds;
        }

        return vec;
    }
}
