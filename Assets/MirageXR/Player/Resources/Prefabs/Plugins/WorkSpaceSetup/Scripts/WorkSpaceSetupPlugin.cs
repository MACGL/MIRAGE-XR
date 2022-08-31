using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class WorkSpaceSetupPlugin : MonoBehaviour
{
    [SerializeField] private GameObject bucketPrefab, bricksPrefab;
    private GameObject _bucket, _brick;
    
    void Start()
    {
        GetComponentInParent<ObjectManipulator>().enabled = false;
        _bucket = Instantiate(bucketPrefab, transform.position + Vector3.left * 0.5f, Quaternion.identity, transform);
        _brick = Instantiate(bricksPrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity, transform);
    }
}
