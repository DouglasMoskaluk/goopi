using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditUI : MonoBehaviour
{
    [SerializeField]
    private EventSystem sys;

    [SerializeField]
    private GameObject Exitbutton;

    // Update is called once per frame
    void Update()
    {
        sys.SetSelectedGameObject(Exitbutton);
    }
}
