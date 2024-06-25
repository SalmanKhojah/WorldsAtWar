using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainManger : MonoBehaviour, IDataPersistence
{
    private float _SpaceShipSpeed = 5;
    private float _YoungOmarSpeed = 4.5f;
    private float _normalShotSpeed = 13.0f;

    private PlayerDeathController _playerDeathController;

    private int _score;

    private SpaceShipObject _shipObject;
    private YoungOmarObject _youngOmarObject;

    public Transform ShipTransform => _shipObject.transform;
    public Transform YoungOmarTransform => _youngOmarObject.transform;

    private float _leftRightInputValue;
    private float _upDownInputValue;

    private int _currentHealth = 0;
    private int _maxHealth = 1;
    private Animator _youngOmarGFXAnimator;
    private Transform _youngOmarObjectTransform;
    public InputActionAsset inputActionAsset;



    public void InitializeYoungOmar()
    {
        EnableActionMap("YoungOmar");
        _leftRightInputValue = 0;
        _upDownInputValue = 0;
        //_score = 0;
        _youngOmarObject = transform.GetChild(1).GetComponent<YoungOmarObject>();
        _youngOmarObjectTransform = transform.GetChild(1);
        _youngOmarGFXAnimator = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Animator>();

        //_youngOmarObject.transform.position = Vector3.zero;  

        EventSystemReference.Instance.SendScoreToPlayerEventHandler.AddListener(UpdatePlayerScore);

        // _playerDeathController = GetComponent<PlayerDeathController>();
        // _playerDeathController.Initialize();

        SetYoungOmarActiveState(true);

        // _currentHealth = _maxHealth;
    }

    public void InitializeSpace()
    {
        DisableActionMap("YoungOmar");
        EnableActionMap("SpaceShip");
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
    public void SetYoungOmarActiveState(bool flag)
    {
        _youngOmarObject.gameObject.SetActive(flag);
    }
    public void SetPlayerShipActiveState(bool flag)
    {
        _shipObject.gameObject.SetActive(flag);
    }
    private string lastVerticalDirection = "up";
    private string lastHorizontalDirection = "right";

    public void UpdateScriptYoungOmar()
    {
        Vector3 moveVector = _youngOmarObject.transform.position;

        moveVector.x += _leftRightInputValue * _YoungOmarSpeed * Time.deltaTime;
        moveVector.y += _upDownInputValue * _YoungOmarSpeed * Time.deltaTime;

        bool isMoving = _leftRightInputValue != 0 || _upDownInputValue != 0;
        if (isMoving)
        {
            lastVerticalDirection = _upDownInputValue > 0 ? "up" : _upDownInputValue < 0 ? "down" : lastVerticalDirection;
            lastHorizontalDirection = _leftRightInputValue > 0 ? "right" : _leftRightInputValue < 0 ? "left" : lastHorizontalDirection;

            if (lastHorizontalDirection == "right")
            {
                _youngOmarGFXAnimator.Play("YoungOmarRunRight", -1, 0f);
            }
            else if (lastHorizontalDirection == "left")
            {
                _youngOmarGFXAnimator.Play("YoungOmarRunLeft", -1, 0f);
            }
            else if (lastVerticalDirection == "up")
            {
                _youngOmarGFXAnimator.Play("YoungOmarRunForward", -1, 0f);
            }
            else if (lastVerticalDirection == "down")
            {
                _youngOmarGFXAnimator.Play("YoungOmarRunDown", -1, 0f);
            }
        }
        else
        {
            if (lastVerticalDirection == "up")
            {
                _youngOmarGFXAnimator.Play("YoungOmarIdleForward", -1, 0f);
            }
            else if (lastVerticalDirection == "down")
            {
                _youngOmarGFXAnimator.Play("YoungOmarIdleDown", -1, 0f);
            }
            else if (lastHorizontalDirection == "right")
            {
                _youngOmarGFXAnimator.Play("YoungOmarIdleRight", -1, 0f);
            }
            else if (lastHorizontalDirection == "left")
            {
                _youngOmarGFXAnimator.Play("YoungOmarIdleLeft", -1, 0f);
            }
        }

        _youngOmarObject.transform.position = moveVector;
    }

    public void UpdateScriptSpace()
    {
        Vector3 moveVector = _shipObject.transform.position;

        moveVector.x += _leftRightInputValue * _SpaceShipSpeed * Time.deltaTime;
        moveVector.y += _upDownInputValue * _SpaceShipSpeed * Time.deltaTime;

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
    public void EnableActionMap(string mapName)
    {
        var actionMap = inputActionAsset.FindActionMap(mapName, true);
        actionMap.Enable();
    }

    public void DisableActionMap(string mapName)
    {
        var actionMap = inputActionAsset.FindActionMap(mapName, true);
        actionMap.Disable();
    }

    public void YoungOmarUserLeftRightMovementInput(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        if (context.performed)
        {
            if (value > 0)
            {
                _leftRightInputValue = 1;
                Debug.Log("movement");
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

    public void YoungOmarUserUpDownMovement(InputAction.CallbackContext context)
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

    public void YoungOmarReadUserOpenVaultInput(InputAction.CallbackContext context)
    {

    }

    public void SpaceReadUserLeftRightMovementInput(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        if (context.performed)
        {
            if (value > 0)
            {
                _leftRightInputValue = 1;
                Debug.Log("movement");
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

    public void SpaceReadUserUpDownMovement(InputAction.CallbackContext context)
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

    public void SpaceReadUserShootInput(InputAction.CallbackContext context)
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
