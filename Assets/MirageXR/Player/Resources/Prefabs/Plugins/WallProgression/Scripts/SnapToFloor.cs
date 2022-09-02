using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToFloor : MonoBehaviour
{
    [SerializeField] private LayerMask lm;
    
    void Update()
    {
        PlaceOnFloor();
    }

    void PlaceOnFloor()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 10f, Vector3.down);
        if (Physics.Raycast(ray, out hit, 10000, lm))
        {
            transform.position = hit.point;
        }
    }
}
