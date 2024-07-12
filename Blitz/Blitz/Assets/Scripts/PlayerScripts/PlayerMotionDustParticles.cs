using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
 
public class PlayerMotionDustParticles : MonoBehaviour
{
    [SerializeField] private VisualEffect[] particles;

    [SerializeField] private List<ParticleStats> particleStatusList;

    public DustParticleStatus status;

    private void Start()
    {

        SetParticleStatus(status);
        EventManager.instance.addListener(Events.onRoundStart, ActivateParticlesEvent);
    }

    private void Update()
    {
        if (GameManager.instance.ALLOW_KEYBOARD_DEVKEYS && Input.GetKeyDown(KeyCode.H))
        {
            SetParticleStatus(DustParticleStatus.Stopped);
        }
    }

    public void ActivateParticlesEvent(EventParams param = new EventParams())
    {
        SetParticlesEnabled(true);
    }

    public void SetParticleStatus(DustParticleStatus status)
    {
        this.status = status;
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].SetInt("AmountOfParticles", particleStatusList[(int)status].amountOfParticles);
            particles[i].SetVector3("ParticleSpeed", particleStatusList[(int)status].particleSpeed);
            particles[i].SetFloat("ParticleSize", particleStatusList[(int)status].particleSize);
            particles[i].SetFloat("MaxLifeTime", particleStatusList[(int)status].particleMaxLifetime);
            particles[i].SetFloat("MinLifeTime", particleStatusList[(int)status].particleMinLifetime);
            Vector3 pRot = particles[i].transform.localEulerAngles;
            if (i == 0) pRot.y = particleStatusList[(int)status].yAxisRotationAngle;
            else pRot.y = -particleStatusList[(int)status].yAxisRotationAngle;
            particles[i].transform.localEulerAngles = pRot;
        }

    }

    public void SetParticlesEnabled(bool onOff)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].enabled = onOff;
        }
    }

    public void hide(EventParams param = new EventParams())
    {
        SetParticleStatus(DustParticleStatus.Stopped);
    }

}

[System.Serializable]
public class ParticleStats
{
    public int amountOfParticles;
    public Vector3 particleSpeed;
    public float particleSize;
    public float particleMaxLifetime;
    public float particleMinLifetime;
    public float yAxisRotationAngle;
}

public enum DustParticleStatus
{
    Stopped, Walk, Slide
}
