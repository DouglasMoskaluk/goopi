using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject textObject;

    private TextMeshProUGUI text;

    private bool isCounting = true;

    void Start()
    {
        text = textObject.transform.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator startCountdown()
    {
        yield return null;
    }

}
