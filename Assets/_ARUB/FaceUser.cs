using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceUser : MonoBehaviour
{
    [SerializeField] private bool backwards = false;
    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToTarget = transform.position - cam.transform.position;
        transform.rotation = Quaternion.LookRotation(backwards ? directionToTarget : -directionToTarget);
    }
}
