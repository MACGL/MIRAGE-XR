using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using TMPro;
public class SliderValue : MonoBehaviour
{
    public float mul;
    [SerializeField] private string ending = "";
    
    private TextMeshPro text;
    
    public void OnSliderValue(SliderEventData sliderEventData)
    {
        if (text == null) text = GetComponent<TextMeshPro>();
        float value = (sliderEventData.NewValue - 0.5f) * 2f * mul; 
        text.text = value.ToString("F1") + ending;
    }
}
