using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<JSONReaderService>().AsSingle();
        Container.Bind<JSONMatrixFillService>().AsSingle();
    }
}