using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICountDownAnimEvent : MonoBehaviour
{
    private int num = 3;
    [SerializeField] private TextMeshProUGUI text;

    public void DecrementNumber()
    {
        num--;
        text.text = num.ToString();
    }
}
