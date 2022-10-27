using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirageXR;

public class PictureView : MonoBehaviour
{
    [SerializeField] private GameObject picture;

    private void Start()
    {
        transform.localRotation = Quaternion.identity;
        OnEditModeChanged(RootObject.Instance.activityManager.EditModeActive);
    }

    private void OnEnable()
    {
        EventManager.OnEditModeChanged += OnEditModeChanged;
    }

    private void OnEditModeChanged(bool editModeActive)
    {
        GetComponentInParent<BoxCollider>().enabled = editModeActive;
        picture.SetActive(editModeActive);
    }

    private void OnDisable()
    {
        EventManager.OnEditModeChanged -= OnEditModeChanged;
    }
    
    public void TogglePicture()
    {
        picture.SetActive(!picture.activeSelf);
    }
}
