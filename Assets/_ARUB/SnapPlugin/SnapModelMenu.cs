using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class SnapModelMenu : MonoBehaviour
{
    [SerializeField] private PinchSlider xRotSlider, yRotSlider, zRotSlider, xPosSlider, yPosSlider, ZPosSlider;
    
    [SerializeField] private float  rotationMul = 360, positionMul = 15f;
    

    private SetToWorldOrigin _setToWorldOrigin;

    public SetToWorldOrigin ToWorldOrigin
    {
        get => _setToWorldOrigin;
        set => _setToWorldOrigin = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        xRotSlider.OnValueUpdated.AddListener(XRotSliderUpdate);
        yRotSlider.OnValueUpdated.AddListener(YRotSliderUpdate);
        zRotSlider.OnValueUpdated.AddListener(ZRotSliderUpdate);
        xPosSlider.OnValueUpdated.AddListener(XPosSliderUpdate);
        yPosSlider.OnValueUpdated.AddListener(YPosSliderUpdate);
        ZPosSlider.OnValueUpdated.AddListener(ZPosSliderUpdate);
        xRotSlider.GetComponentInChildren<SliderValue>().mul = rotationMul;
        yRotSlider.GetComponentInChildren<SliderValue>().mul = rotationMul;
        zRotSlider.GetComponentInChildren<SliderValue>().mul = rotationMul;
        xPosSlider.GetComponentInChildren<SliderValue>().mul = positionMul;
        yPosSlider.GetComponentInChildren<SliderValue>().mul = positionMul;
        ZPosSlider.GetComponentInChildren<SliderValue>().mul = positionMul;
    }

    private void XRotSliderUpdate(SliderEventData sliderEventData)
    {
        float value = (sliderEventData.NewValue - 0.5f) * 2f * rotationMul * Time.deltaTime;
        _setToWorldOrigin.AddRotX(value);
    }


    private void YRotSliderUpdate(SliderEventData sliderEventData)
    {
        float value = (sliderEventData.NewValue - 0.5f) * 2f * rotationMul * Time.deltaTime;
        _setToWorldOrigin.AddRotY(value);
    }
    
    private void ZRotSliderUpdate(SliderEventData sliderEventData)
    {
        float value = (sliderEventData.NewValue - 0.5f) * 2f * rotationMul * Time.deltaTime;
        _setToWorldOrigin.AddRotZ(value);
    }
    
    private void XPosSliderUpdate(SliderEventData sliderEventData)
    {
        float value = (sliderEventData.NewValue - 0.5f) * 2f * positionMul * Time.deltaTime;
        _setToWorldOrigin.AddPosX(value / 100f);
    }
    
    private void YPosSliderUpdate(SliderEventData sliderEventData)
    {
        float value = (sliderEventData.NewValue - 0.5f) * 2f * positionMul * Time.deltaTime;
        _setToWorldOrigin.AddPosY(value / 100f);
    }
    
    private void ZPosSliderUpdate(SliderEventData sliderEventData)
    {
        float value = (sliderEventData.NewValue - 0.5f) * 2f * positionMul * Time.deltaTime;
        _setToWorldOrigin.AddPosZ(value / 100f);
        
    }
    
    public void ResetOffset() => _setToWorldOrigin.ResetOffset();
}
