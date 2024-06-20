using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBody : MonoBehaviour
{
    private BulletBlueprint _bulletLogic;
    private SpriteRenderer _spriteRenderer;

    public void Initialize()
    {
        _bulletLogic = null;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupData(BulletBlueprint bullet, BulletContiner continer)
    {
       GetComponent<Collider2D>().enabled = true;
        _bulletLogic = bullet;
        transform.position  = continer.position;
        _spriteRenderer.color = continer.beamColor;
    }

    public void UpdateScript()
    {
        //CheckBoundery();

        if (_bulletLogic != null)
        {
            transform.position = _bulletLogic.UpdateScript(transform.position);
        }
        else
        {
            RemoveBullet();
        }
    }

    private void CheckBoundery()
    {
        if (transform.position.y > 6 || transform.position.y < -6)
        {
            RemoveBullet();
        }
        else if (transform.position.x > 11 || transform.position.x < -11)
        {
            RemoveBullet();
        }
    }

    public int DealDamage()
    {
        if (_bulletLogic != null)
        {
            return _bulletLogic.Damage;
        }
        return 0;
    }

    public void RemoveBullet()
    {
        GetComponent<Collider2D>().enabled = false; 
        EventSystemReference.Instance.BulletPutBulletBackToSleepEventHandler.Invoke(this);
    }

    public void RemoveObject()
    {
        _bulletLogic = null;
    }
}
