using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICounterSliderScript : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI AICountLabel;
    [SerializeField] private Slider thisSlider;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLabelFromValue();

    }

    public void UpdateLabelFromValue()
    {
        string s = thisSlider.value.ToString();
        s += " AI";
        if (thisSlider.value > 1) { s += "s"; }
        AICountLabel.text = s;
    }

}
