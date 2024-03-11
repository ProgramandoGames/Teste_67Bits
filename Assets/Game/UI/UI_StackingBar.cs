using UnityEngine;

/// <summary>
/// 
/// This class implements a classic bar to display the 
/// current stack capacity.
/// 
/// It can be disabled without affecting the game logic or 
/// any other script.
/// 
/// </summary>

public class UI_StackingBar : MonoBehaviour {

    public RectTransform barPrefab;

    public float spacing = 50;

    private Vector2 _startPosition;
    private int _currentStack = 0;
    
    private void Awake() {

        _startPosition = transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;

        for(int i = 1; i < 3; i++) {
            RectTransform bar = Instantiate(barPrefab, transform);
            bar.anchoredPosition = _startPosition + Vector2.up * spacing * i;
        }

        _currentStack = 3;

    }

    public void OnStackUpgrade(string text) {
        RectTransform bar = Instantiate(barPrefab, transform);
        bar.anchoredPosition = _startPosition + Vector2.up * spacing * _currentStack++;
    }

}
