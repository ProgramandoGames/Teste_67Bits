using UnityEngine;

/// <summary>
/// 
/// This script manages the joystick's knob position based on user touch input. 
/// It achieves this by adjusting the knob's position relative to the touch location,
/// utilizing a reference to the JoystickInput script to access the current 
/// touch position data and state.
/// 
/// </summary>
public class UI_JoystickPad : MonoBehaviour {

    private RectTransform _background;
    private RectTransform _knob;
    private JoystickInput _joystickInput;

    [Tooltip("The maximum offset the knob can move from the center of the pad")]
    public float stickMaxOffset = 50f;

    private void Awake() {

        _joystickInput = FindFirstObjectByType<JoystickInput>();
        _background    = transform.GetChild(0).GetComponent<RectTransform>();
        _knob          = _background.GetChild(0).GetComponent<RectTransform>();


    }

    private void Update() {

        SetKnobPosition();

    }

    private void SetKnobPosition() {

        // If the touch is not active, hide the joystick and stop the function
        if(!_joystickInput.isTouchActive) {
            _background.anchoredPosition = Vector2.left * 10000;
            return;
        } else {
            _background.anchoredPosition = _joystickInput.touchStartPosition;
        }

        _knob.anchoredPosition = _joystickInput.StickDirection * stickMaxOffset;

    }

}
