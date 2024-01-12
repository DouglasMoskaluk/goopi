using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    internal int Owner;

    internal void init(int owner)
    {
        Owner = owner;
    }
}
