﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public PlayerOrder playerOrder;                 // Used to set which player is which in the inspector
    public float mass = 18.0f;                      // Default mass will set player to 180 pounds approximately
    public float speed = 5.0f;                      // Test variables for speed of the gameobject
    private CharacterController control;            // Declares CharacterController for rotation and movement
    private Vector3 gravityVector = Vector3.zero;   // Set an initial velocity for gravity
    public Animator anim;

    //**********************************************************************************************************************//
    // Use this before scene loads
    //**********************************************************************************************************************//
    void Awake()
    {
        //GameManager.Instance.LoadPlayerData(playerOrder, gameObject);   // Always have this in awake to set the player data before game begins
        playerOrder = GetComponent<CharacterStats>().playerOrder;
        
    }
    //**********************************************************************************************************************//

    //**********************************************************************************************************************//
    // Use this for initialization
    void Start()
    {
        
        control = GetComponent<CharacterController>();  // Gets the reference to the CharacterController component
    }
    //**********************************************************************************************************************//

    //**********************************************************************************************************************//
    // Update is called once per frame
    //**********************************************************************************************************************//
    void FixedUpdate()
    {
        Movement();     // Test movement for all players
    }
    //**********************************************************************************************************************//

    void LateUpdate()
    {
        PlayerBoundaries();
    }

    //**********************************************************************************************************************//
    // Test movement for all players
    //**********************************************************************************************************************//
    void Movement()
    {
        Attacks attacks = GetComponent<Attacks>();

        gravityVector += mass * Physics.gravity * Time.deltaTime;
        Vector3 deltaPosition = gravityVector * Time.deltaTime;
        Vector3 move = Vector3.up * deltaPosition.y;

        if (!attacks.IsSpecialAttack)
        {
            float targetX = ControllerManager.Instance.GetLeftStick(playerOrder).x;
            float targetZ = ControllerManager.Instance.GetLeftStick(playerOrder).y;
            float movement = speed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(targetX, 0.0f, targetZ), movement, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
            control.Move(new Vector3(targetX, move.y, targetZ) * movement);

            if (targetX != 0 || targetZ !=0)
            {
                anim.SetBool("IsWalking", true);
            }
            else
                anim.SetBool("IsWalking", false);
        }
    }
    //**********************************************************************************************************************//

    //**********************************************************************************************************************//
    // Test to check each button's functionality
    //**********************************************************************************************************************//
    void ButtonTest()
    {
        if (ControllerManager.Instance.GetAButtonDown(playerOrder))
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if (ControllerManager.Instance.GetBButtonDown(playerOrder))
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (ControllerManager.Instance.GetXButtonDown(playerOrder))
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else if (ControllerManager.Instance.GetYButtonDown(playerOrder))
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }
    //**********************************************************************************************************************//

    //**********************************************************************************************************************//
    // Checks for player boundaries in a rectangular area
    //**********************************************************************************************************************//
    private void PlayerBoundaries()
    {
        CameraControl cam = Camera.main.GetComponent<CameraControl>();

        if (transform.position.x <= cam.GetCenterPoint().x - cam.XLimitFromCenter)
        {
            transform.position = new Vector3(cam.GetCenterPoint().x - cam.XLimitFromCenter, transform.position.y, transform.position.z);
        }
        else if (transform.position.x >= cam.GetCenterPoint().x + cam.XLimitFromCenter)
        {
            transform.position = new Vector3(cam.GetCenterPoint().x + cam.XLimitFromCenter, transform.position.y, transform.position.z);
        }

        if (transform.position.z <= cam.GetCenterPoint().z - cam.ZLimitFromCenter)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, cam.GetCenterPoint().z - cam.ZLimitFromCenter);
        }
        else if (transform.position.z >= cam.GetCenterPoint().z + cam.ZLimitFromCenter)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, cam.GetCenterPoint().z + cam.ZLimitFromCenter);
        }
    }
    //**********************************************************************************************************************//
}
