using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDecal : MonoBehaviour
{

    [SerializeField]
    private float lifetime = 5.0f;

    // Start is called before the first frame update
    void Awake()
    {
        EventManager.instance.addListener(Events.onRoundStart, RemoveSelf);
        StartCoroutine("Countdown");
    }

    public void Initialize()
    {

    }

    public void RemoveSelf(EventParams param = new EventParams())
    {
        Destroy(gameObject);
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}
