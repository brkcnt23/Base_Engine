using System.Collections;
using System.Collections.Generic;
using Base;
using Base.UI;
using UnityEngine;
using static Enum_MainSave;

public class ExampleFunctions : MonoBehaviour {
    
    void Start() {
        B_CentralEventSystem.BTN_OnStartPressed.AddFunction(ExampleMain, false);
        if (TryGetComponent(out Collider collider)) {
            Debug.Log(collider.GetRandomPoint());
            Debug.Log(collider.GetRandomPoint(5));
        }
    }
    
    void ExampleMain() {
        ExampleParticle().RunCoroutine(2);
        ExampleSave().RunCoroutine();
        B_ExtentionFunctions.RunWithDelay(ExampleUI, 2f);
    }

    IEnumerator ExampleParticle() {
        for (int i = 0; i < 5; i++) {
            Enum_Particles.CubeExplosion.SpawnAParticle(transform.position).PlayParticle();
            yield return new WaitForSeconds(.5f);
        }
        yield return new WaitForSeconds(1f);
        B_GameControl.ActivateEndgame(true, 2);
    }

    IEnumerator ExampleSave() {
        string _oldPlayerCoin = PlayerCoin.DataToString();
        Debug.Log($"Current Player Coin is : {PlayerCoin.DataToString()}");
        yield return new WaitForSeconds(1f);
        Debug.Log($"Setting PlayerCoin Data");
        PlayerCoin.SetData(Random.Range(1, 9999));
        yield return new WaitForSeconds(.5f);
        Debug.Log("Saving Completed");
        yield return new WaitForSeconds(.1f);
        Debug.Log($"Old Player Coin Was {_oldPlayerCoin} // Current Player Coin is : {PlayerCoin.DataToString()}");
        yield return null;
    }

    void ExampleUI() {
        // Enum_Menu_PlayerOverlayComponent.TestScore.GetText().ChangeText(PlayerCoin.DataToString());
    }
}