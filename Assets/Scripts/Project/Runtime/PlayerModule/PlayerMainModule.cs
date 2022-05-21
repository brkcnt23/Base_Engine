using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base;
using Base.SoundManagement;
using Base.UI;
using Lean.Touch;
using UnityEngine;

namespace Project.Runner {
    public enum PlayerGeneralState {
        Deactivated,
        Activated,
        Paused,
        Frozen
    }
    public enum PlayerMovementState {
        Idle,
        OnSpline,
        OnGround,
        OnAir
    }
    public enum PlayerStartMovementType {
        Spline,
        Normal,
        LeanDrag,
        None
    }
    [RequireComponent(typeof(PlayerCollisionModule), typeof(PlayerMovementModule), typeof(PlayerSplineFollowerModule))]
    [RequireComponent(typeof(PlayerUIModule), typeof(PlayerVisualControlModule))]
    public class PlayerMainModule : MonoBehaviour {
        #region Variables
        /// <summary>
        /// Select a player movement type
        /// </summary>
        [Tooltip("Select a player movement type")]
        [SerializeField] PlayerStartMovementType startMovementType = PlayerStartMovementType.Spline;
        /// <summary>
        /// General state of the player
        /// </summary>
        public PlayerGeneralState GeneralState { get; private set; }
        /// <summary>
        /// Movement state of the player
        /// </summary>
        public PlayerMovementState MovementState { get; private set; }
        /// <summary>
        /// Called when the player changes its general state
        /// </summary>
        public Action<PlayerGeneralState> OnGeneralStateChanged;
        /// <summary>
        /// Called when the player changes its movement state
        /// </summary>
        public Action<PlayerMovementState> OnMovementStateChanged;
        /// <summary>
        /// Collision Module, detects collisions with other objects and relays them to the Main Module
        /// </summary>
        [Tooltip("Collision Module, detects collisions with other objects and relays them to the Main Module")]
        [HideInInspector]
        public PlayerCollisionModule collisionModule;
        /// <summary>
        /// Movement Module, handles standard or custom movement
        /// </summary>
        [Tooltip("Movement Module, handles standard or custom movement")]
        [HideInInspector]
        public PlayerMovementModule movementModule;
        /// <summary>
        /// Spline follower module, handles spline movement
        /// </summary>
        [Tooltip("Spline follower module, handles spline movement")]
        [HideInInspector]
        public PlayerSplineFollowerModule splineFollowerModule;
        /// <summary>
        /// UI Module, handles UI related stuff, be it the Main UI or World Canvas UI 
        /// </summary>
        [Tooltip("UI Module, handles UI related stuff, be it the Main UI or World Canvas UI")]
        [HideInInspector]
        public PlayerUIModule uiModule;
        /// <summary>
        /// Controls the player's visual appearance, such as the player's sprite, the player's color, etc.
        /// </summary>
        [Tooltip("Controls the player's visual appearance, such as the player's sprite, the player's color, etc.")]
        [HideInInspector]
        public PlayerVisualControlModule visualControlModule;
        /// <summary>
        /// Lean Drag Module, handles the player's drag movement, such as when the player is not on a spline
        /// </summary>
        [Tooltip("Lean Drag Module, handles the player's drag movement, such as when the player is not on a spline")]
        [HideInInspector]
        public LeanDragTranslate leanDragTranslateModule;
        
        List<PlayerMobuleBase> modules = new List<PlayerMobuleBase>();

        #endregion
        
        #region Unity Functions

        private void Awake() {
            SetupPlayer();
        }

        private void Start() {
            
        }

        private void Update() {
            for (int i = 0; i < modules.Count; i++) {
                if(modules[i]._canProcess) {
                    modules[i].UpdateAction?.Invoke();
                }
            }
        }

        private void FixedUpdate() {
            for (int i = 0; i < modules.Count; i++) {
                if(modules[i]._canProcess) {
                    modules[i].FixedUpdateAction?.Invoke();
                }
            }
        }

