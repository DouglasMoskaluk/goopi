using UnityEngine;

public class PlayerThrowGrenadeActionState : PlayerActionState
{
    float chargeTime = 0.0f;

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
        float angleSignCorrection = (cam.eulerAngles.x > 7) ? 0: -grenadeThrower.arcAngle;//change sign of throw angle if player is looking downwards
        direction = Quaternion.AngleAxis(angleSignCorrection, cam.right) * direction;//calculate direction
        direction.Normalize();//normalize direciton
        Debug.DrawRay(throwFrom.position, direction * grenadeThrower.throwSpeed, Color.red, 1);
        //Debug.DrawLine(throwFrom.position, destination, Color.red, 5);

        grenadeThrower.ThrowGrenade(direction, grenadeThrower.arcAngle);

        FSM.logMessage("Grenade button held for " + chargeTime.ToString() + " seconds.");
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        chargeTime += Time.deltaTime;
    }

    public override void transitionCheck()
    {
        if (!input.throwGrenadePressed)
        {
            FSM.transitionState(PlayerActionStates.Idle);
        }
    }
}
