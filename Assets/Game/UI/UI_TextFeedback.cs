using TMPro;
using UnityEngine;

public class UI_TextFeedback : MonoBehaviour {

    public float duration = 2f;

    private RectTransform   _panel;
    private TextMeshProUGUI _text;
    private Vector2 _startPosition;

    private float _hideTime;
    private Vector2 _UpPosition;

    private void Awake() {
        _panel = GetComponent<RectTransform>();
        _text = _panel.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Start() {
        _startPosition = _panel.anchoredPosition;
        _UpPosition    = _startPosition + Vector2.up * 50;

        _panel.anchoredPosition = Vector2.one * 99999;
    }

    void Update() {

        if (Time.time > _hideTime) {
            _panel.anchoredPosition = Vector2.one * 99999;
        } else {
            var t = (_hideTime - Time.time) / duration;
            TextAnimation(t);
        }

    }

    public void OnShowMsg(string text) {
        _panel.anchoredPosition = _startPosition;
        _text.text           = text;
        _hideTime            = Time.time + duration;
    }

    private void TextAnimation(float t) {
        _panel.anchoredPosition = Vector2.Lerp(_startPosition, _UpPosition, 1-t);
    }


}
