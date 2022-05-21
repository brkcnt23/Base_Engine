using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class EditorCleanupSignaller : MonoBehaviour {
#if UNITY_EDITOR
    
    bool levelLoaded = false;
    string lastLevel {
        get => PlayerPrefs.GetString("EDITOR_LAST_LEVEL", "");
        set => PlayerPrefs.SetString("EDITOR_LAST_LEVEL", value);
    }
    void Update() {
        TryGetComponent(out B_LevelCreator levelCreator);
        if (!levelLoaded) {
            if(EditorApplication.isPlaying) return;
            if(lastLevel.IsNullOrWhitespace()) return;
            levelLoaded = true;
            levelCreator.SelectedLevel = lastLevel;
            levelCreator.EditorPlaytimeLoad();
        }
        if (EditorApplication.isPlayingOrWillChangePlaymode) {
            if(EditorApplication.isPlaying) return;
            lastLevel = levelCreator.SelectedLevel;
            levelCreator.EditorPlaytimeClear();
            levelLoaded = false;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    void Test() {
        Debug.Log("Testing_1");
    }
    
    // [InitializeOnEnterPlayMode()]
    // [initalize]

    // [initalize]
#endif
}