using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class delaySliderScript : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI delayLabel;
    [SerializeField] private Slider thisSlider;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLabelFromValue();
    }

    public void UpdateLabelFromValue()
    {
        string s = thisSlider.value.ToString();
        s += "s Delay";
        delayLabel.text = s;
    }
}
