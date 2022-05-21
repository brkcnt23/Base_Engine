using System;
using System.Threading.Tasks;
using Base.SoundManagement;
using Base.UI;
using Sirenix.OdinInspector;
namespace Base {
    [Serializable]
    public class B_Bridge {
        
        #region Managers
        
        [TabGroup("FirstGroup", "Game Manager")]
        [HideLabel]
        public GameManagerFunctions gameManagerFunctions;
        [TabGroup("FirstGroup", "UI Manager")]
        [HideLabel]
        public UIManagerFunctions UIManager;
        [TabGroup("FirstGroup", "Level Manager")]
        [HideLabel]
        public LevelManagerFunctions levelManagerFunctions;
        [TabGroup("SecondGroup", "Coroutine Manager")]
        [HideLabel]
        public CoroutineRunnerFunctions coroutineRunnerFunctions;
        [TabGroup("SecondGroup", "Camera Manager")]
        [HideLabel]
        public CameraFunctions CameraFunctions;
        [TabGroup("SecondGroup","Audio Manager")]
        [HideLabel]
        public SoundManager AudioFunctions;
        [TabGroup("ThirdGroup", "Player Data")]
        [HideLabel]
        public B_Player_Container PlayerContainer;
        
        #endregion

        public Task SetupBridge(BaseEngine bootLoader) {

            B_CoroutineControl.Setup(bootLoader, coroutineRunnerFunctions);
            B_UIControl.Setup(UIManager);
            B_GameControl.Setup(gameManagerFunctions);
            B_LevelControl.Setup(levelManagerFunctions);
            B_CameraControl.Setup(CameraFunctions);
            SoundControl.Setup(AudioFunctions);
            B_Player_Data.Setup(PlayerContainer);

            return Task.CompletedTask;
        }
        
        #region Control Functions

        public void FlushBridgeData() {
            
        }

        #endregion
        
    }
}