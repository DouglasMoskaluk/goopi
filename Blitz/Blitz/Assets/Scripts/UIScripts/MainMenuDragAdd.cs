using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MainMenuDragAdd : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] bones;

    private void Awake()
    {
        bones = transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
    }

    public void SetDrag(float newValue)
    {
        for(int i = 0; i < bones.Length; i++)
        {
            if (bones[i].GetComponent<Rigidbody>())
            {
                bones[i].GetComponent<Rigidbody>().drag = newValue;
            }
        }
    }

    public void VelocityOff()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i].GetComponent<Rigidbody>())
            {
                //bones[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                bones[i].GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }

    public void FreezePosition()
    {
        bones[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

}
