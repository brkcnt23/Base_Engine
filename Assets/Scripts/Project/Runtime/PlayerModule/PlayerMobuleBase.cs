using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Runner {
    public abstract class PlayerMobuleBase : MonoBehaviour {
        protected PlayerMainModule _owner;

        [HideInInspector] public bool IsActive = false;
        [HideInInspector] public bool IsInitailized = false;
        
        public bool _canProcess => IsInitailized && IsActive;

        public Action UpdateAction;
        public Action FixedUpdateAction;
        public Action LateUpdateAction;

        public virtual void Initialize(PlayerMainModule owner) {
            _owner = owner;
            UpdateAction += UnUpdate;
            FixedUpdateAction += UnFixedUpdate;
            LateUpdateAction += UnLateUpdate;
            
            _owner.OnGeneralStateChanged += OnPlayerGeneralStateChanged;
            _owner.OnMovementStateChanged += OnPlayerMovementStateChanged;
            IsInitailized = true;
        }

        protected virtual void OnPlayerGeneralStateChanged(PlayerGeneralState generalState) {
            
        }
        
        protected virtual void OnPlayerMovementStateChanged(PlayerMovementState movementState) {
            
        }
        
        protected virtual void UnUpdate() {
            if (!_canProcess) return;
        }

        protected virtual void UnFixedUpdate() {
            if (!_canProcess) return;
        }
        
        protected virtual void UnLateUpdate() {
            if (!_canProcess) return;
        }

        public virtual void Activate() {
            IsActive = true;
        }
        
        public virtual void Deactivate() {
            IsActive = false;
        }
        
        public virtual void Uninitialized() {
            IsActive = false;
            IsInitailized = false;
            UpdateAction -= UnUpdate;
            FixedUpdateAction -= UnFixedUpdate;
            LateUpdateAction -= UnLateUpdate;
        }
        
    }
}
