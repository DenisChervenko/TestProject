using Zenject;
using UnityEngine;

public class EventManagerInstaller : MonoInstaller
{
    [SerializeField] private EventManager _eventManager;
    public override void InstallBindings()
    {   
        Container.Bind<EventManager>().FromInstance(_eventManager).AsSingle().NonLazy();
    }
}