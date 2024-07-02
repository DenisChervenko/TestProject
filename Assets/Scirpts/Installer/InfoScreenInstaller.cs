using UnityEngine;
using Zenject;
public class InfoScreenInstaller : MonoInstaller
{
    [SerializeField] private InfoScreen _infoScreen;
    public override void InstallBindings()
    {
        Container.Bind<InfoScreen>().FromInstance(_infoScreen).AsSingle().NonLazy();
    }
}
