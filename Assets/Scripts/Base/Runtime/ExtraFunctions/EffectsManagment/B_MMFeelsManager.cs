using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class B_MMFeelsManager : MonoBehaviour
{
    public static B_MMFeelsManager instance;
    
    
    public MMFeedbacks[] MMFeedbacks;
    Dictionary<HapticTypes, MMFeedbacks> _dictionary = new Dictionary<HapticTypes, MMFeedbacks>();

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        
        for (int i = 0; i < Enum.GetValues(typeof(HapticTypes)).Length; i++) {
            _dictionary.Add((HapticTypes)i, MMFeedbacks[i]);
        }
        
    }

    void OnDisable() {
        instance = null;
    }


    [ContextMenu("Build Feedbacks")]
    void BuildFeedbacks() {
        MMFeedbacks = new MMFeedbacks[Enum.GetValues(typeof(HapticTypes)).Length];
        for (int i = 0; i < Enum.GetValues(typeof(HapticTypes)).Length; i++) {
            GameObject obj = new GameObject("Feedback_" + (HapticTypes)i);
            obj.transform.SetParent(transform);
            MMFeedbacks feedback = obj.AddComponent<MMFeedbacks>();
            MMFeedbacks[i] = feedback;
        }
    }
    
    public void PlayFeedback(HapticTypes type) {
        _dictionary[type].PlayFeedbacks();
    }
    
}


