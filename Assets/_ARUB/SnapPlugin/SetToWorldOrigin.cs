using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.UI;
using MirageXR;
using TiltBrush;
using UnityEngine;
using Vuforia;

public class SetToWorldOrigin : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(.1f,0f,.1f);
    [SerializeField] public Vector3 rotationOffsetEuler;
    private Quaternion rotationOffset;
    
    private Transform parentTransform;
    private Vector3 _startOffset, _startRotationOffset;

    public Vector3 Offset
    {
        get => offset;
        set => offset = value;
    }

    private void Start()
    {
        parentTransform = GetComponentInParent<PoiEditor>().transform;
        rotationOffset = Quaternion.Euler(rotationOffsetEuler);
        _startOffset = offset;
        _startRotationOffset = rotationOffsetEuler;
    }

    private void Update()
    {
        Transform markerTransform = FindObjectsOfType<ImageTargetBehaviour>().FirstOrDefault(x => x.gameObject.name == "WEKITCalibrationMarker").transform;
        if (markerTransform == null) return;
        if (parentTransform == null) parentTransform = GetComponentInParent<PoiEditor>().transform;
        
        SetTransform(markerTransform);
    }

    public void ResetOffset()
    {
        offset = _startOffset;
        rotationOffsetEuler = _startRotationOffset;
        rotationOffset = Quaternion.Euler(rotationOffsetEuler);
    }
    public void AddRotX(float value)
    {
        rotationOffsetEuler.x += value;
        rotationOffset = Quaternion.Euler(rotationOffsetEuler);
    }
    public void AddRotY(float value)
    {
        rotationOffsetEuler.y += value;
        rotationOffset = Quaternion.Euler(rotationOffsetEuler);
    }
    public void AddRotZ(float value)
    {
        rotationOffsetEuler.z += value;
        rotationOffset = Quaternion.Euler(rotationOffsetEuler);
    }
    public void AddPosX(float value)
    {
        offset.x += value;
    }
    public void AddPosY(float value)
    {
        offset.y += value;
    }
    public void AddPosZ(float value)
    {
        offset.z += value;
    }
    private void SetTransform(Transform markerTransform)
    {
        Vector3 relativeOffset =
            markerTransform.right * offset.x + markerTransform.up * offset.y + markerTransform.forward * offset.z;
        parentTransform.position = markerTransform.position + relativeOffset;
        parentTransform.rotation = markerTransform.rotation * rotationOffset;
    }
}
