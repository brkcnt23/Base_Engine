using UnityEngine;
namespace Project.Runner {
    public class PlayerMovementModule : PlayerMobuleBase {
        
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
        
    }
}