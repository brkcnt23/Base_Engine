using Base;
using UnityEngine;

namespace Dirty.Docs {
    [CreateAssetMenu(fileName = "Example_SO", menuName = "Dirty/Docs/Example_SO")]
    public class ExampleScriptableObject : ScriptableObject {
        [SerializeField] ScriptableObjectSaveInfo saveInfo;
        
        public void SaveSO() {
            saveInfo.ModifyInfo(this,"", "");
            saveInfo.SaveScriptableObject();
        }
        
        public void LoadSO() {
            saveInfo.LoadScriptableObject();
        }
        
    }
}