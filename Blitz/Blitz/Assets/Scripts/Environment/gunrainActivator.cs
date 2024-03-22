using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunrainActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private GameObject gunHolder;
    // Start is called before the first frame update

    private void Start()
    {
        EventManager.instance.addListener(Events.onRoundStart, enableObjects);
        EventManager.instance.addListener(Events.onPlayStart, disableObjects);
    }

    public void disableObjects(EventParams par = new EventParams())
    {
        camera.SetActive(false);

        for(int i = 0; i < gunHolder.transform.childCount; i++)
        {
            gunHolder.transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    public void enableObjects(EventParams par = new EventParams())
    {
        camera.SetActive(true);

        for (int i = 0; i < gunHolder.transform.childCount; i++)
        {
            gunHolder.transform.GetChild(i).gameObject.SetActive(true);
        }

    }
}
