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

            opbs.transform.position = _offScreenPosition.position;

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
            Renderer fireRenderer = transform.GetChild(1).GetChild(i).GetChild(0).GetChild(0).GetComponent<Renderer>();
            Renderer smokeRenderer = transform.GetChild(1).GetChild(i).GetChild(0).GetChild(1).GetComponent<Renderer>();
            Vector3 fireGFXSize = fireRenderer.bounds.size;
            Vector3 smokeGFXSize = smokeRenderer.bounds.size;
            float fireGFXBaseRadius = Mathf.Min(fireGFXSize.x, fireGFXSize.y) * 0.5f;
            float smokeGFXBaseRadius = Mathf.Min(smokeGFXSize.x, smokeGFXSize.y) * 0.5f;
            Vector3 randomPos;
            Vector3 randomPosSmoke;
            bool positionOccupied;
            bool positionOccupiedSmoke;
            float minYValue = -1.25f;
            float maxYValue = 8.61f;
            float minXvalue = 4971.9f;
            float maxXvalue = 5040;
            float randomPositionOffset = 30;


            Vector3 youngOmarPos = playerMainManager.YoungOmarTransform.position;

            float x = youngOmarPos.x + 10;
            float y = youngOmarPos.y + 5;
            float z = youngOmarPos.z;

            Vector3 basePosition = new Vector3(x, y, z);

            float randomScale = Random.Range(0.8f, 4.2f);

            float randomScaleSmoke = Random.Range(1f, 3f);

            float adjustedRadiusFire = fireGFXBaseRadius * randomScale;
            float adjustedRadiusSmoke = smokeGFXBaseRadius * randomScaleSmoke;

            do
            {
                randomPos = basePosition + new Vector3(Random.Range(-randomPositionOffset, randomPositionOffset), Random.Range(-randomPositionOffset, randomPositionOffset), 0);

                randomPosSmoke = basePosition + new Vector3(0f, 5f, 0f) + new Vector3(Random.Range(-randomPositionOffset, randomPositionOffset), Random.Range(-randomPositionOffset, randomPositionOffset), 0);

                positionOccupied = Physics2D.OverlapCircle(new Vector2(randomPos.x, randomPos.y), adjustedRadiusFire) != null;

                positionOccupiedSmoke = Physics2D.OverlapCircle(new Vector2(randomPosSmoke.x, randomPosSmoke.y), adjustedRadiusSmoke) != null;


            } while (positionOccupied || positionOccupiedSmoke || randomPos.y < minYValue || randomPos.y > maxYValue || randomPos.x < minXvalue || randomPos.x > maxXvalue || randomPosSmoke.y < minYValue || randomPosSmoke.y > maxYValue || randomPosSmoke.x < minXvalue || randomPosSmoke.x > maxXvalue);

            OPBSBody temp = _OPBSQueue.Dequeue();

            Transform smokeGFX = temp.transform.GetChild(0).GetChild(1);

            temp.transform.localScale = new Vector3(randomScale, randomScale, 1);
            temp.transform.position = randomPos;

            smokeGFX.transform.localScale = new Vector3(randomScaleSmoke, randomScaleSmoke, 1);
            smokeGFX.transform.position = randomPosSmoke;

            

            list.Add(temp);
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