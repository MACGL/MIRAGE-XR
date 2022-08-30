using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tour))]
public class TourMovement : MonoBehaviour
{
    public Vector3 Movement = new Vector3();

    // Update is called once per frame
    protected virtual void Update()
    {
        Camera.main.transform.Translate(Movement * Time.deltaTime, Space.Self);
    }
}
