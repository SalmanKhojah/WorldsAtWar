using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private Queue<BulletBody> _bulletBodyQueue;
    private Transform _offScreenPosition;

    public void Initialize()
    {
        _bulletBodyQueue = new Queue<BulletBody>();

        int count = transform.GetChild(1).childCount;

        _offScreenPosition = transform.GetChild(0);

        for (int i = 0; i < count; i++)
        {
            BulletBody temp = transform.GetChild(1).GetChild(i).GetComponent<BulletBody>();

            temp.Initialize();

            temp.transform.position = _offScreenPosition.position;

            temp.gameObject.SetActive(false);

            _bulletBodyQueue.Enqueue(temp);
        }
    }

    public BulletBody SpawnBullet(BulletContiner continer)
    {
        if (_bulletBodyQueue.Count > 0)
        {
            BulletBody bullet = _bulletBodyQueue.Dequeue();

            bullet.gameObject.SetActive(true);

            string layerName = continer.bulletType == BulletType.PlayerNoramlBullet ? "PlayerBullets" : "EnemyBullets";
            bullet.gameObject.layer = LayerMask.NameToLayer(layerName);
            float directionAngle = Mathf.Atan2(continer.direction.y, continer.direction.x) * Mathf.Rad2Deg;

            bullet.transform.position = continer.position; 
            bullet.transform.rotation = Quaternion.Euler(0, 0, directionAngle - 90);

            Vector3 directionVector = Quaternion.Euler(0, 0, directionAngle) * Vector3.right * continer.speed;

            switch (continer.bulletType)
            {
                case BulletType.PlayerNoramlBullet:
                    {
                        PlayerNormalBulletObject bulletLogic = new PlayerNormalBulletObject();

                        bulletLogic.Initialization(directionVector, continer.damage);

                        bullet.SetupData(bulletLogic, continer);

                    }
                    break;
                case BulletType.EnemyNormalBullet:
                    {
                        EnemyNormalBulletObject bulletLogic = new EnemyNormalBulletObject();

                        bulletLogic.Initialization(directionVector, continer.damage);

                        bullet.SetupData(bulletLogic, continer);
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
            return bullet;
        }
        return null;
    }

    public void SetBulletBackToSleep(BulletBody bullet)
    {
        bullet.RemoveObject();

        bullet.transform.position = _offScreenPosition.position;
        bullet.gameObject.SetActive(false);

        _bulletBodyQueue.Enqueue(bullet);
    }
}
