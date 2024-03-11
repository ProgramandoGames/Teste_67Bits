using System;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(JoystickInput))]
public class Player : MonoBehaviour {

    private Movement       _movement;
    private Punch          _punch;
    private JoystickInput  _joystick;
    private EnemyStacking  _enemyStacking;
    private Animator       _animator;
    private UpgradeManager _upgradeManager;

    private Material _bodyMat;

    private Vector3 _input;

    public Color startColor;
    public Color finalColor;

    private void Awake() {

        _movement       = GetComponent<Movement>();
        _punch          = GetComponent<Punch>();
        _joystick       = GetComponent<JoystickInput>();
        _enemyStacking  = GetComponent<EnemyStacking>();
        _animator = transform.GetChild(0).GetComponent<Animator>();

        _upgradeManager = FindAnyObjectByType<UpgradeManager>();

        _bodyMat = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        _bodyMat.color = startColor;

        _punch.OnPunch += OnPunch;

    }

    void Start() {
        
    }

    public void _Update() {

        _input = new Vector3(_joystick.StickDirection.x,
                             0,
                             _joystick.StickDirection.y);

        _animator.SetBool("running", _input.sqrMagnitude > 0);
        _animator.ResetTrigger("punch");
        
        _punch._Update();
        _movement._Update(_input);
        _enemyStacking._Update(_input);

        _bodyMat.color = Color.Lerp(startColor, finalColor, _upgradeManager.currentLevel / 10f);

    }

    public void OnPunch() {

        _animator.SetTrigger("punch");
        _input = Vector3.zero;

    }

    public void SetMaxCapacityEvent(Action<string> func) {
        _enemyStacking.OnMaxCapacity += func;
    }

    public void OnDisable() {
        _punch.OnPunch -= OnPunch;
    }

}
