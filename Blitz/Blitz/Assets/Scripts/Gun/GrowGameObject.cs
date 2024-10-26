using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowGameObject : MonoBehaviour
{
    [SerializeField]
    float startSize;
    [SerializeField]
    float growTime;
    [SerializeField]
    float endSize;

    [SerializeField]
    Transform fishparticles;

    [HideInInspector]
    public float lifeTime = 0;

    public float getScale()
    {
        return endSize / startSize;
    }

    public void ScaleGO(float scaleFactor)
    {
        startSize = scaleFactor * startSize;
        endSize = scaleFactor * startSize;
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        float scale = Mathf.Lerp(startSize, endSize, lifeTime / growTime);
        transform.localScale = new Vector3(scale, scale, scale);
        if (this.gameObject.name.Equals("FishBullet(Clone)"))
        {
            fishparticles.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void SetValues(float newStart, float newEnd, float newTime)
    {
        startSize = newStart;
        endSize = newEnd;
        lifeTime = newTime;

        if(lifeTime >= growTime)
        {
            Destroy(gameObject);
        }

    }

}
