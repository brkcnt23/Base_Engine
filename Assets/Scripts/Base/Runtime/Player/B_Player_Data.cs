using System.Runtime.CompilerServices;
using Base;
public static class B_Player_Data {
    
    private static B_Player_Container _playerContainerFunctions;

    public static float Data_Speed {
        get => _playerContainerFunctions.Data.Data_MovementSpeed;
        set { }
    }
    
    public static float Data_Health {
        get => _playerContainerFunctions.Data.Data_Health;
        set { }
    }
    
    public static float Data_HighScore {
        get => _playerContainerFunctions.Data.Data_HighScore;
        set { }
    }
    
    public static float Data_Score {
        get => _playerContainerFunctions.Data.Data_Score;
        set { }
    }
    
    public static float Data_CoinTotal {
        get => _playerContainerFunctions.Data.Data_CoinTotal;
        set { }
    }
    
    public static float Data_CoinGained {
        get => _playerContainerFunctions.Data.Data_CoinGained;
        set { }
    }

    public static void Setup(B_Player_Container _playerContainer) {
        
        _playerContainerFunctions = _playerContainer;
        _playerContainerFunctions.ManagerStrapping();
        
        B_CentralEventSystem.OnAfterLevelDisablePositive.AddFunction(OnLevelEnded, true);
        
    }
    
    public static void Player_Health_Set(float _health) {
        _playerContainerFunctions.Data.Data_Health = _health;
    }
    
    public static void Player_Health_Subtract(float _health) {
        _playerContainerFunctions.Data.Data_Health -= _health;
        if(_playerContainerFunctions.Data.Data_Health <= 0) {
            _playerContainerFunctions.Data.Data_Health = 0;
        }
    }
    
    public static void Player_Score_Set(float _score) {
        
        _playerContainerFunctions.Data.Data_Score = _score;
        
    }

    public static void Player_Score_Add(float _score) {
        
        _playerContainerFunctions.Data.Data_Score += _score;

    }
    
    public static void Player_HighScore_Set() {

        if (_playerContainerFunctions.Data.Data_Score > _playerContainerFunctions.Data.Data_HighScore) {
            _playerContainerFunctions.Data.Data_HighScore = _playerContainerFunctions.Data.Data_Score;
        }
        
    }
    
    public static void Player_CoinTotal_Set(float _coinTotal) {
        
        _playerContainerFunctions.Data.Data_CoinTotal = _coinTotal;
        
    }
    
    public static void Player_CoinTotal_Add(float _coinTotal) {
        
        _playerContainerFunctions.Data.Data_CoinTotal += _coinTotal;
        
    }
    
    public static void Player_CoinGained_Set(float _coinGained) {
        
        _playerContainerFunctions.Data.Data_CoinGained = _coinGained;
        
    }
    
    public static void Player_CoinGained_Add(float _coinGained) {
        
        _playerContainerFunctions.Data.Data_CoinGained += _coinGained;
        
    }
    


    private static void OnLevelEnded() {
        
        
        Player_HighScore_Set();
        
    }
    
}