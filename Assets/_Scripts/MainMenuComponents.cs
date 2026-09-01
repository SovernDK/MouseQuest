using Atlas.Core;
using Atlas.DB;
using Atlas.Effects;
using Zenject;

public class MainMenuComponents : MonoInstaller
{
    public override void InstallBindings()
    {
        // // Container.Bind<StagingSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        // Container.Bind<ResourcesSystem>().AsSingle().NonLazy();
        // Container.Bind<EffectsSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        // Container.Bind<ConfigSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        // Container.Bind<MapSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
