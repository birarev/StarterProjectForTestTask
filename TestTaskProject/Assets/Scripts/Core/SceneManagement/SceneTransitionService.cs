using System;
using Zenject;
using static EventsProvider;

public class SceneTransitionService : IInitializable, IDisposable
{
    private readonly ZenjectSceneLoader _sceneLoader;
    private readonly EventManager _eventManager;

    public SceneTransitionService(
        ZenjectSceneLoader sceneLoader,
        EventManager eventManager)
    {
        _sceneLoader = sceneLoader;
        _eventManager = eventManager;
    }

    public void Initialize()
    {
        _eventManager.Subscribe<LoadSceneEvent>(OnLoadScene);
    }

    public void Dispose()
    {
        _eventManager.Unsubscribe<LoadSceneEvent>(OnLoadScene);
    }

    private void OnLoadScene(LoadSceneEvent loadSceneEvent)
    {
        if (string.IsNullOrEmpty(loadSceneEvent.SceneName))
            return;

        _sceneLoader.LoadScene(loadSceneEvent.SceneName);
    }
}