using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainManger : MonoBehaviour , IDataPersistence
{ 
    private float _speed = 5;
    private float _normalShotSpeed = 13.0f;

    private PlayerDeathController _playerDeathController; 

    private int _score;

    private SpaceShipObject _shipObject;

    public Transform ShipTransform => _shipObject.transform;

    private float _leftRightInputValue;
    private float _upDownInputValue;

    private int _currentHealth = 0;
    private int _maxHealth = 1;



    
   
    public void Initialize()
    {
        _leftRightInputValue = 0;
        _upDownInputValue = 0;
        //_score = 0;
        _shipObject = transform.GetChild(0).GetComponent<SpaceShipObject>();

        _shipObject.transform.position = Vector3.zero;  

        EventSystemReference.Instance.SendScoreToPlayerEventHandler.AddListener(UpdatePlayerScore);

        _playerDeathController = GetComponent<PlayerDeathController>();
        _playerDeathController.Initialize();

        SetPlayerShipActiveState(true);

        _currentHealth = _maxHealth;
    }

    public void SetScore()
    {
        EventSystemReference.Instance.SendScoreToPlayerEventHandler.Invoke(_score);
    }

    public void SetPlayerShipActiveState(bool flag)
    {
        _shipObject.gameObject.SetActive(flag);
    }

    public void UpdateScript()
    {
        Vector3 moveVector = _shipObject.transform.position;

        moveVector.x += _leftRightInputValue * _speed * Time.deltaTime;
        moveVector.y += _upDownInputValue * _speed * Time.deltaTime;

        // if (moveVector.x > 8)
        // {
        //     moveVector.x = 8;
        // }
        // if (moveVector.x < -8)
        // {
        //     moveVector.x = -8;
        // }

        // if (moveVector.y > 4)
        // {
        //     moveVector.y = 4;
        // }
        // if (moveVector.y < -4)
        // {
        //     moveVector.y = -4;
        // }

        _shipObject.transform.position = moveVector;
    }

    public bool UpdateScriptPlayerDeath()
    {
        bool result = false;

        _shipObject.transform.position = _playerDeathController.UpdateScript();

        if (_shipObject.transform.position == Vector3.zero)
        {
            result = true;
        }

        return result;
    }

    public void ReadUserLeftRightMovementInput(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        if (context.performed)
        {
            if (value > 0)
            {
                _leftRightInputValue = 1;
            }
            else if (value < 0)
            {
                _leftRightInputValue = -1;
            }
        }
        else if (context.canceled)
        {
            _leftRightInputValue = 0;
        }
    }

    public void ReadUserUpDownMovement(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        if (context.performed)
        {
            if (value > 0)
            {
                _upDownInputValue = 1;
            }
            else if (value < 0)
            {
                _upDownInputValue = -1;
            }
        }
        else if (context.canceled)
        {
            _upDownInputValue = 0;
        }
    }

    public void ReadUserShootInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            BulletContiner continer = new BulletContiner();

            continer.position = _shipObject.transform.position;
            continer.bulletType = BulletType.PlayerNoramlBullet;
            continer.damage = 5;
            continer.beamColor = Color.magenta;
            continer.speed = _normalShotSpeed;
            continer.direction = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;


            EventSystemReference.Instance.BulletRequestEventHandler.Invoke(continer);
        }
    }

    private void UpdatePlayerScore(int score)
    {
        _score += score;

        EventSystemReference.Instance.UpdateUiScoreEventTextHandler.Invoke(_score);
    }

    public void WriteDataToFileContiner(ref GameDataContiner continer)
    {
        continer._scoreCount = _score;
        continer._playerPosition = _shipObject.transform.position;
    }

    public void ReadDataToFileContiner(GameDataContiner continer)
    {
        _score = continer._scoreCount;
        _shipObject.transform.position = continer._playerPosition;
    }

    public void TakeDamage(int Amount)
    {
        _currentHealth -= Amount;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            EventSystemReference.Instance.GameManagerStartPlayerDeathSequenceHandler.Invoke();
            _playerDeathController.StartMovingToCenterLerpProcess(_shipObject.transform.position);
        }
    }

   
}
