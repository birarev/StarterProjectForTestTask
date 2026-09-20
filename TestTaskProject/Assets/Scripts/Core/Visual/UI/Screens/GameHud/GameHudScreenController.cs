using UnityEngine;
using static EventsProvider;

public class GameHudScreenController : ScreenController
{
    private readonly GameHudScreenView _gameHudView;

    public GameHudScreenController(
        GameHudScreenView view,
        EventManager eventManager)
        : base(view, eventManager)
    {
        _gameHudView = view;
    }

    public override void Open()
    {
        base.Open();

        _gameHudView.BackButton.onClick.AddListener(OnBackClicked);
        _gameHudView.RestartButton.onClick.AddListener(OnRestartClicked);

        _gameHudView.Game.StateChanged += OnStateChanged;
        _gameHudView.Game.GameEnded += OnGameEnded;

        _gameHudView.Target.CorrectClicked += OnCorrectClicked;
        _gameHudView.Target.WrongClicked += OnWrongClicked;
        _gameHudView.Target.GreenMissed += OnGreenMissed;

        _gameHudView.Target.Initialize(_gameHudView.Game);

        _gameHudView.HideResult();
        _gameHudView.Game.StartGame();
    }

    public override void Dispose()
    {
        _gameHudView.BackButton.onClick.RemoveListener(OnBackClicked);
        _gameHudView.RestartButton.onClick.RemoveListener(OnRestartClicked);

        _gameHudView.Game.StateChanged -= OnStateChanged;
        _gameHudView.Game.GameEnded -= OnGameEnded;

        _gameHudView.Target.CorrectClicked -= OnCorrectClicked;
        _gameHudView.Target.WrongClicked -= OnWrongClicked;
        _gameHudView.Target.GreenMissed -= OnGreenMissed;
    }

    private void OnStateChanged(
        int score,
        int lives,
        float timeLeft,
        int combo)
    {
        _gameHudView.UpdateScore(score, 10);
        _gameHudView.UpdateLives(lives);
        _gameHudView.UpdateTime(timeLeft);
        _gameHudView.UpdateCombo(combo);
    }

    private void OnGameEnded(bool isWin)
    {
        _gameHudView.ShowResult(isWin);
    }

    private void OnRestartClicked()
    {
        _gameHudView.HideResult();
        _gameHudView.Game.StartGame();
        _gameHudView.Target.Initialize(_gameHudView.Game);
    }

    private void OnBackClicked()
    {
        _eventManager.Publish(
            new LoadSceneEvent("FirstScene"));
    }

    private void OnCorrectClicked()
    {
        _gameHudView.ShowFeedback("+1", Color.green);
    }

    private void OnWrongClicked()
    {
        _gameHudView.ShowFeedback("WRONG!", Color.red);
    }

    private void OnGreenMissed()
    {
        _gameHudView.ShowFeedback("MISS!", Color.red);
    }
}