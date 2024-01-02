using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    internal GameObject Owner;

    internal void init(GameObject owner)
    {
        Owner = owner;
    }
}
