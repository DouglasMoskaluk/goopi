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

    public override void initState(stateParams stateParams)
    {
        base.initState(stateParams);
        camHolder = stateParams.camholder;
    }

    protected void basicMovement()
    {

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
