using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 5 11 9 10 -> anim only affects these bones, animation playback speed for these animations clips needs to be 0
[RequireComponent(typeof(PlayerBodyFSM))]
public class RotateSpineWithCamera : MonoBehaviour
{
    [SerializeField] private Transform rotBone;
    [SerializeField] private Animator anim;
    [SerializeField] private CinemachineFreeLook freeLook;
    private PlayerBodyFSM FSM;

    private void Awake()
    {
        FSM = GetComponent<PlayerBodyFSM>();
    }

    private void Start()
    {
        //EventManager.instance.addListener(Events.onPlayerRespawn, ResetSpine);
    }

    public void LateUpdate() {
        float angle = 1 - freeLook.m_YAxis.Value;
        if (FSM.currentMotionStateFlag == PlayerMotionStates.Slide)
        {
            anim.Play("SpineRotateSlide", 2, angle);
        }
        else
        {
            anim.Play("SpineRotate", 2, angle);
            
            
        }
    }

    public void ResetSpine(EventParams param = new EventParams())
    {
        anim.Play("SpineRotate", 2, 0.5f);
        freeLook.m_YAxis.Value = 0.5f;
    }

}

