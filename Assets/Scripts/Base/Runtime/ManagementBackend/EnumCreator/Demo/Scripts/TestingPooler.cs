using UnityEngine;
namespace Base {
    public class TestingPooler : B_PoolerBase {
        private void Awake() { }
        private void Start() {
            InitiatePooller();
            for (var i = 0; i < 10; i++)
                SpawnObjFromPool(DemoPoolerTester.Dilaver, Vector3.zero);
        }
    }
}