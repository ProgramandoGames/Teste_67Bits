using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 
/// Implements stacking mechanics for dead enemies.
/// This script uses raycasting to detect enemy bodies.
/// 
/// A new GameObject is used for stacked enemies. 
/// Since enemies are dead, updating all the logic within
/// the Enemy script is no longer necessary.
/// 
/// </summary>
public class EnemyStacking : MonoBehaviour {

    public Action<string> OnMaxCapacity;

    public Transform stackedEnemyPrefab;
    public Vector3   stackStartPosition;
    public float     stackOffset = 0.5f;
    public int       stackLimit = 5;
    public float     adjustPosition  = 0.8f;
    public float     inertiaIntensity = 1f;

    public LayerMask enemyLayer;

    private Raycasting _raycasting;

    private List<Transform> _stack = new List<Transform>();
    private List<Vector3>  _originalPosition = new List<Vector3>();

    public int stackCount { get { return _stack.Count; } }

    private void Awake() {

        _raycasting = GetComponent<Raycasting>();

    }

    public void _Update(Vector3 _input) {

        InertiaEffect(_input);

        RaycastHit hit;
        if(_raycasting.Detects(out hit, enemyLayer)) {

            // Only stack the enemy if it's done
            if(hit.transform.GetComponent<Enemy>().done) {
                PushEnemy(hit.transform.gameObject);
            }

        }

        // Just for testing purposes
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.V)) {
            PushEnemy(null);
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            PopEnemy();
        }
#endif

    }

    public void UpgradeStackLimit() {
        stackLimit++;
    }

    private void PushEnemy(GameObject enemy) {
        if(_stack.Count < stackLimit) {
            Transform obj = Instantiate(stackedEnemyPrefab);
            Vector3 startPosition = transform.position - transform.forward * adjustPosition + transform.up * adjustPosition;
            Vector3 offset = Vector3.up * stackOffset * _stack.Count;
            obj.position = startPosition + offset;
            _originalPosition.Add(offset);
            _stack.Add(obj);
            Destroy(enemy);
        } else {
            OnMaxCapacity?.Invoke("Can’t carry any more bodies!");
        }
    }

    public void PopEnemy() {
        if(_stack.Count == 0) return;
        StartCoroutine(PopAnimation(_stack[0]));
        _stack.RemoveAt(0);
        // Update the position of the remaining enemies in the stack
        for(int i = 0; i < _stack.Count; i++) {
            _stack[i].localPosition = stackStartPosition + Vector3.up * stackOffset * i;
        }
    }

    private void InertiaEffect(Vector3 _input) {

        Vector3 pivotPosition = transform.position - transform.forward * adjustPosition + transform.up * adjustPosition;

        for(int i = 0; i < _stack.Count; i++) {

            var factor = Mathf.Lerp(inertiaIntensity, 1f, (float)i / _stack.Count);

            Vector3 target;
            if(i == 0) {
                target = pivotPosition + _originalPosition[i];
            } else {
                target = _stack[i-1].position + Vector3.up * stackOffset;
            }
            
            _stack[i].position = Vector3.Lerp(_stack[i].position, target, factor * Time.deltaTime);

            float diff = (_stack[i].position - target).sqrMagnitude;

            _stack[i].forward = transform.forward;
            _stack[i].localRotation *= Quaternion.Euler(50 - 40 * diff, 0, 90);
            
        }
    }

    // Performs a pop animation for the enemy when it is removed from the stack
    private IEnumerator PopAnimation(Transform enemy) {

        Vector3 startPosition = enemy.position;
        Vector3 popPosition   = enemy.position + transform.forward * 2;
        for(float t = 0; t <= 1f; t += Time.deltaTime * 5) {
            enemy.position = Vector3.Lerp(startPosition, popPosition, t);
            yield return null;
        }
        Destroy(enemy.gameObject);

    }

}
