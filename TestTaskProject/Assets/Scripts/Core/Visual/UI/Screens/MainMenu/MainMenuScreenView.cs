using UnityEngine.UI;

public class MainMenuScreenView : ScreenView
{
    public Button PlayButton => _playButton;

    [UnityEngine.SerializeField]
    private Button _playButton;

    public override ScreenController Construct(EventManager eventManager)
    {
        return new MainMenuScreenController(this, eventManager);
    }
}