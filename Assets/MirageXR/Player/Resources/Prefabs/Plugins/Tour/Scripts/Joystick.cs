using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : TourMovement, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _joystickDragSpeed;

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition += (Vector3)eventData.delta * _joystickDragSpeed;
        transform.localPosition = transform.localPosition.normalized * Mathf.Min(transform.localPosition.magnitude, transform.localPosition.normalized.magnitude * 60);
        Movement = transform.localPosition;
        Direction = transform.forward;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        Movement = Vector3.zero;
        Direction = Vector3.zero;
    }
}
