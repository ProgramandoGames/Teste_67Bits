using System;
using UnityEngine;

/// <summary>
/// 
/// This script implements the player's punch mechanic using raycasting.
/// 
/// </summary>

public class Punch : MonoBehaviour {

    public Action OnPunch;

    public float punchForce = 10f;
    public float rayLength  = 1f;
    public float delay      = 0.5f;

    public LayerMask targetLayer;

    private float _punchTime = 0;

    public void _Update() {
        if(Time.time > _punchTime) {
            RaycastDetection();
        }
    }

    private void RaycastDetection() {

       RaycastHit hit;
       bool hitsTarget = Physics.Raycast(transform.position, 
                                         transform.forward, 
                                         out hit, 
                                         rayLength,
                                         targetLayer);
       if(hitsTarget) {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null) {
                if(!enemy.done) {
                    enemy.Hit(transform.forward, punchForce);
                    OnPunch?.Invoke();
                    TimeController.obj.SlowDownTime(0.1f, 0.0f);
                    _punchTime = Time.time + delay;
                }
            }
       }

    }

    

}
