using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ESMainManager : MonoBehaviour
{
    public List<ESBody> _activeESObjectList;
    private ESSpawner _enemyScannerSpawnerController;
    private PlayerMainManger playerMainManager;

    private Vector3 _playerStartPosition;
    private float _movementThreshold = 10f; // Distance the player needs to move to trigger the wave
    private bool _beginWaveFlag;
    private bool _playerHasMovedEnough; 

    public void Initialize()
    {
        _playerHasMovedEnough = false;
        _beginWaveFlag = false;
        

        playerMainManager = FindObjectOfType<PlayerMainManger>();

        _enemyScannerSpawnerController = transform.GetChild(0).GetComponent<ESSpawner>();

        _enemyScannerSpawnerController.Initialize();
        EventSystemReference.Instance.ESPutObjectBackToSleepEventHandler.AddListener(PutObjectToSleep);


        _playerStartPosition = playerMainManager.ShipTransform.position;
    }

    public void BeginWave(int numberOfEnemies, float yOffSetAheadOfPlayer)
    {
        if (!_beginWaveFlag)
        _activeESObjectList = _enemyScannerSpawnerController.Spawn(numberOfEnemies, yOffSetAheadOfPlayer);

        _activeESObjectList[_activeESObjectList.Count - 1].BeginObject();
        _beginWaveFlag = true;

    }
    private bool PlayerMovedEnough()
    {
        // if (playerMainManager.ShipTransform.position == null || _playerStartPosition == null)
        // {
        //     return false; 
        // }

        float distanceMoved = Vector3.Distance(_playerStartPosition, playerMainManager.ShipTransform.position);
        return distanceMoved >= _movementThreshold;
    }

    public void UpdateScript()
    {
        if (!_playerHasMovedEnough && PlayerMovedEnough())
        {
            _playerHasMovedEnough = true;
        }

        if (_playerHasMovedEnough)
        {
            BeginWave(1, 10);



            int count = _activeESObjectList.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                _activeESObjectList[i].UpdateScript();

                if (i > 0)
                {
                    if (_activeESObjectList[i].StartMovingCheck == true && _activeESObjectList[i - 1].IsCountingDown == false)
                    {
                        _activeESObjectList[i - 1].IsCountingDown = true;
                    }
                }
            }
        }


    }


    private void PutObjectToSleep(ESBody es)
    {
        int count = _activeESObjectList.Count;

        for (int i = 0; i < count; i++)
        {
           if (i > 0 && _activeESObjectList[i] == es) 
           {
                _activeESObjectList[i - 1].IsCountingDown = true;
           }
        }
        _activeESObjectList.Remove(es);
        _enemyScannerSpawnerController.PutESBackToSleep(es); 
    }

public void PutAllEnemiesToSleep()
{
    if (_activeESObjectList == null || _enemyScannerSpawnerController == null) return;

    int count = _activeESObjectList.Count;

    for (int i = count - 1; i >= 0; i--)
    {
        ESBody es = _activeESObjectList[i];

        _activeESObjectList.Remove(es);
        _enemyScannerSpawnerController.PutESBackToSleep(es);
    }
}

}
