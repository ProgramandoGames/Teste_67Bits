using System;
using TMPro;
using UnityEngine;

public class SalesManager : MonoBehaviour {

    public Action<string> OnSold;

    public Transform       player;
    public TextMeshProUGUI moneyUI;

    public float sellDelay = 1f;
    public float sellValue = 10f;

    public float money { get { return _money; } }

    private float _money = 0;

    private EnemyStacking _enemyStacking;
    private float         _sellTime = 0;

    void Start() {
        _enemyStacking = player.GetComponent<EnemyStacking>();
    }

    public void _Update() {
        
        if(_enemyStacking.stackCount == 0) return;

        if(IsInsideXZArea(player.position, transform.position, 3f)) {
            if(IsForwardInAngleRange(player.forward, 45)) {
                if(Time.time > _sellTime) {
                    _enemyStacking.PopEnemy();
                    _sellTime = Time.time + sellDelay;
                    _money += sellValue;
                    moneyUI.text = "$ " + _money.ToString();
                    OnSold?.Invoke("+$ " + sellValue.ToString());
                }
            }
        }

    }

    public void UpdateMoney(float value) {
        _money += value;
        moneyUI.text = "$ " + _money.ToString();
    }

    private bool IsInsideXZArea(Vector3 position, Vector3 center, float size) {
        return Mathf.Abs(position.x - center.x) <= size / 2f &&
               Mathf.Abs(position.z - center.z) <= size / 2f;
    }

    public static bool IsForwardInAngleRange(Vector3 forward, float minAngle) {
        float angle = Vector3.Angle(forward, Vector3.right);
        return angle <= minAngle;
    }

}
