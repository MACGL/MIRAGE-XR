using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementKeyboard : TourMovement
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void Update()
    {
        Movement = Vector2.zero;
        if (Input.GetKey(KeyCode.Keypad8))
            Movement.z = -1;
        if (Input.GetKey(KeyCode.Keypad2))
            Movement.z = 1;
        if (Input.GetKey(KeyCode.Keypad4))
            Movement.x = 1;
        if (Input.GetKey(KeyCode.Keypad6))
            Movement.x = -1;

        base.Update();
    }
}
