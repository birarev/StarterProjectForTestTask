using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHudScreenView : ScreenView
{
    public Button BackButton => _backButton;
    public TargetRushTarget Target => _target;
    public TargetRushGame Game => _game;

    public TMP_Text ScoreText => _scoreText;
    public TMP_Text LivesText => _livesText;
    public TMP_Text TimeText => _timeText;

    [SerializeField] private Button _backButton;
    [SerializeField] private TargetRushTarget _target;
    [SerializeField] private TargetRushGame _game;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _resultText;

    public override ScreenController Construct(EventManager eventManager)
    {
        return new GameHudScreenController(this, eventManager);
    }

    public void UpdateScore(int score, int targetScore)
    {
        _scoreText.text = $"SCORE: {score} / {targetScore}";
    }

    public void UpdateLives(int lives)
    {
        _livesText.text = $"LIVES: {lives}";
    }

    public void UpdateTime(float time)
    {
        _timeText.text = $"TIME: {Mathf.CeilToInt(time)}";
    }

    public void HideResult()
    {
        _resultText.gameObject.SetActive(false);
    }

    public void ShowResult(bool isWin)
    {
        _resultText.text = isWin
            ? "YOU WIN"
            : "YOU LOSE";

        _resultText.gameObject.SetActive(true);

        Debug.Log(
            $"RESULT SHOWN: {_resultText.text}, " +
            $"activeSelf={_resultText.gameObject.activeSelf}, " +
            $"activeInHierarchy={_resultText.gameObject.activeInHierarchy}"
        );
    }
}