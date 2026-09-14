using static EventsProvider;

public class MainMenuScreenController : ScreenController
{
    private readonly MainMenuScreenView _mainMenuView;

    public MainMenuScreenController(
        MainMenuScreenView view,
        EventManager eventManager)
        : base(view, eventManager)
    {
        _mainMenuView = view;
    }

    public override void Open()
    {
        base.Open();

        _mainMenuView.PlayButton.onClick.AddListener(OnPlayClicked);
    }

    public override void Dispose()
    {
        _mainMenuView.PlayButton.onClick.RemoveListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        _eventManager.Publish(new LoadSceneEvent("SecondScene"));
    }
}