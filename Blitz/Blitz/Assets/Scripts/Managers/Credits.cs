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

    private void Awake()
    {
        ragdolls = new GameObject[devSkinLook.Length];
    }

    [SerializeField]
    UnityEngine.Video.VideoPlayer video;



    public void GoToMainMenu()
    {
        SceneTransitionManager.instance.switchScene(Scenes.MainMenu);
    }

    private void summonNewDev()
    {
        if (devSkinLook.Length <= devsSummoed) return;
        ragdolls[devsSummoed] = Instantiate(Ragdoll, AnimalDropLocation.position, AnimalDropLocation.rotation);
        ragdolls[devsSummoed].GetComponent<PlayerModelHandler>().SetRagdollSkin(devSkinLook[devsSummoed].x);
        ragdolls[devsSummoed].GetComponent<PlayerModelHandler>().SetModel(devSkinLook[devsSummoed].y);
        devsSummoed++;
    }


    private void RewordRoleText(string newText)
    {
        Debug.Log(newText);
        textboxes[0].text = newText;
    }
    private void RewordNameText(string newText)
    {
        Debug.Log(newText);
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
        Debug.Log("STOP! Wait a second.");

        //Time.timeScale = 0.05f;
        StartCoroutine(frozen(frozenTime));
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
        GetComponent<Animator>().speed = 0.1f;
        Transform[] bones;
        for (int j=0; j< devsSummoed; j++)
        {
            bones = ragdolls[j].transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
            for (int i = 0; i < bones.Length; i++)
            {
                Rigidbody rb = bones[i].GetComponent<Rigidbody>();
                if (rb != null) rb.drag = 30;
            }
        }
        yield return new WaitForSecondsRealtime(frozenTime);
        GetComponent<Animator>().speed = 1;
        for (int j = 0; j < devsSummoed; j++)
        {
            bones = ragdolls[j].transform.GetChild(0).GetComponent<BoneRenderer>().transforms;
            for (int i = 0; i < bones.Length; i++)
            {
                Rigidbody rb = bones[i].GetComponent<Rigidbody>();
                if (rb != null) rb.drag = 0;
            }
        }
        //Time.timeScale = 1;
        Debug.Log("This game needs some animal 4 in it.");
    }
}


enum TextElement
{
    Role, Name, Role2, Name2
}
