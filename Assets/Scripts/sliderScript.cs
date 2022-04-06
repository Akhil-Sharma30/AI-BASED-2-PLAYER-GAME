using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderScript : MonoBehaviour
{
    private Slider slider;

    private Text sliderText;

    private void Awake()
    {
        slider = GetComponentInParent<Slider>();
        sliderText = GetComponent<Text>();
    }

    private void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    void UpdateText(float val)
    {
        sliderText.text = slider.value.ToString();
    }
}
