using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletMainManger : MonoBehaviour
{
    public BulletSpawner _bulletSpawner;
    private List<BulletBody> _activeBulletList = new List<BulletBody>();

    public void Initialize()
    { 
        _bulletSpawner = transform.GetChild(0).GetComponent<BulletSpawner>();
        _activeBulletList = new List<BulletBody> ();

        _bulletSpawner.Initialize();

        EventSystemReference.Instance.BulletRequestEventHandler.AddListener(SpawnBulletRequest);

        EventSystemReference.Instance.BulletPutBulletBackToSleepEventHandler.AddListener(PutBulletBackToSleep);
    }

    private void SpawnBulletRequest(BulletContiner continer)
    {
        BulletBody temp = _bulletSpawner.SpawnBullet(continer);

        _activeBulletList.Add(temp);
    }

    public void UpdateScript()
    {
        int count = _activeBulletList.Count;

        for (int i = count -1; i >= 0; i--) 
        {
            _activeBulletList[i].UpdateScript();
        }
    }

    private void PutBulletBackToSleep(BulletBody bullet)
    {
        _bulletSpawner.SetBulletBackToSleep (bullet);
        _activeBulletList.Remove(bullet);
    }
}
