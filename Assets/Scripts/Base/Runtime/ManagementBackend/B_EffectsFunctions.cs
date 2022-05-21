using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Base {
    public class B_EffectsFunctions : B_PoolerBase {
        #region Variables

        public static B_EffectsFunctions instance;
        private List<B_PooledParticle> _usedParticles;
        #endregion

        #region Editor Functions

        #if UNITY_EDITOR

        [Button("Set Pooled Particles")]
        private void Load() {
            var ObjectsInResources = Resources.LoadAll<GameObject>("Particles");
            var particleNames = new List<string>();

            PoolsList = new List<ObjectsToPool>();
            PoolsDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (var g in ObjectsInResources) {
                particleNames.Add(g.name);
                AddPoolInEditor(g, g.name, 10);
            }
            B_EnumCreator.CreateEnum("Particles", particleNames.ToArray());
        }

#endif

        #endregion

        #region Main Functions

        public Task VFXManagerStrapping() {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
            InitiatePooller(transform);
            _usedParticles = new List<B_PooledParticle>();
            B_CentralEventSystem.OnBeforeLevelLoaded.AddFunction(ResetParticles, true);
            return Task.CompletedTask;
        }

        public B_PooledParticle SpawnAParticle(object enumToPull, Vector3 positionToSpawnIn, [Optional] Quaternion rotationToSpawnIn) {
            var obj = SpawnObjFromPool(enumToPull.ToString(), positionToSpawnIn, rotationToSpawnIn);
            _usedParticles.Add(obj.GetComponent<B_PooledParticle>());
            return obj.GetComponent<B_PooledParticle>();
        }

        private void OnDisable() {
            instance = null;
        }

        void ResetParticles() {
            if(_usedParticles == null || _usedParticles.Count <= 0) return;
            _usedParticles.ForEach(t => t.transform.SetParent(transform, false));
            _usedParticles.ForEach(t => t.ResetParticle());
            _usedParticles.Clear();
        }

        #endregion
    }

}