using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// abstract motion state that holds functionality for basic movement controls 
/// that can be utilized by other motion states that require basic movement
/// </summary>
public class PlayerMotionState : PlayerState
{
    protected Vector3 previousVertMotion;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"> the input direction vector of the player</param>
    /// <param name="previousVerticalMotion"> the player's previous vertical motion</param>
    /// <param name="speed"> the player's move speed</param>
    /// <param name="gravity"> the absolute value of gravity</param>
    protected void basicMovement(Vector2 inputDir, Vector3 previousVerticalMotion, float speed, float gravity)
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
       // RaycastHit hitInfo;
        //bool rayHit = Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, Vector3.down, out hitInfo, 0.12f);//raycat to ground

        Vector3 horizontalMotion = forward * inputDir.y + //forward component of horizontal motion
            -Vector3.Cross(forward, Vector3.up) * inputDir.x;//right component of horizontal motio
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
        if (groRay.rayHit) { //alter vert motion when grounded so player isnt "falling super fast" when theyre on the ground
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

    public void RotateBodyToCamera()
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
        playerBody.forward = Vector3.Slerp(playerBody.forward, forward, 0.5f);
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
