using DG.Tweening;
using UnityEngine;

namespace Project.Runner {
    public class PlayerVisualControlModule : PlayerMobuleBase {

        [HideInInspector] public GameObject VisualHolder;
        [HideInInspector] public GameObject Visual;

        #region Module Data

        public override void Initialize(PlayerMainModule mainModule) {
            base.Initialize(mainModule);
            VisualHolder = transform.GetChild(0).gameObject;
            Visual = VisualHolder.transform.GetChild(0).gameObject;
        }
                
        public override void Activate() {
            base.Activate();
            //Example Functions on how to use visuals with DoTween
            
            // VisualHolder.transform.DOScale(VisualHolder.transform.localScale * 1.5f, .75f)
            //     .SetLoops(-1, LoopType.Yoyo);
            // Visual.transform.TryGetComponent(out Renderer rend);
            // rend.material.color = Color.white;
            // rend.material.DOColor(Color.yellow, .75f).SetLoops(-1, LoopType.Yoyo);
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