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

    public void ChangeKillsDisplay(int kills)
    {
        slider.value = kills;
        killText.text = kills.ToString();
    }

    public void SetKillsMax(int to)
    {
        slider.maxValue = to;
    }

    public void SetAnimalSprite(Sprite s)
    {
        animalHead.sprite = s;
    }
}
