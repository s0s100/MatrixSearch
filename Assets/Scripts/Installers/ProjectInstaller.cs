using Zenject;
using UnityEngine;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private ConfigProvider configProvider;

    public override void InstallBindings()
    {
        Container.BindInstance(configProvider).AsSingle();
        Container.Bind<JSONReaderService>().AsSingle();
        Container.Bind<JSONMatrixFillService>().AsSingle();
    }
}