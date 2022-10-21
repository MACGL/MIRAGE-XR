using System;
using System.Collections;
using System.Collections.Generic;
using MirageXR;
using UnityEngine;

[RequireComponent(typeof(SetToWorldOrigin))]
public class GaragePlugin : MonoBehaviour
{
    [SerializeField] private SnapModelMenu snapModelMenuPrefab;
    
    private SetToWorldOrigin _setToWorldOrigin;
    private SnapModelMenu _snapModelMenuInstance;
    void Awake()
    {
        _setToWorldOrigin = GetComponent<SetToWorldOrigin>();
    }

    private void Start()
    {
        _snapModelMenuInstance = Instantiate(snapModelMenuPrefab, GameObject.Find("Things").transform);
        _snapModelMenuInstance.ToWorldOrigin = _setToWorldOrigin;
        OnEditModeChanged(RootObject.Instance.activityManager.EditModeActive);
    }

    private void OnEnable()
    {
        EventManager.OnEditModeChanged += OnEditModeChanged;
    }

    private void OnEditModeChanged(bool editModeActive)
    {
        _snapModelMenuInstance.gameObject.SetActive(editModeActive);
    }

    private void OnDisable()
    {
        EventManager.OnEditModeChanged -= OnEditModeChanged;
    }

    private void OnDestroy()
    {
        Destroy(_snapModelMenuInstance.gameObject);
    }
}
