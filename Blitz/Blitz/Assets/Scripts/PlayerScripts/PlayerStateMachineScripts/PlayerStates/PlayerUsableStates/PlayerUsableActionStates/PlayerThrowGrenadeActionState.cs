using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class PlayerThrowGrenadeActionState : PlayerActionState
{
    float chargeTime = 0.0f;
    float elapsedTime = 0.0f;
    bool hasThrown = false;
    bool leaveState = false;
    Vector3 direction;

    public override void onStateEnter()
    {
        base.onStateEnter();
        anim.CrossFadeInFixedTime("ThrowStart", 0.1f, 1);
        arcRenderer.EnableRendering();
    }

    public override void onStateExit()
    {
        
    }

    public override void stateUpdate()
    {
        base.stateUpdate();
        if (hasThrown) elapsedTime += Time.deltaTime;

        if (input.throwGrenadePressed && hasThrown == false)//charging the grenade
        {
            chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0, stateVariableHolder.maxChargeTime);
        }

        if (!input.throwGrenadePressed && hasThrown == true)//not charging and in process of throwing
        {
            if (elapsedTime >= 0.2f && leaveState == false)//grenade gets thrown
            {
                grenadeThrower.ThrowGrenade(direction, stateVariableHolder.maxChargeTime);
                arcRenderer.DisableRendering();
                leaveState = true;

                //Debug.DrawRay(throwFrom.position, direction * 2, Color.red, 0.1f);
                //Debug.Break();
            }
        }
        else if (!input.throwGrenadePressed && hasThrown == false)//not charging but the grenade hasnt thrown yet
        {
            RaycastHit hitInfo;
            bool rayHit = Physics.Raycast(cam.position, cam.forward, out hitInfo, 50f);
            Vector3 destination;
            if (rayHit)
                destination = hitInfo.point;
            else
                destination = cam.position + (cam.forward * 50f);

            //calculate the direction the grenade should be thrown in
            direction = (destination - throwFrom.position);//find direction from throw arm to raycast point
                                                           //float angleSignCorrection = (cam.eulerAngles.x > 7) ? -grenadeThrower.arcAngle: grenadeThrower.arcAngle;//change sign of throw angle if player is looking downwards
            direction = Quaternion.AngleAxis(-grenadeThrower.arcAngle, cam.right) * direction;//calculate direction
            direction.Normalize();//normalize direciton

            hasThrown = true;
            anim.CrossFadeInFixedTime("ThrowEnd", 0, 1);
            elapsedTime = 0;
        }
    }

    public override void transitionCheck()
    {
        if (leaveState && elapsedTime >= 1.66f)
        {
            FSM.transitionState(PlayerActionStates.Idle);
        }
    }
}
