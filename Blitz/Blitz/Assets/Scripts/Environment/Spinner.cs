using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform spinPoint;

    private IEnumerator fastSpinCoro;

    [SerializeField]
    public float flingForce = 20.0f;

    private bool fast = false;

    [SerializeField]
    private BoxCollider[] colliders;

    void Start()
    {
        spinPoint = transform.GetChild(0);
        //slowSpinCoro = RegularRotation();
        StartCoroutine(RegularRotation());
        fastSpinCoro = FastRotation();
        //StartCoroutine(slowSpinCoro);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {

    //        Vector3 dir = ((other.transform.position + Vector3.up * 2) - transform.position).normalized;//the Vector3.up will have to be changed to corrolate with the players height roughly, getting direction to head gives more upwards force which i think feels better ~jordan
    //        PlayerBodyFSM fsm = other.GetComponent<PlayerBodyFSM>();
    //        fsm.addKnockBack(dir * flingForce);
    //        fsm.transitionState(PlayerMotionStates.KnockBack);
    //    }
    //}

    public void FastSpin()
    {
            fast = true;
            StopCoroutine(fastSpinCoro);
            fastSpinCoro = FastRotation();
            StartCoroutine(fastSpinCoro);
    }

    IEnumerator RegularRotation()
    {
        while (true)
        {
            spinPoint.Rotate(0.0f, 180 * Time.deltaTime, 0.0f);
            yield return null;
        }
    }

    IEnumerator FastRotation()
    {

        flingForce = 40.0f;

        float timer = 0f;

        while(timer < 10)
        {
            timer += Time.deltaTime;
            spinPoint.Rotate(0.0f, 330 * Time.deltaTime, 0.0f);
            yield return null;
        }

        flingForce = 20.0f;

    }

}
