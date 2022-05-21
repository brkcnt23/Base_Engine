using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
namespace Base {
    public class SaveSystemWindow : OdinMenuEditorWindow {
        [MenuItem("Tools/SaveSystem System/SaveSystem Editor %F2")]
        private static void OpenWindow() {
            GetWindow<SaveSystemWindow>().Show();
        }
        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree();
            tree.Add("Save System Manager", new B_SaveSystemEditor(tree), EditorIcons.DayCalendar);
            tree.AddAllAssetsAtPath("Saves", "Assets/Resources/SaveAssets");
            return tree;
        }
    }
}