using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTieKillsIndicator : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private Image animalHead;

    public void ChangeKillsDisplay(float kills)
    {
        slider.value = Mathf.Clamp(kills, 0, slider.maxValue);
        killText.text = Mathf.Clamp(kills, 0, slider.maxValue).ToString();
    }

    public void SetKillsMax(float to)
    {
        slider.maxValue = to;
    }

    public void SetAnimalSprite(Sprite s)
    {
        animalHead.sprite = s;
    }

    public float GetKillsNum()
    {
        return slider.value;
    }

    public void IncrementSlider()
    {
        slider.value++;
    }

    public bool AtMaxValue()
    {
        return slider.value >= slider.maxValue;
    }
}
