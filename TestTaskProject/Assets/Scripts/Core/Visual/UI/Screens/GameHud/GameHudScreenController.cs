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
    }

    public override void Dispose()
    {
        _gameHudView.BackButton.onClick.RemoveListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        _eventManager.Publish(new LoadSceneEvent("FirstScene"));
    }
}