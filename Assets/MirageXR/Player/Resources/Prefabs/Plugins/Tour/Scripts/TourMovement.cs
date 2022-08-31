using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tour))]
public class TourMovement : MonoBehaviour
{
    protected Vector3 Movement = new Vector3();
    protected Vector3 Direction = new Vector3();
    private Vector3 TranslatedMovement = new Vector3();
    [SerializeField] float Speed;


    // Update is called once per frame
    protected virtual void Update()
    {
        TranslatedMovement = Direction * Movement.y + Quaternion.Euler(0, 90, 0) * Direction * Movement.x;
        TranslatedMovement.y = 0;
        Tour.Singleton.contentContainer.Translate(TranslatedMovement.normalized * Speed);
    }
}
