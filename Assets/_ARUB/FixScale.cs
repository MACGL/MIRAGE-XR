using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

public class FixScale : MonoBehaviour
{
    [SerializeField] private Vector3 scale = Vector3.one;

    
    void OnEnable()
    {
        StartCoroutine(SetScale());
    }

    private IEnumerator SetScale()
    {
        yield return null;
        Transform parent = transform.parent;
        transform.parent = null;
        transform.localScale = scale;
        transform.SetParent(parent);
        Debug.LogWarning(transform.lossyScale);
        // transform.localScale = scale * 2f;
        // if (transform.lossyScale == scale) return;
        // transform.localScale = new Vector3(1/transform.lossyScale.x, 
        //     1/transform.lossyScale.y, 
        //     1/transform.lossyScale.z).Mul(scale);
    }
}
