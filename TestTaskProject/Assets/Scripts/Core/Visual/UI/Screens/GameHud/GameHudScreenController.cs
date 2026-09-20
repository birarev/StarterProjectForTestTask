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
    }

    private void OnRestartClicked()
    {
        _gameHudView.HideResult();
        _gameHudView.Game.StartGame();
    }

    private void OnStateChanged(
        int score,
        int lives,
        float timeLeft)
    {
        _gameHudView.UpdateScore(score, 10);
        _gameHudView.UpdateLives(lives);
        _gameHudView.UpdateTime(timeLeft);
    }

    private void OnGameEnded(bool isWin)
    {
        UnityEngine.Debug.Log("CONTROLLER RECEIVED GAME END");

        _gameHudView.ShowResult(isWin);
    }

    private void OnBackClicked()
    {
        _eventManager.Publish(
            new LoadSceneEvent("FirstScene")
        );
    }
}