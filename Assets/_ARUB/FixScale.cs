using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

public class FixScale : MonoBehaviour
{
    [SerializeField] private Vector3 scale = Vector3.one;

    
    IEnumerator Do()
    {
        SetScale();
        yield return null;
        SetScale();
    }

    private void OnEnable()
    {
        //StartCoroutine(Do());
    }

    private void Update()
    {
        //SetScale();
    }

    private void SetScale()
    {
        transform.localScale *= 2f;
        if (transform.lossyScale == scale) return;
        transform.localScale = new Vector3(1/transform.lossyScale.x, 
            1/transform.lossyScale.y, 
            1/transform.lossyScale.z).Mul(scale);
    }
}
