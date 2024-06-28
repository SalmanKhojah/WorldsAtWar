using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainManger : MonoBehaviour, IDataPersistence
{
    private float _SpaceShipSpeed = 5;
    private float _YoungOmarSpeed = 4.5f;
    private float _normalShotSpeed = 13.0f;

    //private PlayerDeathController _playerDeathController;

    private int _score;

    private SpaceShipObject _shipObject;
    private YoungOmarObject _youngOmarObject;

    public Transform ShipTransform => _shipObject.transform;
    public Transform YoungOmarTransform => _youngOmarObject.transform;
    
    public bool OldManDialogue => _hasTalkedToOldMan;
    public bool InHatch => _inHatch;

    private float _leftRightInputValue;
    private float _upDownInputValue;

    private int _currentHealth = 0;
    private int _maxHealth = 1;
    private Animator _youngOmarGFXAnimator;
    public InputActionAsset inputActionAsset;
    private string _youngOmarDirection = "right"; //Defaults at right

    private Vector2 _lastInputDirection = Vector2.zero;
    private bool _hasTalkedToOldMan = false;
    private bool _inHatch = false;
    public CinemachineVirtualCamera cameraSpace;
    public CinemachineVirtualCamera cameraYoungOmar;



    public void InitializeYoungOmar()
    {
        cameraYoungOmar.Priority = 10;
        cameraSpace.Priority = 5;
        EnableActionMap("YoungOmar");
        _leftRightInputValue = 0;
        _upDownInputValue = 0;
        _youngOmarObject = transform.GetChild(1).GetComponent<YoungOmarObject>();
        _youngOmarGFXAnimator = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Animator>();


        EventSystemReference.Instance.SendScoreToPlayerEventHandler.AddListener(UpdatePlayerScore);


        SetYoungOmarActiveState(true);
    }

    public void InitializeSpace()
    {
        cameraYoungOmar.Priority = 5;
        cameraSpace.Priority = 10;
        DisableActionMap("YoungOmar");
        EnableActionMap("SpaceShip");
        _leftRightInputValue = 0;
        _upDownInputValue = 0;
        //_score = 0;
        _shipObject = transform.GetChild(0).GetComponent<SpaceShipObject>();

        _shipObject.transform.position = Vector3.zero;

        EventSystemReference.Instance.SendScoreToPlayerEventHandler.AddListener(UpdatePlayerScore);

        // _playerDeathController = GetComponent<PlayerDeathController>();
        // _playerDeathController.Initialize();

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
    private void TalkOldMan()
    {

        Vector2 inputDirection = new Vector2(_leftRightInputValue, _upDownInputValue).normalized;

        if (inputDirection != Vector2.zero)
        {
            _lastInputDirection = inputDirection;
        }



        Vector3 rayStartPosition = _youngOmarObject.transform.position + new Vector3(0, -1.6f, 0);
        Debug.DrawRay(rayStartPosition, _lastInputDirection * 3f, Color.red, 0.01f);
        RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, _lastInputDirection, 3f, LayerMask.GetMask("Oldman"));
        if (hit.collider != null && !_hasTalkedToOldMan)
        {


            _hasTalkedToOldMan = true;
            Debug.Log("Talked to");
        }

    }
    private void CheckIfInHatch()
    {
     
        Vector3 moveVector = _youngOmarObject.transform.position;
        float leftBound = 5124.46f;
        float rightBound = 5126.5f;
        float upperBound = -0.7f;
        float lowerBound = -1.7f;


        if (moveVector.x > leftBound && moveVector.x < rightBound && moveVector.y < upperBound && moveVector.y > lowerBound)
        {
            
            _inHatch = true;
        }
        else
        {

            _inHatch = false;
        }

    }
    public void UpdateScriptYoungOmar()
    {

        MoveYoungOmar();

        TalkOldMan();

        CheckIfInHatch();
    }

    private void MoveYoungOmar()
    {
        Vector3 moveVector = _youngOmarObject.transform.position;

        moveVector.x += _leftRightInputValue * _YoungOmarSpeed * Time.deltaTime;
        moveVector.y += _upDownInputValue * _YoungOmarSpeed * Time.deltaTime;
        bool isMoving = _leftRightInputValue != 0 || _upDownInputValue != 0;
        if (isMoving)
        {
            _youngOmarDirection = _leftRightInputValue > 0 ? "right" : _leftRightInputValue < 0 ? "left" : _upDownInputValue > 0 ? "up" : _upDownInputValue < 0 ? "down" : _youngOmarDirection;

            PlayAnimationIfNeeded(_youngOmarDirection);
        }
        else
        {

            HandleIdleAnimation();
        }

        float leftBound = 5056f;
        float rightBound = 5058.259f;
        float upperBound = 0.39f;
        float lowerBound = 0.09f;

        if (moveVector.x > leftBound && moveVector.x < rightBound && moveVector.y < upperBound && moveVector.y > lowerBound)
        {

            float distToLeft = Mathf.Abs(moveVector.x - leftBound);
            float distToRight = Mathf.Abs(moveVector.x - rightBound);
            float distToUpper = Mathf.Abs(moveVector.y - upperBound);
            float distToLower = Mathf.Abs(moveVector.y - lowerBound);


            float minDist = Mathf.Min(distToLeft, distToRight, distToUpper, distToLower);


            if (minDist == distToLeft)
            {
                moveVector.x = leftBound;
                Debug.Log("Moved to left boundary.");
            }
            else if (minDist == distToRight)
            {
                moveVector.x = rightBound;
                Debug.Log("Moved to right boundary.");
            }
            else if (minDist == distToUpper)
            {
                moveVector.y = upperBound;
                Debug.Log("Moved to upper boundary.");
            }
            else if (minDist == distToLower)
            {
                moveVector.y = lowerBound;
                Debug.Log("Moved to lower boundary.");
            }
        }

                if (moveVector.x > 5170.6)
        {
            moveVector.x = 5170.6f;
        }
        if (moveVector.x < 4975.5)
        {
            moveVector.x = 4975.5f;
        }

        if (moveVector.y > 18)
        {
            moveVector.y = 18;
        }
        if (moveVector.y < -13.6)
        {
            moveVector.y = -13.6f;
        }

        _youngOmarObject.transform.position = moveVector;
    }
    private void PlayAnimationIfNeeded(string _youngOmarDirection)
    {
        string currentAnimation = _youngOmarGFXAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (_youngOmarDirection == "right" && currentAnimation != "YoungOmarRunRight")
        {
            _youngOmarGFXAnimator.Play("YoungOmarRunRight", 0, 0f);
        }
        else if (_youngOmarDirection == "left" && currentAnimation != "YoungOmarRunLeft")
        {
            _youngOmarGFXAnimator.Play("YoungOmarRunLeft", 0, 0f);
        }
        else if (_youngOmarDirection == "up" && currentAnimation != "YoungOmarRunUp")
        {
            _youngOmarGFXAnimator.Play("YoungOmarRunUp", 0, 0f);
        }
        else if (_youngOmarDirection == "down" && currentAnimation != "YoungOmarRunDown")
        {
            _youngOmarGFXAnimator.Play("YoungOmarRunDown", 0, 0f);
        }
    }

    private void HandleIdleAnimation()
    {
        string currentAnimation = _youngOmarGFXAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (_youngOmarDirection == "right" && currentAnimation != "YoungOmarIdleRight")
        {
            _youngOmarGFXAnimator.Play("YoungOmarIdleRight", 0, 0f);
        }
        else if (_youngOmarDirection == "left" && currentAnimation != "YoungOmarIdleLeft")
        {
            _youngOmarGFXAnimator.Play("YoungOmarIdleLeft", 0, 0f);
        }
        else if (_youngOmarDirection == "up" && currentAnimation != "YoungOmarIdleForward")
        {
            _youngOmarGFXAnimator.Play("YoungOmarIdleForward", 0, 0f);
        }
        else if (_youngOmarDirection == "down" && currentAnimation != "YoungOmarIdleDown")
        {
            _youngOmarGFXAnimator.Play("YoungOmarIdleDown", 0, 0f);
        }
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

        // _shipObject.transform.position = _playerDeathController.UpdateScript();

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
            // EventSystemReference.Instance.GameManagerStartPlayerDeathSequenceHandler.Invoke();
            // _playerDeathController.StartMovingToCenterLerpProcess(_shipObject.transform.position);
        }
    }


}