        private void LateUpdate() {
            for (int i = 0; i < modules.Count; i++) {
                if(modules[i]._canProcess) {
                    modules[i].LateUpdateAction?.Invoke();
                }
            }
        }

        #endregion

        #region Class Functions
        /// <summary>
        /// Called when the player is created, sets up the player's modules
        /// sets the camera to the player's transform,
        /// adds functionality to the player's components and
        /// adds functions to buttons
        /// </summary>
        private void SetupPlayer() {
            InitializePlayer();
            ActiveVirtualCameras.VirCam1.CameraSetAll(transform);
            B_CentralEventSystem.BTN_OnStartPressed.AddFunction(Activate);
        }
        /// <summary>
        /// Called first in the SetupPlayer function, sets up the player's modules,
        /// adds them to the modules list, and sets up the player's components
        /// </summary>
        private void InitializePlayer() {
            
            GeneralState = PlayerGeneralState.Deactivated;
            MovementState = PlayerMovementState.Idle;
            
            collisionModule = GetComponent<PlayerCollisionModule>();
            modules.Add(collisionModule);
            movementModule = GetComponent<PlayerMovementModule>();
            modules.Add(movementModule);
            splineFollowerModule = GetComponent<PlayerSplineFollowerModule>();
            modules.Add(splineFollowerModule);
            uiModule = GetComponent<PlayerUIModule>();
            modules.Add(uiModule);
            visualControlModule = GetComponent<PlayerVisualControlModule>();
            modules.Add(visualControlModule);
            
            leanDragTranslateModule = GetComponent<LeanDragTranslate>();
            
            modules.ForEach(module => module.Initialize(this));
            
            GeneralState = PlayerGeneralState.Activated;
            
            splineFollowerModule.OnSplineEndReached += Deactivate;
        }
        
        /// <summary>
        /// Activates the player, sets the player's state to active,
        /// also activates the player's modules
        /// Works with a delay, so that movement doesn't start before the player is fully activated
        /// </summary>
        private async void Activate() {
            await Task.Delay(100);
            ModuleActivation();
            Enum_MainMusicCollection.B_AutoClick.PlaySound().SetDelay(2f).SetLoop(10, .2f);
        }

        void ModuleActivation() {
            foreach (PlayerMobuleBase module in modules) {
                if(module == splineFollowerModule) {
                    if (startMovementType == PlayerStartMovementType.Spline) {
                        module.Activate();
                    }
                    continue;
                }
                if(module == movementModule) {
                    if (startMovementType == PlayerStartMovementType.Normal) {
                        module.Activate();
                    }
                    continue;
                }
                module.Activate();
            }

            if (startMovementType == PlayerStartMovementType.LeanDrag) {
                leanDragTranslateModule.CanMove = true;
            }
        }

        private void Deactivate(double d) {
            modules.ForEach(t => t.Deactivate());
            ChangePlayerState(PlayerGeneralState.Deactivated);
            ChangePlayerMovementState(PlayerMovementState.Idle);
            
            if (startMovementType == PlayerStartMovementType.LeanDrag) {
                leanDragTranslateModule.CanMove = false;
            }
        }
        
        private void ChangePlayerState(PlayerGeneralState newState) {
            GeneralState = newState;
            OnGeneralStateChanged?.Invoke(newState);
        }
        
        private void ChangePlayerMovementState(PlayerMovementState newState) {
            MovementState = newState;
            OnMovementStateChanged?.Invoke(newState);
        }

        #endregion

        #region Module Interaction

        public void OnModuleTriggerEnter(Collider other) {
            
        }
        
        public void OnModuleTriggerExit(Collider other) {
            
        }

        public void OnModuleCollisionEnter(Collision other) {
            
        }
        
        public void OnModuleCollisionExit(Collision other) {
            
        }
        
        #endregion


    }
}