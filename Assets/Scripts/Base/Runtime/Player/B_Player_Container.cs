using System;
using System.Threading.Tasks;
using Base;
using Sirenix.OdinInspector;
using UnityEngine;
[Serializable]
public class B_Player_Container : B_ManagerBase {
    
    [HideLabel]
    public B_Player_Container_Data Data;

    public override Task ManagerStrapping() {
        return base.ManagerStrapping();
    }

    public override Task ManagerDataFlush() {
        return base.ManagerDataFlush();
    }

}