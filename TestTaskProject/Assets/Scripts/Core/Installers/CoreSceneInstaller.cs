using UnityEngine;
using Zenject;
using static EventsProvider;

public class CoreSceneInstaller : MonoInstaller
{
    [SerializeField] private string _initialScreenId;

    private EventManager _eventManager;

    [Inject]
    public void Construct(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    public override void InstallBindings()
    {
    }

    public override void Start()
    {
        if (!string.IsNullOrEmpty(_initialScreenId))
        {
            _eventManager.Publish(new OpenScreenEvent(_initialScreenId));
        }
    }
}