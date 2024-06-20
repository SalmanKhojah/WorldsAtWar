using UnityEngine;
[System.Serializable]
public class GameDataContiner
{
    public int _scoreCount;
    public Vector3 _playerPosition;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameDataContiner()
    {
        _scoreCount = 0;
        _playerPosition = Vector3.zero;
    }
}