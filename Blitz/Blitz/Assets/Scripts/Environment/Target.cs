using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    UnityEvent newEvent;

    void Start()
    {
        
    }

    public void BulletHit()
    {
        Debug.Log("hit target");
        newEvent.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag == "Bullet")
    //    {
    //        Debug.Log("TEST");
    //        //newEvent.Invoke();
    //    }
    //}

}
