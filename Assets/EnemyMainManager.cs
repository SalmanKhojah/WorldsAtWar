using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class EnemyMainManager : MonoBehaviour
{
    public List<EnemyUnitObject> _enemyObjectList;
    private EnemySpawnerController _enemySpawnerController;

    public void Init()
    {
        _enemySpawnerController = transform.GetChild(0).GetComponent<EnemySpawnerController>();

        _enemyObjectList = _enemySpawnerController.SpawnEnemy(15);

        int count = _enemyObjectList.Count;

        for (int i = 0; i < count; i++)
        {
            _enemyObjectList[i].Init();
        }
        _enemyObjectList[0].StartMovingCheck = true;
    }

    public void UpdateScript()
    {
        int count = _enemyObjectList.Count;
        
        for (int i = 0; i < count; i++)
        {
            _enemyObjectList[i].UpdateScript();

            if (i < count - 1)
            {
                if (_enemyObjectList[i].StartMovingCheck == true && _enemyObjectList[i + 1].IsCountingDown == false)
                {
                    _enemyObjectList[i+1].IsCountingDown = true;
                }
            }
        }
    }
}
