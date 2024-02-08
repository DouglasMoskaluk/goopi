using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerCord : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform target;
    [SerializeField]
    private List<LayerMask> renderLayers;

    // Start is called before the first frame update
    void Start()
    {
        target = SplitScreenManager.instance.GetPlayers(transform.parent.GetComponent<Bullet>().bulletVars.owner).playerGun.gunVars.bulletSpawnPoint[0];
        lineRenderer = GetComponent<LineRenderer>();
        int layerToAdd = (int)Mathf.Log(renderLayers[transform.parent.GetComponent<Bullet>().bulletVars.owner].value, 2);
        transform.gameObject.layer = layerToAdd;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.position);
    }
}
