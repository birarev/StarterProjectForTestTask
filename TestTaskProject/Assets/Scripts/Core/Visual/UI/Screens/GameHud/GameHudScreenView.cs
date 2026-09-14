using UnityEngine;
using UnityEngine.UI;

public class GameHudScreenView : ScreenView
{
    public Button BackButton => _backButton;

    [SerializeField] private Button _backButton;

    public override ScreenController Construct(EventManager eventManager)
    {
        return new GameHudScreenController(this, eventManager);
    }
}