using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHorizontal : MonoBehaviour
{

    [SerializeField] private Transform follow;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0 , follow.eulerAngles.y, follow.eulerAngles.z);

        Debug.DrawRay(transform.position, transform.forward, Color.magenta);
    }
}
