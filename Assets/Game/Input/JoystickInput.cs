using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/// <summary>
/// 
/// This script handles player touch input using Unity's new Input System (version 1.7).
/// It detects touches and calculates the movement direction based on the last touch position.
/// 
/// NOTE:
/// For simplicity I'm using the EnhancedTouch.Touch class. 
/// All the information about the methods I used here is in the links below.
/// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/Touch.html#enhancedtouchtouch-class
/// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/api/UnityEngine.InputSystem.EnhancedTouch.Touch.html#
/// 
/// </summary>
public class JoystickInput : MonoBehaviour {

    public Vector2 StickDirection => _touchDirection;

    [HideInInspector]
    public bool isTouchActive  = false;

    [Tooltip("How much the touch will affect the direction")]
    public float moveSensitivity = 0.5f;
    [Tooltip("The maximum length of the touch direction")]
    public float moveMaxLength   = 1f;

    public Vector2 touchStartPosition => _touchStartPosition;

    private Vector2 _touchStartPosition = Vector2.zero;
    private Vector2 _touchDirection     = Vector2.zero;

    private Finger _activeFinger = null;

    private void Awake() {
        EnhancedTouchSupport.Enable();
    }

    private void Update() {
        CalculateTouchDirection();
    }

    private void CalculateTouchDirection() {

        if(_activeFinger == null) return;

        Vector2 touchPosition = _activeFinger.currentTouch.screenPosition;

        _touchDirection = (touchPosition - _touchStartPosition) * moveSensitivity;

        // Limit the touch direction to the maximum length
        _touchDirection = Vector2.ClampMagnitude(_touchDirection, moveMaxLength);

    }

    private void OnFingerDown(Finger finger) {
        isTouchActive = true;
        _touchStartPosition = finger.currentTouch.screenPosition;
    }

    private void OnFingerMove(Finger finger) {
        _activeFinger = finger;
    }

    private void OnFingerUp(Finger finger) {
        isTouchActive   = false;
        _touchDirection = Vector2.zero;
        _activeFinger   = null;
    }

    private void OnEnable() {

        TouchSimulation.Enable();

        // Subscribe to touch events
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp   += OnFingerUp;
        Touch.onFingerMove += OnFingerMove;

    }
    private void OnDisable() {

        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp   -= OnFingerUp;
        Touch.onFingerMove -= OnFingerMove;

        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();

    }

}
