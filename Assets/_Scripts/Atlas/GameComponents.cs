using Atlas.Core;
using Atlas.Core.Serialization;
using Atlas.Effects;
using Atlas.Player;
using Atlas.Presenters;
using Atlas.Systems;
using Atlas.Utility;
using Atlas.Views;
using UnityEngine;
using Zenject;

public class GameComponents : MonoInstaller
{
    public Config config;
    
    public override void InstallBindings()
    {
        Container.Bind<Config>().FromScriptableObject(config).AsSingle().NonLazy();

        Gamemaster gamemaster = FindAnyObjectByType<Gamemaster>();
        if (gamemaster == null)
        {
            Debug.LogError("Gamemaster is not found in the scene. Make sure it's carried over using DontDestroyOnLoad.");
            return;
        }

        Container.Bind<Gamemaster>().FromInstance(gamemaster).AsSingle();

        Container.Bind<ResourcesSystem>().AsSingle().NonLazy();
        Container.Bind<EffectsSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        
        Container.Bind<ProgressSystem>().AsSingle().NonLazy();
        
        Container.Bind<ShopPresenter>().AsSingle().NonLazy();
        Container.Bind<ShopSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ShopView>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<AtlasBattlePresenter>().AsSingle().NonLazy();
        Container.Bind<AtlasBattleSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<AtlasBattleView>().FromComponentInHierarchy().AsSingle().NonLazy();
        
        Container.Bind<AttributePresenter>().AsSingle().NonLazy();
        Container.Bind<InventoryPresenter>().AsSingle().NonLazy();
        Container.Bind<SpellPresenter>().AsSingle().NonLazy();

        // Container.Bind<RestPresenter>().AsSingle().NonLazy();
        // Container.Bind<RestSystem>().FromComponentInHierarchy().AsSingle().NonLazy();
        // Container.Bind<RestView>().FromComponentInHierarchy().AsSingle().NonLazy();
        
        Container.Bind<PlayerSystem>().FromComponentInHierarchy().AsSingle();

        Container.Bind<SaveLoadSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SerializationSystem>().FromComponentInHierarchy().AsSingle();

        Container.QueueForInject(FindFirstObjectByType<SerializationSystem>());
        Container.QueueForInject(FindFirstObjectByType<SaveLoadSystem>());
        Container.QueueForInject(FindFirstObjectByType<Gamemaster>());

        #region Factories
        Container.BindInterfacesTo<InventoryView>().AsSingle();
        Container.BindFactory<Object, InventoryCell, InventoryCell.Factory>().FromFactory<PrefabFactory<InventoryCell>>();

        Container.BindInterfacesTo<AttributeView>().AsSingle();
        Container.BindFactory<Object, AttributeRow, AttributeRow.Factory>().FromFactory<PrefabFactory<AttributeRow>>();
        Container.BindFactory<Object, ResistanceRow, ResistanceRow.Factory>().FromFactory<PrefabFactory<ResistanceRow>>();
        #endregion
    }
}
