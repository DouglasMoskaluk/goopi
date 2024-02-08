using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerCord : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform target;
    [SerializeField]
    private List<LayerMask> renderLayers;
    internal int owner;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.GetComponent<Bullet>() != null)
        {
            owner = transform.parent.GetComponent<Bullet>().bulletVars.owner;
            target = SplitScreenManager.instance.GetPlayers(owner).playerGun.gunVars.bulletSpawnPoint[0];
            lineRenderer = GetComponent<LineRenderer>();
            int layerToAdd = (int)Mathf.Log(renderLayers[owner].value, 2);
            transform.gameObject.layer = layerToAdd;
        }
    }

    internal void init(int Owner)
    {
        owner = Owner;
        target = SplitScreenManager.instance.GetPlayers(owner).playerGun.gunVars.bulletSpawnPoint[0];
        lineRenderer = GetComponent<LineRenderer>();
        int layerToAdd = (int)Mathf.Log(renderLayers[owner].value, 2);
        transform.gameObject.layer = layerToAdd;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.position);
    }
}
