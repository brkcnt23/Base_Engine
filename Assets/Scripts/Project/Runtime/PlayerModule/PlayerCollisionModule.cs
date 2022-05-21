using System;
using UnityEngine;

namespace Project.Runner {
    public class PlayerCollisionModule : PlayerMobuleBase {
        
        #region Module Data

        public override void Initialize(PlayerMainModule mainModule) {
            base.Initialize(mainModule);
        }
        
        public override void Activate() {
            base.Activate();
        }

        public override void Deactivate() {
            base.Deactivate();
        }

        protected override void OnPlayerGeneralStateChanged(PlayerGeneralState generalState) {
            base.OnPlayerGeneralStateChanged(generalState);
        }

        protected override void OnPlayerMovementStateChanged(PlayerMovementState movementState) {
            base.OnPlayerMovementStateChanged(movementState);
        }

        protected override void UnUpdate() {
            base.UnUpdate();
        }

        protected override void UnFixedUpdate() {
            base.UnFixedUpdate();
        }

        protected override void UnLateUpdate() {
            base.UnLateUpdate();
        }

        public override void Uninitialized() {
            base.Uninitialized();
        }

        
        #endregion

        #region Collision Module Events 
        
        void OnEnterTriggerDecide(Collider other) {
            if(!_canProcess) return;
            _owner.OnModuleTriggerEnter(other);
        }
        
        void OnExitTriggerDecide(Collider other) {
            if(!_canProcess) return;
            _owner.OnModuleTriggerExit(other);
        }
        
        void OnEnterCollisionDecide(Collision other) {
            if(!_canProcess) return;
            _owner.OnModuleCollisionEnter(other);
        }

        void OnExitCollisionDecide(Collision other) {
            if(!_canProcess) return;
            _owner.OnModuleCollisionExit(other);
        }

        #endregion

        #region Unity Functions

        private void OnTriggerEnter(Collider other) {
            OnEnterTriggerDecide(other);
        }

        private void OnTriggerExit(Collider other) {
            OnExitTriggerDecide(other);
        }

        private void OnCollisionEnter(Collision collision) {
            OnEnterCollisionDecide(collision);
        }

        private void OnCollisionExit(Collision other) {
            OnExitCollisionDecide(other);
        }

        #endregion
        
    }
}