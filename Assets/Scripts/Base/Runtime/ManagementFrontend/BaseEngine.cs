using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.UI;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
#endif
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

namespace Base {
    
    [DefaultExecutionOrder(-100)]
    public class BaseEngine : MonoBehaviour {

        #region Properties
        
        [TabGroup("Master", "Bridge")]
        [HideLabel]
        public B_Bridge Bridge;

        [TabGroup("Master", "Game Aspect Control")]
        [SerializeField] private bool HasTutorial;
        [TabGroup("Master", "Editor Control")]
        [TabGroup("Master", "Editor Control")]
        [SerializeField] private B_EffectsFunctions effectsFunctions;
        [TabGroup("Master", "Editor Control")]
        [SerializeField] private bool ShowFps;
        /// <summary>
        /// Set to -1 to get unlimited frame rate
        /// </summary>
        [TabGroup("Master", "Game Aspect Control")]
        [SerializeField] private int TargetFrameRate = 60;
        
        #endregion
        
        #region Unity Functions

        private void Awake() {
            StartCoroutine(Temp());
        }

        private IEnumerator Temp() {
            
            //If you need to setup data tracking, ask for permissions here
#if UNITY_IOS
            //Asks for permission on iOS devices
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED || ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.RESTRICTED || ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED)
            {
                yield return new WaitForSeconds(1.2f);
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
#else
            yield return null;
#endif
            InitiateBootLoading();
        }

        #endregion

        #region Spesific Functions


        /// <summary>
        /// Prepares every aspect of the Base Engine
        /// Loads and sets up managers, level loading system, save system etc.
        /// </summary>
        private async void InitiateBootLoading() {
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
            await B_CentralEventSystem.CentralEventSystemStrapping();
            
            await Bridge.SetupBridge(this);

            if (!HasTutorial) Enum_MainSave.TutorialPlayed.SetData(1);
            await effectsFunctions.VFXManagerStrapping();
            await B_EffectsManager.EffectsManagerStrapping();

            GameStates.Start.SetGameState();
            
            B_GameControl.SaveAllGameData();
            
            await Task.Delay(100);
            
            B_GUIManager.ActivateOnePanel(Enum_MenuTypes.Menu_Main, .2f);

            if (TargetFrameRate < 0) {
                TargetFrameRate = -1;
            }
            Application.targetFrameRate = TargetFrameRate;

            B_LevelControl.LoadLevel(Enum_MainSave.PlayerLevel.ToInt());
        }
        private void OnApplicationQuit() {
            B_GameControl.SaveAllGameData();
            DOTween.KillAll();
        }


        private void OnApplicationPause(bool pauseStatus) {
            #if UNITY_IOS
            B_GameControl.SaveAllGameData();
            
            DOTween.KillAll();
            #endif
        }
        private float deltaTime;
        
        void Update()
        {
            if(ShowFps)
                deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        void OnGUI()
        {
            if(ShowFps)
                ShowFPS();
        }

        private void ShowFPS()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }

        #endregion

    }
}