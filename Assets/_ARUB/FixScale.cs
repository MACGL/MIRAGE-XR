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
    }
}
