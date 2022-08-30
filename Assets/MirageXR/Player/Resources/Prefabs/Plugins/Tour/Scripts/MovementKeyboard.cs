using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementKeyboard : TourMovement
{
    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private Vector2 deltaPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void Update()
    {
        Movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            Movement.z = -1;
        if (Input.GetKey(KeyCode.S))
            Movement.z = 1;
        if (Input.GetKey(KeyCode.A))
            Movement.x = 1;
        if (Input.GetKey(KeyCode.D))
            Movement.x = -1;

        base.Update();
    }
}
