using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tour))]
public class TourMovement : MonoBehaviour
{
    protected Vector3 Movement = new Vector3();
    [SerializeField] float Speed;


    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3 forwardHorizontal = Camera.main.transform.forward * Movement.z + Camera.main.transform.right * Movement.x;
        forwardHorizontal.y = 0;
        //Camera.main.transform.Translate(Movement * Time.deltaTime, Space.Self);
        Tour.Singleton.contentContainer.Translate(forwardHorizontal.normalized * Speed);
        //Tour.Singleton.contentContainer.Translate(Movement * Speed, Space.Self);
    }
}
