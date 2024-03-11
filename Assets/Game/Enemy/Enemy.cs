using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 
/// Implements a simple enemy AI.
/// The enemy will move to a random position and then stop for a while,
/// This process is repeated indefinitely, until the enemy is hit by the player.
/// 
/// </summary>
[RequireComponent(typeof(Movement))]
public class Enemy : MonoBehaviour{

    enum STATTE {
        IDLE,
        GENERATE_RANDOM_POSITION,
        MOVING,
        DEAD
    }

    public Action<Enemy> Destroyed;

    private STATTE  _currentState;

    private Movement _movement;
    private Animator _animator;

    public float idleDelay     = 2f;
    public float moveLimits    = 9f;
    public float desceleration = 5f;
    public float deadDelay     = 1f;

    public bool done {
        get { return _done; }
    }

    private List<Rigidbody> _bodies = new List<Rigidbody>();

    private Material _bodyMat;
    private Vector3  _direction;
    private Vector3  _targetPosition;
    private float    _idleTimeEnd = 0;
    private float    _deadTimeEnd = 0;
    private bool     _done        = false;

    private Transform _hips;

    private void Awake() {

        _movement = GetComponent<Movement>();

        _animator = transform.GetChild(0).GetComponent<Animator>();

        _bodyMat = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material;

        _hips = transform.GetChild(0).GetChild(4);

        _currentState = STATTE.IDLE;
        _idleTimeEnd  = Time.time + idleDelay;

        _bodies = GetComponentsInChildren<Rigidbody>().ToList();
        SetActiveRagdoll(false);

    }

    void Update() {

        // NOTE:
        // To simplify this test, I opted for a switch statement instead
        // of the usual approach of using a dedicated class for each state.
        // This keeps all the code within this script.
        switch(_currentState) {
            case STATTE.IDLE:
                Idle();
                break;
            case STATTE.GENERATE_RANDOM_POSITION:
                GenerateRandomPosition();
                break;
            case STATTE.MOVING:
                Moving();
                break;
            case STATTE.DEAD:
                Dead();
                break;

        }

        if(transform.position.y <= -10) {
            Destroy(gameObject);
        }

    }

    private void Idle() {

        if(Time.time > _idleTimeEnd) {
            _currentState = STATTE.GENERATE_RANDOM_POSITION;
        }
        _animator.SetBool("running", false);


    }

    private void GenerateRandomPosition() {

        _targetPosition = new Vector3(UnityEngine.Random.Range(-moveLimits, moveLimits), 0,
                                      UnityEngine.Random.Range(-moveLimits, moveLimits));

        _direction = (_targetPosition - new Vector3(transform.position.x, 0, transform.position.z)).normalized;

        _currentState = STATTE.MOVING;

    }

    private void Moving() {

        if((new Vector3(transform.position.x, 0, transform.position.z) - _targetPosition).sqrMagnitude <= 0.5f) {
            _currentState = STATTE.IDLE;
            _idleTimeEnd  = Time.time + idleDelay;
            return;
        }

        _movement._Update(_direction);

        _animator.SetBool("running", true);

    }

    private void Dead() {

        _direction = Vector3.Lerp(_direction, Vector3.zero, Time.deltaTime * desceleration);
        _movement._Update(_direction);
        transform.rotation = Quaternion.Euler(90, 0, 0);

        // A delay to allow the enemy to fall to the ground
        // before the player can stack it.
        if(Time.time > _deadTimeEnd) {
            _done = true;
            SetActiveRagdoll(false);
            _bodyMat.color = Color.white;
            _movement.SetColliderPosition(_hips.localPosition);
        }

    }

    public void Hit(Vector3 punchDirection, float punchForce) {

        if(_currentState == STATTE.DEAD) return;

        SetActiveRagdoll(true);

        _currentState = STATTE.DEAD;
        _direction    = punchDirection * punchForce;
        _movement.Jump(punchForce);

        _deadTimeEnd = Time.time + deadDelay;

        _animator.enabled = false;

    }

    private void SetActiveRagdoll(bool value) {
        foreach(Rigidbody body in _bodies) {
            body.isKinematic = !value;
            if(value)
                body.velocity = Vector3.zero;
        }
    }

    private void OnDestroy() {
        Destroyed?.Invoke(this);
    }

}
