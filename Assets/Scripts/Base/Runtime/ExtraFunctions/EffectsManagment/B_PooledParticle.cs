using System.Threading.Tasks;
using UnityEngine;
namespace Base {
    public class B_PooledParticle : MonoBehaviour, B_OPS_IPooledObject {

        // private float _loopDelay;
        private ParticleSystem _particleSystem;

        public void OnFirstSpawn() {
            SetupParticle();
        }
        public void OnObjectSpawn() { }
        public void OnRespawn() { }

        private void SetupParticle() {
            _particleSystem = GetComponent<ParticleSystem>();
            if (_particleSystem.isPlaying)
                _particleSystem.Stop();

            // _loopDelay = _particleSystem.main.duration;
        }

        public B_PooledParticle PlayParticle() {
            ResetParticle();
            _particleSystem.Play();
            return this;
        }
        
        //Shouldn't be used

        // public async Task<B_PooledParticle> SetLoop(int loopAmount) {
        //     for (var i = 0; i < loopAmount; i++) {
        //         _particleSystem.Stop();
        //         ResetParticle();
        //         _particleSystem.Play();
        //         await Task.Delay((int)_loopDelay * 1000);
        //     }
        //     return this;
        // }

        public void ResetParticle() {
            _particleSystem.Clear();
        }
    }
}