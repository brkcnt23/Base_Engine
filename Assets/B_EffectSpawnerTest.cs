using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(50)]
public class B_EffectSpawnerTest : MonoBehaviour {
    
    public Enum_Particles particleType;
    
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 spawnRotation;
    
    [FoldoutGroup("Events")]
    [SerializeField] private UnityEvent OnAwakeEvent;
    [FoldoutGroup("Events")]
    [SerializeField] private UnityEvent OnEnableEvent;
    [FoldoutGroup("Events")]
    [SerializeField] private UnityEvent OnStartEvent;
    [FoldoutGroup("Events")]
    [SerializeField] private UnityEvent OnGameStartedEvent;
    [FoldoutGroup("Events")]
    [SerializeField] private UnityEvent OnGameEndedEvent;
    
    public B_PooledParticle PlayEffect() {
        return particleType.SpawnAParticle(spawnPosition, Quaternion.Euler(spawnRotation)).PlayParticle();
    }
    
    public void PlayEffectNoReturn() {
        particleType.SpawnAParticle(spawnPosition, Quaternion.Euler(spawnRotation)).PlayParticle();
    }

    private async void Awake() {
        await Task.Delay(100);
        B_CentralEventSystem.BTN_OnStartPressed.AddFunction(() => OnGameStartedEvent?.Invoke());
        B_CentralEventSystem.OnAfterLevelEnd.AddFunction(() => OnGameEndedEvent?.Invoke());
        OnAwakeEvent?.Invoke();
    }

    private async void OnEnable() {
        await Task.Delay(100);
        OnEnableEvent?.Invoke();
    }

    private async void Start() {
        await Task.Delay(100);
        OnStartEvent?.Invoke();
    }

}
