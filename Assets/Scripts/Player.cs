using System.Collections;
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
    public float jumpSpeed = 5f;

    private bool[] inputs;
    private float yVelocity = 0;
    private float rotationSpeed = 2f;
    private float smoothRotVelocity;
    private float smoothTime = 0.1f;

    private void Awake()
    {
        _networkComponent = GetComponent<NetworkComponent>();
    }

    protected override void Start()
    {
        if (inventory == null)
            inventory = gameObject.AddComponent<Inventory>();
        if (equipmentManager == null)
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
        //
        Vector3 playerRotation = transform.eulerAngles + new Vector3(0, _inputDirection.x * rotationSpeed, 0);
        transform.eulerAngles = playerRotation;
        //Vector3 _moveDirection = new Vector3(_inputDirection.x, 0f, _inputDirection.y).normalized;
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        if (_moveDirection.sqrMagnitude >= 0.1f * 0.1f)
        {
            //float targetRotation = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
            //float finalRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref smoothRotVelocity, smoothTime);
            //transform.rotation = Quaternion.Euler(0f, finalRotation, 0f);
            _moveDirection *= moveSpeed;
        }

        if (charControl.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;
        _moveDirection.y = yVelocity;

        charControl.Move(_moveDirection);

        //charControl.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }
}
