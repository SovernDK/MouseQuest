#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Atlas.Utility;
using Atlas.OdinEditor;

namespace Atlas.DB
{
    public class DatabaseEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Atlas/Database Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<DatabaseEditorWindow>();
            window.titleContent = new GUIContent("Database Editor");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            Database.Instance.LoadAll();

            tree.AddAssetAtPath("Config", "Assets/Resources/Configs/Config.asset", typeof(Config));

            tree.AddAllAssetsAtPath("Characters", "Assets/Resources/Data/Characters", typeof(PlayerCharacterPrototype), true);
            tree.AddAllAssetsAtPath("Attributes", "Assets/Resources/Data/Attributes", typeof(AttributePrototype), true)
                .ForEach(AddDragHandles);
                
            tree.AddAllAssetsAtPath("Resistances", "Assets/Resources/Data/Resistances", typeof(ResistancePrototype), true)
                .ForEach(AddDragHandles);

            tree.AddAllAssetsAtPath("Items", "Assets/Resources/Data/Items", typeof(ItemPrototype), true)
                .ForEach(AddDragHandles);

            tree.AddAllAssetsAtPath("Spells", "Assets/Resources/Data/Spells", typeof(SpellPrototype), true)
                .ForEach(AddDragHandles);

            tree.AddAllAssetsAtPath("Enemies", "Assets/Resources/Data/Enemies", typeof(EnemyPrototype), true)
                .ForEach(AddDragHandles);

            tree.AddAllAssetsAtPath("States", "Assets/Resources/Data/BattlerStates", typeof(BattlerStatePrototype), true)
                .ForEach(AddDragHandles);

            tree.AddAllAssetsAtPath("Shops", "Assets/Resources/Data/Shop", typeof(ShopPrototype), true);

            tree.EnumerateTree().Where(x => x.Value as PlayerCharacterPrototype).ForEach(AddDragHandles);
            tree.EnumerateTree().Where(x => x.Value as ItemPrototype).ForEach(AddDragHandles);
            tree.EnumerateTree().Where(x => x.Value as SpellPrototype).ForEach(AddDragHandles);
            tree.EnumerateTree().Where(x => x.Value as EnemyPrototype).ForEach(AddDragHandles);

            tree.EnumerateTree().AddIcons<PlayerCharacterPrototype>(x => x.playerCharacter.icon);
            tree.EnumerateTree().AddIcons<ItemPrototype>(x => x.item.icon);
            tree.EnumerateTree().AddIcons<SpellPrototype>(x => x.spell.icon);
            tree.EnumerateTree().AddIcons<EnemyPrototype>(x => x.data.icon);
            tree.EnumerateTree().AddIcons<BattlerStatePrototype>(x => x.state.icon);

            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                {
                    ScriptableObjectCreator.ShowDialog<ItemPrototype>("Assets/Resources/Data/Items", obj =>
                    {
                        obj.item.name = obj.name;
                        obj.item.Create(obj.name);
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Spell")))
                {
                    ScriptableObjectCreator.ShowDialog<SpellPrototype>("Assets/Resources/Data/Spells", obj =>
                    {
                        obj.spell.name = obj.name;
                        obj.spell.Create(obj.name);
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Character")))
                {
                    ScriptableObjectCreator.ShowDialog<PlayerCharacterPrototype>("Assets/Resources/Data/Characters", obj =>
                    {
                        obj.playerCharacter.name = obj.name;
                        obj.playerCharacter.Create();
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Enemy")))
                {
                    ScriptableObjectCreator.ShowDialog<EnemyPrototype>("Assets/Resources/Data/Enemies", obj =>
                    {
                        obj.data.name = obj.name;
                        obj.data.Create();
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create State")))
                {
                    ScriptableObjectCreator.ShowDialog<BattlerStatePrototype>("Assets/Resources/Data/BattlerStates", obj =>
                    {
                        obj.state.name = obj.name;
                        obj.state.Create(obj.name);
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

    }
}
#endif
