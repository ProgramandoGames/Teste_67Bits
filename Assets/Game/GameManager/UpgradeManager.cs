using System;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {

    public event Action<string> NotEnougthMoney;
    public event Action<string> OnUpgrade;

    public Transform       player;
    public TextMeshProUGUI levelUI;
    public RectTransform   upgradeButton;

    public float upgradePrice = 30f;

    public int currentLevel {
        get { return _currentLevel; }
    }

    private SalesManager  _salesManager;
    private EnemyStacking _enemyStacking;

    private Vector2 _buttonPosition;

    private int _currentLevel = 0;

    private void Awake() {
        _enemyStacking = player.GetComponent<EnemyStacking>();
        _salesManager  = FindAnyObjectByType<SalesManager>();
    }

    void Start() {
        _buttonPosition = upgradeButton.anchoredPosition;
    }

    public void _Update() {
        
        // Show the upgrade button if the player is onto the upgrade area
        if(IsInsideXZArea(player.position, transform.position, 3f)) {
            upgradeButton.anchoredPosition = _buttonPosition;
        } else {
            upgradeButton.anchoredPosition = Vector2.one * 99999;
        }

    }

    private bool IsInsideXZArea(Vector3 position, Vector3 center, float size) {
        return Mathf.Abs(position.x - center.x) <= size / 2f &&
               Mathf.Abs(position.z - center.z) <= size / 2f;
    }

    public void OnButtonPressed() {

        if(_salesManager.money < upgradePrice) {
            NotEnougthMoney?.Invoke("Not enough money!");
            return;
        }

        _enemyStacking.UpgradeStackLimit();
        _currentLevel++;
        levelUI.text = "Level " + _currentLevel.ToString();
        _salesManager.UpdateMoney(-upgradePrice);
        OnUpgrade?.Invoke("Level up!");

    }

}
