using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    public EnemyUnitObject EnemyPrefab;

    public List<EnemyUnitObject> SpawnEnemy(int count)
    {

        List<EnemyUnitObject> list = new List<EnemyUnitObject>();
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-8.0f, 8.0f);
            float y = Random.Range(6.0f, 10.0f);
            float z = 0;

            Vector3 pop = new Vector3(x,y,z);
            
            EnemyUnitObject temp = Instantiate(EnemyPrefab, pop, transform.rotation);

            list.Add(temp);
        }

        return list;
    }
}
