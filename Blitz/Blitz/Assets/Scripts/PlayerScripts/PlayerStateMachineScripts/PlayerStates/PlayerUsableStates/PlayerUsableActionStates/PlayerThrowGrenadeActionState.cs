using UnityEngine;

public class PlayerThrowGrenadeActionState : PlayerActionState
{
    float chargeTime = 0.0f;
    float maxChargeTime = 1.0f;

    public override void onStateExit()
    {
        
        RaycastHit hitInfo;
        bool rayHit = Physics.Raycast(cam.position, cam.forward, out hitInfo, 50f);
        Vector3 destination;
        if (rayHit)
            destination = hitInfo.point;
        else
            destination = cam.position + (cam.forward * 50f);
        
        //calculate the direction the grenade should be thrown in
        Vector3 direction = (destination - throwFrom.position);//find direction from throw arm to raycast point
        float angleSignCorrection = (cam.eulerAngles.x > 7) ? -grenadeThrower.arcAngle: grenadeThrower.arcAngle;//change sign of throw angle if player is looking downwards
        direction = Quaternion.AngleAxis(angleSignCorrection, cam.right) * direction;//calculate direction
        direction.Normalize();//normalize direciton

        grenadeThrower.ThrowGrenade(direction, chargeTime / maxChargeTime);
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0, maxChargeTime);
    }

    public override void transitionCheck()
    {
        if (!input.throwGrenadePressed)
        {
            FSM.transitionState(PlayerActionStates.Idle);
        }
    }
}
