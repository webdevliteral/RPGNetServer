﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkComponent))]
public class Player : Entity
{
    public Focus focus;
    public Inventory inventory;
    public EquipmentManager equipmentManager;
    public int id;
    public string username;
    public CharacterController charControl;
    public float gravity = -9.18f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;

    private bool[] inputs;
    private float yVelocity = 0;

    private void Awake()
    {
        _networkComponent = GetComponent<NetworkComponent>();
    }

    private void Start()
    {
        if (inventory == null)
            inventory = gameObject.AddComponent<Inventory>();
        if(equipmentManager == null)
            equipmentManager = gameObject.AddComponent<EquipmentManager>();
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
        focus = GetComponent<Focus>();   
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;

        //a bool array the length of all readable inputs
        inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.x -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.y -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }

        Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if(charControl.isGrounded)
        {
            yVelocity = 0f;
            if(inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;
        _moveDirection.y = yVelocity;
        charControl.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }
}
