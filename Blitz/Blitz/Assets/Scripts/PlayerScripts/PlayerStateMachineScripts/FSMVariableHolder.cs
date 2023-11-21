using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMVariableHolder : MonoBehaviour
{
    [Header("Motion Settings")]
    [Tooltip("The speed at which the player walks at.")] public float WALK_SPEED = 12f;

    [Tooltip("The amount of force the player is given upwards when jumping. Applied once at start of jump.")] 
    public float JUMP_FORCE = 10f;

    [Tooltip("The amount of speed the player starts with at the beginning of a slide.")] 
    public float SLIDE_SPEED = 24f;

    [Tooltip("The speed the player moves at while in the air. Not when getting knocked back though.")] 
    public float IN_AIR_SPEED = 12f;





    [Header("Slide Settings")]
    [Tooltip("The amount of time it takes for the slide to go from maximum speed to 0 in seconds. When slide reaches walking speed it transitions back to walk state")] 
    public float timeToSlow = 3f;

    [Tooltip("How much to clamp horizontal input when sliding.")] public float slideStrafeMax = 0.2f;

    [Header("Gravity Settings")]
    [Tooltip("The maximum speed a player can fall at.")] public float MAX_GRAVITY_VEL = -35f;
    [Tooltip("The amount of force applied to players downwards from gravity.")] public float GRAVITY = 30f;

    [Header("Grenade Settings")]
    [Tooltip("The maximum amount of time it takes to fully charge an impulse grenade throw in seconds.")] public float maxChargeTime = 1.0f;
}
