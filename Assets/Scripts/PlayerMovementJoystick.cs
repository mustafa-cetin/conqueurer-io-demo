using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementJoystick : MonoBehaviour
{
    public Joystick joystick;
    float playerspeed;
    Rigidbody2D rigidbody;

    public void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

        playerspeed = GetComponent<Lord>().speed;
    }

    public void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(joystick.Horizontal * playerspeed * Time.fixedDeltaTime, joystick.Vertical * playerspeed * Time.fixedDeltaTime);
    }
}
