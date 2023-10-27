using UnityEngine;

public class PlayerThrowGrenadeActionState : PlayerActionState
{
    public override void onStateEnter()
    {
        //Transform cam = camHolder.GetChild(0);
        Transform cam = camHolder;
        RaycastHit hitInfo;
        bool rayHit = Physics.Raycast(cam.position, cam.forward, out hitInfo, 50f);
        Vector3 destination;
        if (rayHit)
            destination = hitInfo.point;
        else
            destination = cam.position + (cam.forward * 50f);
            
        
        
        //calculate the direction the grenade should be thrown in
        Vector3 direction = (destination - throwFrom.position);//find direction from throw arm to raycast point
        float angleSignCorrection = (cam.forward.y < 0) ? -1 * grenadeThrower.arcAngle: grenadeThrower.arcAngle;//change sign of throw angle if player is looking downwards
        direction = Quaternion.AngleAxis(angleSignCorrection, cam.right) * direction;//calculate direction
        direction.Normalize();//normalize direciton


        grenadeThrower.ThrowGrenade(direction, grenadeThrower.arcAngle);
    }

    public override void transitionCheck()
    {
        FSM.transitionState(PlayerActionStates.Idle);
    }
}
