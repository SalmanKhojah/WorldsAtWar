using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainManager : MonoBehaviour
{
    public float _speed = 5;
    private SpaceShip _shipObject;
    private int _points;


    public void Init()
    {
        _shipObject = transform.GetChild(0).GetComponent<SpaceShip>();

        _points = 0;
    }

    // Update is called once per frame
    public void UpdateScript()
    {
        Vector3 moveVector = _shipObject.transform.position;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveVector.x = moveVector.x + (_speed * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveVector.x = moveVector.x + (-_speed * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveVector.y = moveVector.y + (_speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveVector.y = moveVector.y + (-_speed * Time.deltaTime);
        }

        if (moveVector.x > 10.844)
        {
            moveVector.x = 10.844f;
        }
        if (moveVector.x < -10.844)
        {
            moveVector.x = -10.844f;
        }
        if (moveVector.y > 3.791)
        {
            moveVector.y = 3.791f;
        }
        if (moveVector.y < -3.791)
        {
            moveVector.y = -3.791f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BulletContainer container = new BulletContainer();
            //float twopi = (2f * Mathf.PI);

            
            container.position = _shipObject.transform.position;
            container.bulletType = BulletSpawner.BulletType.NormalBullet;
            container.BeamColor = Color.red;
                
            EventSystemRef.instance.BulletRequest.Invoke(container);

            _points++;
            
            EventSystemRef.instance.UpdateTextHandler.Invoke(_points.ToString());

            // for (float k = 0; k < twopi; k += Time.deltaTime * 100)
            // {
            //     container.position.x = _shipObject.transform.position.x + 2 * Mathf.Cos(k);
            //     container.position.y = _shipObject.transform.position.y + 2 * Mathf.Sin(k);
            //     container.bulletType = BulletSpawner.BulletType.NormalBullet;
            //     container.BeamColor = Color.red;
                
            //     EventSystemRef.instance.BulletRequest.Invoke(container);
            // }
        }
                _shipObject.transform.position = moveVector;
        }



    }