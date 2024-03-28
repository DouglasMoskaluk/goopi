using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Credits : MonoBehaviour
{
    [SerializeField]
    GameObject Ragdoll;
    [SerializeField]
    Vector2Int[] devSkinLook;

    [SerializeField]
    Transform AnimalDropLocation;

    [SerializeField]
    TMP_Text[] textboxes;

    int devsSummoed = 0;

    GameObject[] ragdolls;
    IEnumerator freezeCR;

    private void Awake()
    {
        ragdolls = new GameObject[devSkinLook.Length];
    }

    [SerializeField]
    UnityEngine.Video.VideoPlayer video;



    public void GoToMainMenu()
    {
        Debug.Log("Switching");
        for (int i=0; i<ragdolls.Length; i++)
        {
            if (ragdolls[i] != null)
            {
                Destroy(ragdolls[i], 2);
            }
        }
        if (freezeCR != null) StopCoroutine(freezeCR);
        SceneTransitionManager.instance.switchScene(Scenes.MainMenu);
    }

    private void summonNewDev()
    {
        if (devSkinLook.Length <= devsSummoed) return;
        ragdolls[devsSummoed] = Instantiate(Ragdoll, AnimalDropLocation.position, AnimalDropLocation.rotation);
        ragdolls[devsSummoed].GetComponent<PlayerModelHandler>().SetRagdollSkin(devSkinLook[devsSummoed].x);
        ragdolls[devsSummoed].GetComponent<PlayerModelHandler>().SetModel(devSkinLook[devsSummoed].y);

        /*Transform[] bones = ragdolls[devsSummoed].transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
        for (int i = 0; i < bones.Length; i++)
        {
            Rigidbody rb = bones[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(Random.Range(-2f, 2f) * 2 / (i + 2), Random.Range(-2f, 2f) * 2 / (i + 2), Random.Range(-2f, 2f) * 2 / (i + 2));
                rb.velocity *= 5;
            }
        }*/

        devsSummoed++;
    }


    private void RewordRoleText(string newText)
    {
        //Debug.Log(newText);
        textboxes[0].text = newText;
    }
    private void RewordNameText(string newText)
    {
        //Debug.Log(newText);
        textboxes[1].text = newText;
    }

    private void RewordRoleText2(string newText)
    {
        textboxes[2].text = newText;
    }
    private void RewordNameText2(string newText)
    {
        textboxes[3].text = newText;
    }


    private void freezeFrame(float frozenTime)
    {
        //Debug.Log("STOP! Wait a second.");

        //Time.timeScale = 0.05f;
        freezeCR = frozen(frozenTime);
        GetComponent<Animator>().speed = 0.1f;
        for (int j = 0; j < devsSummoed; j++)
        {
            Transform[] bones = ragdolls[j].transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
            for (int i = 0; i < bones.Length; i++)
            {
                Rigidbody rb = bones[i].GetComponent<Rigidbody>();
                if (rb != null) rb.drag = 30;
            }
        }
        StartCoroutine(freezeCR);
    }

    private void ram(float frozenTime)
    {
        freezeCR = frozen(frozenTime);
        GetComponent<Animator>().speed = 0.1f;
        for (int j = 0; j < devsSummoed; j++)
        {
            Transform[] bones = ragdolls[j].transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
            if (j != devsSummoed - 1)
            {
                for (int i = 0; i < bones.Length; i++)
                {
                    Rigidbody rb = bones[i].GetComponent<Rigidbody>();
                    if (rb != null) rb.drag = 0;
                }
            } else
            {
                for (int i = 0; i < bones.Length; i++)
                {
                    Rigidbody rb = bones[i].GetComponent<Rigidbody>();
                    if (rb != null) rb.drag = 30;
                }
            }
        }
        StartCoroutine(freezeCR);
    }

    private void playVideo()
    {
        video.Play();
    }

    private void pauseVideo()
    {
        video.Pause();
    }

    private IEnumerator frozen(float frozenTime)
    {
           
        yield return new WaitForSecondsRealtime(frozenTime);
        GetComponent<Animator>().speed = 1;
        
        for (int j = 0; j < devsSummoed; j++)
        {
            Transform[] bones = ragdolls[j].transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
            for (int i = 0; i < bones.Length; i++)
            {
                Rigidbody rb = bones[i].GetComponent<Rigidbody>();
                if (rb != null) rb.drag = 0;
            }
        }
        //Time.timeScale = 1;
        freezeCR = null;
        Debug.Log("This game needs some animal 4 in it.");
    }
}


enum TextElement
{
    Role, Name, Role2, Name2
}
