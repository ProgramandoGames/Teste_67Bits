using UnityEngine;

/// <summary>
/// 
/// Implements generic character movement using the 
/// character controller component.
/// This script provides basic movement functionality for any entity.
/// The `_Update` method handles movement based on a provided direction.
/// 
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour {

    private CharacterController _controller;

    public float maxSpeed      = 10f;
    public float acceleration  = 10f;
    public float rotationSpeed = 10f;

    public float gravity = 10f;

    [Tooltip("If true, the forward direction will change according to the movement direction")]
    public bool changeForward  = true;

    private Vector3 _direction;
    private Vector3 _horizontalVelocity;
    private Vector3 _verticalVelocity;
    private Vector3 _refVelocity;


    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    public void _Update(Vector3 direction) {

        _direction = direction;

        _horizontalVelocity = Vector3.SmoothDamp(_horizontalVelocity, _direction * maxSpeed, ref _refVelocity, 1 / acceleration);

        _verticalVelocity += Vector3.down * gravity * Time.deltaTime;

        Vector3 finalVelocity = _horizontalVelocity + _verticalVelocity;

        _controller.Move(finalVelocity * Time.deltaTime);

        if(changeForward) {
            transform.forward = Vector3.Lerp(transform.forward, _direction, rotationSpeed * Time.deltaTime);
        }

    }

    public void SetColliderPosition(Vector3 position) {
        _controller.center = position;
    }

    public void Jump(float jumpForce) {
        _verticalVelocity = Vector3.up * jumpForce;
    }

}
