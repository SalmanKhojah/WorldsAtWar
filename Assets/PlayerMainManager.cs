using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainManager : MonoBehaviour
{
    public float _speed = 5;
    public SpaceShip _shipObject;
    public BulletMovement _bulletObject;

    public List<BulletMovement> _bulletList;

    public void Init()
    {
        _bulletList = new List<BulletMovement>();
        _shipObject = transform.GetChild(0).GetComponent<SpaceShip>();
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
            //_bulletFiredFlag = true;
            BulletMovement temp = Instantiate(_bulletObject, _shipObject.transform.position, _shipObject.transform.rotation);

            _bulletList.Add(temp);
        }

        _shipObject.transform.position = moveVector;



        int count = _bulletList.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            Vector3 bulletVector = _bulletList[i].transform.position;

            bulletVector.y = bulletVector.y + 5 * Time.deltaTime;

            _bulletList[i].transform.position = bulletVector;

            if (_bulletList[i].transform.position.y > 10)
            {
                BulletMovement temp = _bulletList[i];
                _bulletList.RemoveAt(i);

                Destroy(temp.gameObject);
            }
        }


    }
}