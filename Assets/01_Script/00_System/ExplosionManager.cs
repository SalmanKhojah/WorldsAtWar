using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosionManager : MonoBehaviour
{
    private Transform _offscreeanPosition;
    private Queue<ExposionObject> _expolsionBodyQueue;
    private List<ExposionObject> _activeExoplsionBodyList;

    public void Initialize()
    {
        _expolsionBodyQueue = new Queue<ExposionObject>();
        _activeExoplsionBodyList = new List<ExposionObject>();

        Sprite[] explosionSpriteArray = ResourcesLoader.Instance.GetExposionSpritesArray();
        AudioClip clip = ResourcesLoader.Instance.GetAudioClip("EXPLOSION");

        _offscreeanPosition = transform.GetChild(0);

        int count = transform.GetChild(1).childCount;

        for (int i = 0; i < count; i++)
        {
            ExposionObject temp = transform.GetChild(1).GetChild(i).GetComponent<ExposionObject>();

            temp.Initialize(explosionSpriteArray, clip);

            temp.transform.position = _offscreeanPosition.position;

            _expolsionBodyQueue.Enqueue(temp);
        }
        EventSystemReference.Instance.ExplostionRequestEventHandler.AddListener(SpwanExposionBodyEvent);
    }

    public void UpdateScript()
    {
        int count = _activeExoplsionBodyList.Count;

        for (int i = count - 1; i >= 0; i--)
        {
            bool result = _activeExoplsionBodyList[i].UpdateScript();

            if (result)
            {
                PutExposionBackToSleep(_activeExoplsionBodyList[i]);
            }
        }
    }

    public bool UpdateExpolsitionBodySequence()
    {
        int count = _activeExoplsionBodyList.Count;

        for (int i = count - 1; i >= 0; i--)
        {
            bool result = _activeExoplsionBodyList[i].UpdateScript();

            if (result )
            {
                if (i != 0)
                {
                    _activeExoplsionBodyList[i - 1].StartAnimatinFlag = true;
                    _activeExoplsionBodyList[i - 1].PlaySoundEffect();
                }
                

                PutExposionBackToSleep(_activeExoplsionBodyList[i]);
            }
        }

        return _activeExoplsionBodyList.Count == 0;
    }

    public bool UpdateBigExpolsitionBody()
    {
        bool result = false;

        int count = _activeExoplsionBodyList.Count;

        for (int i = count - 1; i >= 0; i--)
        {
            result = _activeExoplsionBodyList[i].UpdateScript();

            if (result)
            {
                PutExposionBackToSleep(_activeExoplsionBodyList[i]);
            }
        }

        return result;
    }

    private void SpwanExposionBodyEvent(Vector3 position)
    {
        ExposionObject temp = _expolsionBodyQueue.Dequeue();

        temp.BeginEffect(position);

        _activeExoplsionBodyList.Add(temp);
    }

    public void SpwanExposionBodyForPlayerDeath(int expositionCount)
    {
        Vector3[] positionArray = new Vector3[expositionCount];

        for (int i = 0;i < expositionCount;i++)
        {
            float r = 1 * Mathf.Sqrt(Random.value);
            float theta = Random.value * 2 * Mathf.PI;

            Vector3 expositionPosition = new Vector3();

            expositionPosition.x = 0 + r * Mathf.Cos(theta);
            expositionPosition.y = 0 + r * Mathf.Sin(theta);

            positionArray[i] = expositionPosition;


        }

        SpwanExposionBodySequenceEvent(positionArray);

    }

    private void SpwanExposionBodySequenceEvent(Vector3[] position)
    {
        int count = position.Length;

        for (int i = 0; i < count; i++)
        {
            ExposionObject temp = _expolsionBodyQueue.Dequeue();

            temp.PrimeEffect(position[i]);

            _activeExoplsionBodyList.Add(temp);
        }

        _activeExoplsionBodyList[_activeExoplsionBodyList.Count - 1].StartAnimatinFlag = true;
        _activeExoplsionBodyList[_activeExoplsionBodyList.Count - 1].PlaySoundEffect();
    }

    public void SpwanBigExpostionRequest()
    {
        ExposionObject temp = _expolsionBodyQueue.Dequeue();

        temp.BeginBigVersion(Vector3.zero);

        _activeExoplsionBodyList.Add(temp);
    }

    private void PutExposionBackToSleep(ExposionObject body)
    {
        body.gameObject.SetActive(false);
        body.transform.position = _offscreeanPosition.position;

        _expolsionBodyQueue.Enqueue(body);
        _activeExoplsionBodyList.Remove(body);
    }

    public void PutExpolsionToSleep()
    {
        int count = _activeExoplsionBodyList.Count;

        for (int i = count - 1; i >= 0; i--)
        {
            ExposionObject body = _activeExoplsionBodyList[i];

            body.gameObject.SetActive(false);
            body.transform.position = _offscreeanPosition.position;
            body.StartAnimatinFlag = false;

            _expolsionBodyQueue.Enqueue(body);
            _activeExoplsionBodyList.Remove(body);
        }
    }

}
