using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPBSSpawner : MonoBehaviour
{
    private Queue<OPBSBody> _OPBSQueue;
    private Transform _offScreenPosition;
    private PlayerMainManger playerMainManager;

    public void Initialize()
    {
        playerMainManager = FindObjectOfType<PlayerMainManger>();

        _offScreenPosition = transform.GetChild(0);

        int count = transform.GetChild(1).childCount;

        _OPBSQueue = new Queue<OPBSBody>();

        for (int i = 0; i < count; i++)
        {
            OPBSBody opbs = transform.GetChild(1).GetChild(i).GetComponent<OPBSBody>();

            opbs.Initialize();

            //opbs.transform.position = _offScreenPosition.position;

            _OPBSQueue.Enqueue(opbs);
        }
    }



    public List<OPBSBody> Spawn(int count)
    {
        List<OPBSBody> list = new List<OPBSBody>();

        if (count > _OPBSQueue.Count)
        {
            count = _OPBSQueue.Count;
        }

        for (int i = 0; i < count; i++)
        {
            // Renderer fireRenderer = transform.GetChild(1).GetChild(i).GetChild(0).GetChild(0).GetComponent<Renderer>();
            // // Renderer smokeRenderer = transform.GetChild(1).GetChild(i).GetChild(0).GetChild(1).GetComponent<Renderer>();
            // BoxCollider2D fireCollider = transform.GetChild(1).GetChild(i).GetChild(1).GetChild(0).GetComponent<BoxCollider2D>();
            // BoxCollider2D smokeCollider = transform.GetChild(1).GetChild(i).GetChild(0).GetChild(1).GetComponent<BoxCollider2D>();
            // // Vector3 fireGFXSize = fireRenderer.bounds.size;
            // // Vector3 smokeGFXSize = smokeRenderer.bounds.size;
            // // float fireGFXBaseRadius = Mathf.Min(fireGFXSize.x, fireGFXSize.y) * 0.5f;
            // // float smokeGFXBaseRadius = Mathf.Min(smokeGFXSize.x, smokeGFXSize.y) * 0.5f;
            // Vector3 randomPos;
            // Vector3 randomPosSmoke;
            // bool positionOccupied;
            // bool positionOccupiedSmoke;
            // float minYValue = -1.25f;
            // float maxYValue = 8.61f;
            // float minXvalue = 4973.9f;
            // float maxXvalue = 5072.1f;
            // float rangeX = 98.2f;
            // float rangeY = 9.85f;


            // Vector3 youngOmarPos = playerMainManager.YoungOmarTransform.position;

            // float x = youngOmarPos.x + 10;
            // float y = youngOmarPos.y;
            // float z = youngOmarPos.z;

            // Vector3 basePosition = new Vector3(x, y, z);

            // float randomScale = Random.Range(0.8f, 3.2f);

            // float randomScaleSmoke = Random.Range(0.5f, 2f);

            // // float adjustedRadiusFire = fireGFXBaseRadius * randomScale;
            // // float adjustedRadiusSmoke = smokeGFXBaseRadius * randomScaleSmoke;
            // fireCollider.size = new Vector2(fireCollider.size.x * randomScale, fireCollider.size.y * randomScale);
            // smokeCollider.size = new Vector2(smokeCollider.size.x * randomScaleSmoke / 2, smokeCollider.size.y * randomScaleSmoke / 2);

            // // do
            // // {
            //     randomPos = basePosition + new Vector3(Random.Range(0, rangeX), Random.Range(0, rangeY), 0);

            //     randomPosSmoke = basePosition + new Vector3(0f, 4f, 0f) + new Vector3(Random.Range(0, rangeX), Random.Range(0, rangeY), 0);

                // positionOccupied = Physics2D.OverlapCircle(new Vector2(randomPos.x, randomPos.y), 1) != null;

                // positionOccupiedSmoke = Physics2D.OverlapCircle(new Vector2(randomPosSmoke.x, randomPosSmoke.y), 1) != null;


            // } while (randomPos.y < minYValue || randomPos.y > maxYValue || randomPos.x < minXvalue || randomPos.x > maxXvalue || randomPosSmoke.y < minYValue || randomPosSmoke.y > maxYValue || randomPosSmoke.x < minXvalue || randomPosSmoke.x > maxXvalue);

            OPBSBody temp = _OPBSQueue.Dequeue();

            // Transform smokeGFX = temp.transform.GetChild(0).GetChild(1);

            // temp.transform.localScale = new Vector3(randomScale, randomScale, 1);
            // temp.transform.position = randomPos;

            // smokeGFX.transform.localScale = new Vector3(randomScaleSmoke, randomScaleSmoke, 1);
            // smokeGFX.transform.position = randomPosSmoke;

            

            list.Add(temp);

            temp.gameObject.SetActive(true);
        }

        return list;
    }



    public void PutOPBSBackToSleep(OPBSBody opbs)
    {
        opbs.RemoveObject();

        opbs.transform.position = _offScreenPosition.position;

        _OPBSQueue.Enqueue(opbs);
    }
}