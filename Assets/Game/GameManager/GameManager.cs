using UnityEngine;

public class GameManager : MonoBehaviour {

    private Player         _player;
    private UpgradeManager _upgradeManager;
    private SalesManager   _salesManager;

    private UI_TextFeedback UI_textFeedback;
    private UI_StackingBar  UI_stackingBar;

    private void Awake() {
        
        _player         = FindAnyObjectByType<Player>();
        _upgradeManager = FindAnyObjectByType<UpgradeManager>();
        _salesManager   = FindAnyObjectByType<SalesManager>();

        UI_textFeedback = FindAnyObjectByType<UI_TextFeedback>();
        UI_stackingBar  = FindAnyObjectByType<UI_StackingBar>();

        // Subscribe events to the text feedback
        _upgradeManager.NotEnougthMoney += UI_textFeedback.OnShowMsg;
        _salesManager.OnSold            += UI_textFeedback.OnShowMsg;

        _upgradeManager.OnUpgrade       += UI_textFeedback.OnShowMsg;
        _upgradeManager.OnUpgrade       += UI_stackingBar.OnStackUpgrade;

        // Use this wrapper because the EnemyStacking script is
        // a private field of the Player script.
        _player.SetMaxCapacityEvent(UI_textFeedback.OnShowMsg);

    }

    void Update() {
        
        _player._Update();
        _upgradeManager._Update();
        _salesManager._Update();

    }

    private void OnDisable() {
        _upgradeManager.NotEnougthMoney -= UI_textFeedback.OnShowMsg;
        _salesManager.OnSold            -= UI_textFeedback.OnShowMsg;
        _upgradeManager.OnUpgrade       -= UI_textFeedback.OnShowMsg;
        _upgradeManager.OnUpgrade       -= UI_stackingBar.OnStackUpgrade;
    }

}
