using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using MirageXR;
using UnityEngine;

public class WorkSpaceSetupPlugin : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private GameObject placementSphere;
    [SerializeField] private float yOffset = -1;
    [SerializeField] private float distanceBetwenItems = 1f;
    private List<GameObject> _objects;
    
    
    
    private GameObject _bucket, _brick;
    
    void Start()
    {
        transform.localRotation = Quaternion.identity;
        GameObject pluginParent = GetComponentInParent<ConstraintManager>().gameObject;
        
        var rotationAxisConstraint = pluginParent.AddComponent<RotationAxisConstraint>();
        if(rotationAxisConstraint == null) rotationAxisConstraint = pluginParent.AddComponent<RotationAxisConstraint>();
        rotationAxisConstraint.ConstraintOnRotation = AxisFlags.XAxis | AxisFlags.ZAxis;
        _objects = new List<GameObject>();
        Vector3 offsetDir = Vector3.left;
        foreach (var prefab in prefabs)
        {
            GameObject newObj = Instantiate(prefab, transform);
            _objects.Add(newObj);
        }
        OnEditModeChanged(RootObject.Instance.activityManager.EditModeActive);
    }

    private void OnEnable()
    {
        EventManager.OnEditModeChanged += OnEditModeChanged;
    }

    private void OnEditModeChanged(bool editModeActive)
    {
        placementSphere.SetActive(editModeActive);
        float leftOffset = -_objects.Count / 2f * distanceBetwenItems;
        for(int i = 0; i < _objects.Count; i++)
        {
            var obj = _objects[i];
            obj.GetComponent<Collider>().enabled = !editModeActive;
            float xPos = leftOffset + (i * distanceBetwenItems);
            obj.transform.localPosition = Vector3.right * xPos + Vector3.up * yOffset;
        }
    }

    private void OnDisable()
    {
        EventManager.OnEditModeChanged -= OnEditModeChanged;
    }
}
