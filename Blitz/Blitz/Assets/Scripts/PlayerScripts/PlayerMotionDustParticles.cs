using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMotionDustParticles : MonoBehaviour
{
    [SerializeField] private VisualEffect[] particles;

    [SerializeField] private List<ParticleStats> particleStatusList;

    private void Awake()
    {

        SetParticleStatus(DustParticleStatus.Walk);
    }

    public void SetParticleStatus(DustParticleStatus status)
    {
        switch (status) 
        {
            case DustParticleStatus.Stopped:
                break;
            case DustParticleStatus.Walk:

                break;
            case DustParticleStatus.Slide:

                break;
        }

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
