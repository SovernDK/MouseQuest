using Atlas.AI.Grid;
using Atlas.Core;
using Atlas.Core.Serialization;
using Atlas.DB;
using Atlas.Effects;
using Atlas.Map;
using Atlas.Player;
using Atlas.Pooling;
using Atlas.Utility;
using Atlas.Views;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class ComponentsRoot : MonoInstaller
{
    public Config config;
    
    public override void InstallBindings()
    {
        #region Resources
        Container.Bind<Config>().FromScriptableObject(config).AsSingle().NonLazy();
        Container.Bind<Volume>().FromComponentInHierarchy().AsSingle().NonLazy();
        #endregion

        #region Systems
        Container.Bind<PoolSystem>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<ResourcesSystem>().AsSingle().NonLazy();
        Container.Bind<ViewAnimation>().AsSingle().NonLazy();

        Container.Bind<BattlePresenter>().AsSingle().NonLazy();
        Container.Bind<InventoryPresenter>().AsSingle().NonLazy();
        Container.Bind<AttributePresenter>().AsSingle().NonLazy();
        Container.Bind<DevPresenter>().AsSingle().NonLazy();
        Container.Bind<DialoguePresenter>().AsSingle().NonLazy();
        Container.Bind<SpellPresenter>().AsSingle().NonLazy();
        Container.Bind<CookingPresenter>().AsSingle().NonLazy();
        Container.Bind<ShopPresenter>().AsSingle().NonLazy();
        Container.Bind<TransitionsPresenter>().AsSingle().NonLazy();

        Container.Bind<CameraSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<DialogueSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<BattleSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ActionSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GridSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CookingSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ShopSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<TransitionsSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<EffectsSystem>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<MapSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SaveLoadSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Gamemaster>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SerializationSystem>().FromComponentInHierarchy().AsSingle();

        Container.QueueForInject(FindFirstObjectByType<SerializationSystem>());
        Container.QueueForInject(FindFirstObjectByType<SaveLoadSystem>());
        Container.QueueForInject(FindFirstObjectByType<Gamemaster>());
        #endregion

        #region Factories
        Container.BindInterfacesTo<InventoryView>().AsSingle();
        Container.BindFactory<Object, InventoryCell, InventoryCell.Factory>().FromFactory<PrefabFactory<InventoryCell>>();

        Container.BindInterfacesTo<AttributeView>().AsSingle();
        Container.BindFactory<Object, AttributeRow, AttributeRow.Factory>().FromFactory<PrefabFactory<AttributeRow>>();
        #endregion
    }
}