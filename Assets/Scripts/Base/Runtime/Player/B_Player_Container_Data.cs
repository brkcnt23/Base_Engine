using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_Container", menuName = "Base/Player")]
[InlineEditor]
public class B_Player_Container_Data : ScriptableObject {
    
    public float Data_MovementSpeed;
    public float Data_Health;
    public float Data_HighScore;
    public float Data_Score;
    public float Data_CoinTotal;
    public float Data_CoinGained;
    
}