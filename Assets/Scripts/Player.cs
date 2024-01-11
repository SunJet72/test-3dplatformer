using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    [Serializable]
    private class InputSettings
    {
        public string FORWARD_AXIS = "Vertical";
        public string SIDEWAYS_AXIS = "Horizontal";
        public string TURN_AXIS = "Mouse X";
        public string JUMP_AXIS = "Jump";
    }
    [SerializeField] private InputSettings inputSettings;

    [Serializable]
    private class MoveSettings
    {
        public float runVelocity = 12f, rotateVelocity = 100f, jumpVelocity = 8f, distanceToGround = 1.3f;
        public LayerMask ground;
    }
    [SerializeField] private MoveSettings moveSettings;

    [SerializeField] private Transform spawnPoint;


    #region InputValues

    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private Quaternion _targetRotation;
    private float _forwardInput, _sidewaysInput, _turnInput, _jumpInput;

    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _velocity = Vector3.zero;
        _targetRotation = transform.localRotation;
    }

    private bool isSpawned = false;

    private void Update()
    {
        GetInput();
        Turn();
    }

    private void FixedUpdate()
    {
        if (!isSpawned)
        {
            isSpawned = true;
            Spawn();
        }
        Run();
        Jump();
    }

    private void Spawn()
    {
        transform.position = spawnPoint.position;
    }

    private void GetInput()
    {
        _forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
        _sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);
        _turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
        _jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
    }

    private void Turn()
    {
        if (_turnInput == 0) return;

        var amtToRotate = moveSettings.rotateVelocity * _turnInput * Time.deltaTime;
        _targetRotation *= Quaternion.AngleAxis(amtToRotate, Vector3.up);
        transform.rotation = _targetRotation;
    }

    private void Run()
    {
        _velocity.x = _sidewaysInput * moveSettings.runVelocity;
        _velocity.y = _rigidbody.velocity.y;
        _velocity.z = _forwardInput * moveSettings.runVelocity;

        _rigidbody.velocity = transform.TransformDirection(_velocity);
    }

    private void Jump()
    {
        if (_jumpInput == 0 || !IsGrounded()) return;

        _velocity.x = _rigidbody.velocity.x;
        _velocity.y = moveSettings.jumpVelocity;
        _velocity.z = _rigidbody.velocity.z;

        _rigidbody.velocity = _velocity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down,
            moveSettings.distanceToGround, moveSettings.ground);
    }
}