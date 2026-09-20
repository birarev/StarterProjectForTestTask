using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TargetRushTarget : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _movementArea;
    [SerializeField] private Image _targetImage;
    [SerializeField] private Sprite _greenTargetSprite;
    [SerializeField] private Sprite _redTargetSprite;
    [SerializeField] private TMP_Text _targetText;

    [SerializeField] private float _startChangeInterval = 1.5f;
    [SerializeField] private float _minimumChangeInterval = 0.8f;
    [SerializeField] private float _dangerousChance = 0.35f;

    public event Action CorrectClicked;
    public event Action WrongClicked;
    public event Action GreenMissed;

    private TargetRushGame _game;
    private RectTransform _rectTransform;

    private bool _isDangerous;
    private bool _currentTargetWasClicked;
    private float _changeTimer;

    public void Initialize(TargetRushGame game)
    {
        _game = game;
        _rectTransform = GetComponent<RectTransform>();

        if (_button == null)
            _button = GetComponent<Button>();

        if (_targetImage == null)
            _targetImage = GetComponent<Image>();

        _button.onClick.RemoveListener(OnClicked);
        _button.onClick.AddListener(OnClicked);

        ShowNextTarget();
    }

    private void Update()
    {
        if (_game == null || _game.IsGameOver)
            return;

        _changeTimer -= Time.deltaTime;

        if (_changeTimer <= 0f)
        {
            // Missing a green target costs one life.
            if (!_isDangerous && !_currentTargetWasClicked)
            {
                _game.LoseLife();
                GreenMissed?.Invoke();

                if (_game.IsGameOver)
                    return;
            }

            ShowNextTarget();
        }
    }

    private void OnClicked()
    {
        if (_game == null || _game.IsGameOver)
            return;

        // Prevent multiple clicks on the same target.
        if (_currentTargetWasClicked)
            return;

        _currentTargetWasClicked = true;

        if (_isDangerous)
        {
            _game.LoseLife();
            WrongClicked?.Invoke();
        }
        else
        {
            _game.AddScore();
            CorrectClicked?.Invoke();
        }

        if (_game.IsGameOver)
            return;

        ShowNextTarget();
    }

    private void ShowNextTarget()
    {
        MoveToRandomPosition();

        _isDangerous = Random.value < _dangerousChance;
        _currentTargetWasClicked = false;
        _changeTimer = GetCurrentChangeInterval();

        UpdateAppearance();
    }

    private void MoveToRandomPosition()
    {
        if (_movementArea == null || _rectTransform == null)
            return;

        Rect area = _movementArea.rect;

        float halfWidth = _rectTransform.rect.width * 0.5f;
        float halfHeight = _rectTransform.rect.height * 0.5f;

        float x = Random.Range(
            area.xMin + halfWidth,
            area.xMax - halfWidth);

        float y = Random.Range(
            area.yMin + halfHeight,
            area.yMax - halfHeight);

        _rectTransform.anchoredPosition = new Vector2(x, y);
    }

    private float GetCurrentChangeInterval()
    {
        if (_game == null)
            return _startChangeInterval;

        float progress = Mathf.Clamp01(_game.Score / 10f);

        return Mathf.Lerp(
            _startChangeInterval,
            _minimumChangeInterval,
            progress);
    }

    private void UpdateAppearance()
    {
        if (_targetImage != null)
        {
            _targetImage.sprite = _isDangerous
                ? _redTargetSprite
                : _greenTargetSprite;

            _targetImage.color = Color.white;
        }

        if (_targetText != null)
        {
            _targetText.text = string.Empty;
        }
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(OnClicked);
    }
}