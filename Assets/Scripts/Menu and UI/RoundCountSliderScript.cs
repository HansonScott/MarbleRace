using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCountSliderScript : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI roundCountLabel;
    [SerializeField] private Slider thisSlider;

    void Start()
    {
        UpdateLabelFromValue();

    }

    public void UpdateLabelFromValue()
    {
        string s = thisSlider.value.ToString();
        s += " Round";
        if(thisSlider.value > 1) { s += "s"; }
        roundCountLabel.text = s;
    }
}
