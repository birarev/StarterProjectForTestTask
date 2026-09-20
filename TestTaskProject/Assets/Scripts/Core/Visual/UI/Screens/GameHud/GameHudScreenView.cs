using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHudScreenView : ScreenView
{
    public Button BackButton => _backButton;
    public Button RestartButton => _restartButton;
    public TargetRushTarget Target => _target;
    public TargetRushGame Game => _game;

    public TMP_Text ScoreText => _scoreText;
    public TMP_Text LivesText => _livesText;
    public TMP_Text TimeText => _timeText;

    [Header("Gameplay")]
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private Button _backButton;
    [SerializeField] private TargetRushTarget _target;
    [SerializeField] private TargetRushGame _game;

    [Header("HUD")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _comboText;
    [SerializeField] private TMP_Text _feedbackText;

    [Header("Result")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private Image _winImage;
    [SerializeField] private Image _loseImage;

    private Coroutine _feedbackCoroutine;

    public override ScreenController Construct(EventManager eventManager)
    {
        return new GameHudScreenController(this, eventManager);
    }

    public void UpdateScore(int score, int targetScore)
    {
        _scoreText.text = $"SCORE {score} / {targetScore}";
    }

    public void UpdateLives(int lives)
    {
        string hearts = new string('♥', Mathf.Max(0, lives));
        _livesText.text = $"LIVES   {hearts}";
    }

    public void UpdateTime(float time)
    {
        _timeText.text = $"TIME {Mathf.CeilToInt(time)}";
    }

    public void UpdateCombo(int combo)
    {
        if (_comboText == null)
            return;

        _comboText.text = combo >= 2
            ? $"COMBO ×{combo}"
            : string.Empty;
    }

    public void ShowFeedback(string message, Color color)
    {
        if (_feedbackText == null)
            return;

        if (_feedbackCoroutine != null)
            StopCoroutine(_feedbackCoroutine);

        _feedbackCoroutine = StartCoroutine(
            FeedbackRoutine(message, color)
        );
    }

    private IEnumerator FeedbackRoutine(string message, Color color)
    {
        _feedbackText.gameObject.SetActive(true);
        _feedbackText.text = message;

        RectTransform rectTransform = _feedbackText.rectTransform;

        Vector2 targetPosition =
            ((RectTransform)_target.transform).anchoredPosition;

        Vector2 startPosition =
            targetPosition + new Vector2(70f, 60f);

        Vector2 endPosition =
            targetPosition + new Vector2(70f, 130f);

        rectTransform.anchoredPosition = startPosition;

        Color startColor = color;
        startColor.a = 1f;
        _feedbackText.color = startColor;

        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress =
                Mathf.Clamp01(elapsed / duration);

            rectTransform.anchoredPosition =
                Vector2.Lerp(
                    startPosition,
                    endPosition,
                    progress
                );

            Color currentColor = startColor;
            currentColor.a = 1f - progress;

            _feedbackText.color = currentColor;

            yield return null;
        }

        _feedbackText.gameObject.SetActive(false);
        _feedbackCoroutine = null;
    }

    public void HideResult()
    {
        // Stop old feedback if Restart is pressed while
        // the feedback animation is still active.
        if (_feedbackCoroutine != null)
        {
            StopCoroutine(_feedbackCoroutine);
            _feedbackCoroutine = null;
        }

        if (_feedbackText != null)
            _feedbackText.gameObject.SetActive(false);

        // Show gameplay interface again.
        if (_gameplayUI != null)
            _gameplayUI.SetActive(true);

        // Hide result interface.
        if (_resultText != null)
            _resultText.gameObject.SetActive(false);

        if (_winImage != null)
            _winImage.gameObject.SetActive(false);

        if (_loseImage != null)
            _loseImage.gameObject.SetActive(false);

        if (_restartButton != null)
            _restartButton.gameObject.SetActive(false);

        if (_target != null)
            _target.gameObject.SetActive(true);
    }

    public void ShowResult(bool isWin)
    {
        // Stop feedback so it cannot remain visible
        // on the result screen.
        if (_feedbackCoroutine != null)
        {
            StopCoroutine(_feedbackCoroutine);
            _feedbackCoroutine = null;
        }

        if (_feedbackText != null)
            _feedbackText.gameObject.SetActive(false);

        // Hide the whole gameplay interface.
        if (_gameplayUI != null)
            _gameplayUI.SetActive(false);

        // Old TMP result is no longer used visually.
        if (_resultText != null)
            _resultText.gameObject.SetActive(false);

        // Show only the correct result image.
        if (_winImage != null)
            _winImage.gameObject.SetActive(isWin);

        if (_loseImage != null)
            _loseImage.gameObject.SetActive(!isWin);

        // Restart remains outside GameplayUI.
        if (_restartButton != null)
            _restartButton.gameObject.SetActive(true);
    }
}