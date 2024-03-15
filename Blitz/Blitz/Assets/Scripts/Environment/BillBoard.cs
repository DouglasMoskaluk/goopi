using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BillBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject[] imageObject;

    private Transform[] lookToPoint;



    // Start is called before the first frame update
    void Awake()
    {
        List<PlayerInput> playersList = SplitScreenManager.instance.GetPlayers();
        lookToPoint = new Transform[playersList.Count];
        for(int i = 0; i < playersList.Count; i++)
        {
            lookToPoint[i] = playersList[i].transform.GetChild(0).GetChild(0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < lookToPoint.Length;i++)
        {
            imageObject[i].transform.LookAt(lookToPoint[i]);
        }
    }
}
