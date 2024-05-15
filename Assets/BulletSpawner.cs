using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public NormalBullet _normalBullet;
    public enum BulletType
    {
        NormalBullet = 0, 
        Missile = 1,
    }

    public BulletBlueprint SpawnBullet (BulletContainer container)
    {
        BulletBlueprint bullet = null;

        switch (container.bulletType)
        {
            case BulletType.NormalBullet:
            {
                NormalBullet normalBullet = Instantiate(_normalBullet, container.position, transform.rotation);

                normalBullet.Init(5, 1);
                
                normalBullet.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = container.BeamColor;

                bullet = normalBullet;
            }
            break;
        default: 
        {

        }
        break;
        }

        return bullet;
    }
}
