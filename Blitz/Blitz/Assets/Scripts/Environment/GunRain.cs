using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRain : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform startPoint;

    private Rigidbody rb;

    [SerializeField]
    private GameObject[] gunModels;

    void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        //rb.velocity = new Vector3(0, -13.5f, 0);
    }

    private void Start()
    {
        ChangeGun();
        //ResetHeight();
    }

    // Update is called once per frame
    //void Update()
    //{
    //   rb.velocity = new Vector3(0, -13.5f, 0);
    //}

    private void FixedUpdate()
    {
        transform.Translate(0f, -0.01f, 0f);
    }

    private void ChangeGun()
    {
        int newGunInt = Random.Range(0, gunModels.Length);

        for(int i = 0; i < gunModels.Length; i++)
        {
            if(i == newGunInt)
            {
                gunModels[i].gameObject.SetActive(true);
            }
            else
            {
                gunModels[i].gameObject.SetActive(false);
            }
        }

    }

    private void ResetHeight()
    {
        //rb.velocity = new Vector3(0, -13.5f, 0);
        transform.position = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Finish"))
        {
            ResetHeight();
            ChangeGun();
        }
    }

}
