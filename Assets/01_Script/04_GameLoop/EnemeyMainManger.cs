using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemeyMainManger : MonoBehaviour
{
    private List<EnemyBody> _activeEnemyObjectList;
    private EnemySpawner _enemySpawnerController;

    public void Initialize()
    {
        _enemySpawnerController = transform.GetChild(0).GetComponent<EnemySpawner>();

        _enemySpawnerController.Initialize();
        EventSystemReference.Instance.EnemyPutObjectBackToSleepEventHandler.AddListener(PutObjectToSleep);
    }

    public void BeginWave(int numberOfEnemies, float yOffsetAheadOfPlayer)
    {
        _activeEnemyObjectList = _enemySpawnerController.SpawnEnemy(numberOfEnemies, yOffsetAheadOfPlayer);

        _activeEnemyObjectList[_activeEnemyObjectList.Count - 1].BeginObject();
    }

    public void UpdateScript()
    {
        int count = _activeEnemyObjectList.Count;

        for (int i = count - 1; i >= 0; i--)
        {
            _activeEnemyObjectList[i].UpdateScript();

            if (i > 0)
            {
                if (_activeEnemyObjectList[i].StartMovingCheck == true && _activeEnemyObjectList[i - 1].IsCountingDown == false) 
                {
                    _activeEnemyObjectList[i - 1].IsCountingDown = true;
                }
            }
        }
    }

    private void PutObjectToSleep(EnemyBody enemy)
    {
        int count = _activeEnemyObjectList.Count;

        for (int i = 0; i < count; i++)
        {
           if (i > 0 && _activeEnemyObjectList[i] == enemy) 
           {
                _activeEnemyObjectList[i - 1].IsCountingDown = true;
           }
        }
        _activeEnemyObjectList.Remove(enemy);
        _enemySpawnerController.PutEnemyBackToSleep(enemy); 
    }

    public void PutAllEnemiesToSleep()
    {
        int count = _activeEnemyObjectList.Count;

        for(int i = count - 1; i >=0; i--)
        {
            EnemyBody enemy = _activeEnemyObjectList[i];

            _activeEnemyObjectList.Remove(enemy);
            _enemySpawnerController.PutEnemyBackToSleep(enemy);

        }
    }

}
