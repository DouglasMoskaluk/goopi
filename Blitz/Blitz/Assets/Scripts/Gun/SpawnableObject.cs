using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    [SerializeField]
    internal int Owner = -1;
    private bool listening = false;

    internal virtual void init(int owner)
    {
        Owner = owner;
        EventManager.instance.addListener(Events.onRoundEnd, roundEnd);
        listening = true;
    }

    internal virtual void roundEnd(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }

    ~SpawnableObject()
    {
        if (listening) EventManager.instance.removeListener(Events.onRoundEnd, roundEnd);
    }
}
