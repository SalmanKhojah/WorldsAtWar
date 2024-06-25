using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OPBSMainManager : MonoBehaviour
{
    private List<OPBSBody> _activeOPBSObjectList;
    private OPBSSpawner _OPBSSpawnerController;
    private PlayerMainManger playerMainManager;

    private Vector3 _playerStartPosition;
    private float _movementThreshold = 30f; // Distance the player needs to move to trigger the wave
    private bool _beginOPBSFlag;
    private bool _playerHasMovedEnough; 

    public void Initialize()
    {
        _playerHasMovedEnough = false;
        _beginOPBSFlag = false;
        

        playerMainManager = FindObjectOfType<PlayerMainManger>();

        _OPBSSpawnerController = transform.GetChild(0).GetComponent<OPBSSpawner>();

        _OPBSSpawnerController.Initialize();
        EventSystemReference.Instance.OPBSPutObjectBackToSleepEventHandler.AddListener(PutObjectToSleep);


        _playerStartPosition = playerMainManager.YoungOmarTransform.position;
    }

    public void BeginOPBS(int numberOfFires)
    {
        if (!_beginOPBSFlag)
        _activeOPBSObjectList = _OPBSSpawnerController.Spawn(numberOfFires);

        if (_activeOPBSObjectList.Count > 0)
        {
            _activeOPBSObjectList[_activeOPBSObjectList.Count - 1].BeginObject();
            _beginOPBSFlag = true;
        }


    }
    private bool PlayerMovedEnough()
    {
        // if (playerMainManager.ShipTransform.position == null || _playerStartPosition == null)
        // {
        //     return false; 
        // }

        float distanceMoved = Vector3.Distance(_playerStartPosition, playerMainManager.YoungOmarTransform.position);
        return distanceMoved >= _movementThreshold;
    }

    public void UpdateScript()
    {
            BeginOPBS(5);



            int count = _activeOPBSObjectList.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                _activeOPBSObjectList[i].UpdateScript();

                // if (i > 0)
                // {
                //     if (_activeOPBSObjectList[i].StartMovingCheck == true && _activeOPBSObjectList[i - 1].IsCountingDown == false)
                //     {
                //         _activeOPBSObjectList[i - 1].IsCountingDown = true;
                //     }
                // }
            }
        }





    private void PutObjectToSleep(OPBSBody opbs)
    {
        int count = _activeOPBSObjectList.Count;

        _activeOPBSObjectList.Remove(opbs);
        _OPBSSpawnerController.PutOPBSBackToSleep(opbs); 
    }
    

// public void PutAllEnemiesToSleep()
// {
//     if (_OPBSObjectList == null || _OPBSSpawnerController == null) return;

//     int count = _OPBSObjectList.Count;

//     for (int i = count - 1; i >= 0; i--)
//     {
//         ESBody es = _OPBSObjectList[i];

//         _OPBSObjectList.Remove(es);
//         _OPBSSpawnerController.PutOPBSBackToSleep(es);
//     }
// }

}

