using UnityEngine;
using Zenject;

public class CoreProjectInstaller : MonoInstaller
{
    [SerializeField] private UIController _uiController;

    public override void InstallBindings()
    {
        Container.Bind<EventManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneTransitionService>().AsSingle();
        Container.Bind<UIController>().FromInstance(_uiController).AsSingle();
    }
}
