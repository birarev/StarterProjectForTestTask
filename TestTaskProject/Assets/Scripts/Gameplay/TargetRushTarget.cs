using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetRushTarget : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _movementArea;
    [SerializeField] private Image _targetImage;
    [SerializeField] private TMP_Text _targetText;

    [SerializeField] private float _changeInterval = 1.5f;
    [SerializeField] private float _dangerousChance = 0.35f;

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
            // Green target disappeared before the player clicked it.
            if (!_isDangerous && !_currentTargetWasClicked)
            {
                _game.LoseLife();

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

        // Do not allow several clicks on the same target.
        if (_currentTargetWasClicked)
            return;

        _currentTargetWasClicked = true;

        if (_isDangerous)
        {
            // Red target = mistake.
            _game.LoseLife();
        }
        else
        {
            // Green target = correct reaction.
            _game.AddScore();
        }

        if (_game.IsGameOver)
            return;

        // After a click, immediately show the next target.
        ShowNextTarget();
    }

    private void ShowNextTarget()
    {
        MoveToRandomPosition();

        _isDangerous = Random.value < _dangerousChance;
        _currentTargetWasClicked = false;
        _changeTimer = _changeInterval;

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

    private void UpdateAppearance()
    {
        if (_targetImage != null)
        {
            _targetImage.color = _isDangerous
                ? Color.red
                : Color.green;
        }

        if (_targetText != null)
        {
            _targetText.text = _isDangerous
                ? "DON'T!"
                : "CLICK!";
        }
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(OnClicked);
    }
}