using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESSpawner : MonoBehaviour
{
    private Queue<ESBody> _ESBodyQueue;
    private Transform _offScreenPosition;
    private PlayerMainManger playerMainManager;

    public void Initialize()
    {
        playerMainManager = FindObjectOfType<PlayerMainManger>();

        _offScreenPosition = transform.GetChild(0);

        int count = transform.GetChild(1).childCount;

        _ESBodyQueue = new Queue<ESBody>();

        for (int i = 0; i < count; i++)
        {
            ESBody es = transform.GetChild(1).GetChild(i).GetComponent<ESBody>();

            es.Initialize();
            
            es.transform.position = _offScreenPosition.position;

            _ESBodyQueue.Enqueue(es);
        }
    }

    

    public List<ESBody> Spawn(int count, float yOffsetAheadOfPlayer)
    {
        Vector3 shipPosition = playerMainManager.ShipTransform.position;
        List<ESBody> list = new List<ESBody>();

        if (count > _ESBodyQueue.Count) 
        {
            count = _ESBodyQueue.Count;
        }

        for (int i = 0; i < count; i++) 
        {
            float x = shipPosition.x;
            float y = shipPosition.y + yOffsetAheadOfPlayer;
            float z = 0;

            Vector3 pop = new Vector3(x, y, z);

            ESBody temp = _ESBodyQueue.Dequeue();

            temp.transform.position = pop;

            temp.gameObject.SetActive(true);

            list.Add(temp);
        }

        return list;
    }

    public void PutESBackToSleep(ESBody es)
    {
        es.RemoveObject();

        es.transform.position = _offScreenPosition.position;

        _ESBodyQueue.Enqueue(es);
    }
}