using UnityEngine;

/// <summary>
/// 
/// This is a singleton class that controls the time scale of the game.
/// It is used to create a slow motion effect when the player hits the enemy.
/// 
/// </summary>

public class TimeController : MonoBehaviour {

    public static TimeController obj = null;

    public float transitionSpeed = 0;

    private float _minTimeScale = 0.1f;
    private float _duration     = 1f;

    private bool  _started = false;
    private bool  _waiting = false;
    private float _backTimeScale = 0;
    private float _velocity = 0;

    private void Awake() {
        if(obj == null) obj = this;
    }

    private void Update() {

        if(!_started) return;
        
        if(!_waiting) {
            if(ApplyTimeScale(_minTimeScale)) {
                _waiting = true;
                _backTimeScale = Time.unscaledTime + _duration;
            }
        } else {
            if(Time.unscaledTime >= _backTimeScale) {
                if(ApplyTimeScale(1f)) {
                    ResetSlotMotion();
                }
            }
        }

    }

    private bool ApplyTimeScale(float targetScale) {
        Time.timeScale = Mathf.SmoothDamp(Time.timeScale, targetScale, ref _velocity, Time.unscaledDeltaTime * transitionSpeed);
        if(Mathf.Abs(Time.timeScale - targetScale) <= 0.01f) {
            return true;
        }
        return false;
    }

    public void SlowDownTime(float minTimeScale, float duration) {
        _started      = true;
        _minTimeScale = minTimeScale;
        _duration     = duration;
    }

    public void SetTimeScale(float value) {
        Time.timeScale = value;
    }

    private void ResetSlotMotion() {
        _started = false;
        _waiting = false;
    }

    public void Reset() {
        ResetSlotMotion();
    }

}
