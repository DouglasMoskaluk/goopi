using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    [SerializeField]
    internal int Owner;

    internal virtual void init(int owner)
    {
        Owner = owner;
    }
}
