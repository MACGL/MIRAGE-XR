using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.MixedReality.Toolkit.UI;
using MirageXR;
using UnityEngine;

public class WorkSpaceSetupPlugin : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private GameObject placementSphere;
    [SerializeField] private List<GameObject> _objects;
    
    
    [SerializeField] private float zOffset = -1;
    
    private GameObject _bucket, _brick;
    
    void Start()
    {
        _objects = new List<GameObject>();
        Vector3 offsetDir = Vector3.left;
        foreach (var prefab in prefabs)
        {
            GameObject newObj = Instantiate(prefab, transform.position + offsetDir + Vector3.up * zOffset, Quaternion.identity);
            offsetDir = Quaternion.Euler(0, 360 / prefabs.Count, 0) * offsetDir;
            newObj.transform.SetParent(transform, true);
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
        foreach (var obj in _objects)
            obj.GetComponent<Collider>().enabled = !editModeActive;
    }

    private void OnDisable()
    {
        EventManager.OnEditModeChanged -= OnEditModeChanged;
    }
}
