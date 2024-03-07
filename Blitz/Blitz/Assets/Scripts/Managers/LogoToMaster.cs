using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LogoToMaster : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer vidPlayer;

    private float timeElapsed = 0f;

    private bool isLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed > 2.5f && !vidPlayer.isPlaying && !isLoading)
        {
            isLoading = true;
            SceneManager.LoadScene("MasterScene", LoadSceneMode.Single);
        }

    }
}
