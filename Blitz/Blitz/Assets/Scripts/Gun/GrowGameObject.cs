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
    float lifeTime = 0;

    private void Update()
    {
        lifeTime += Time.deltaTime;
        float scale = Mathf.Lerp(startSize, endSize, lifeTime / growTime);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
