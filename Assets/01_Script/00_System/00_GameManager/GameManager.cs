using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private enum PlayerDeathAnimationStep
    {
        start = 0,
        AtCenter = 1,
        bigExplosition = 2,
        RestingGame = 3,

    }

    public static GameManager instance;
    private string _stageName = "Stage Zero";

    private GMBaseState _currentState;

    private LoadDataState _loadState = new LoadDataState();
    private InitState _initSate = new InitState();
    private GameLoopState _gameLoopState = new GameLoopState();
    private PlayerDeathSequence _playerDeathSequenceState = new PlayerDeathSequence();

    private Level1RobotBattle _level1RobotBattle = new Level1RobotBattle();

    private ResourcesLoader _resourcesLoader;
    private PlayerMainManger _playerManager;
    private EnemeyMainManger _enemyMainManger;
    private ESMainManager _esMainManager;
    private BulletMainManger _bulletMainManger;
    private GameUiMainManager _gameUiMainManager;
    private ExplosionManager _explosionManager;
    private FadeinManager _fadeinManager;
    private float _currentWaitBeforeResettingTime = 0;
    private float _maxWaitBeforeResettingTime = 3.0f;

    private EventSystemReference _eventSystem;

    private PlayerDeathAnimationStep _step;

    public void SwitchState(GMBaseState nextState)
    {
        _currentState.OnExitState(this);
        _currentState = nextState;
        _currentState.OnEnterState(this);
    }

    public void DataLoadStateOnEnterState()
    {
        Debug.Log("this is on Enter for Load Data State");

        _eventSystem = GetComponent<EventSystemReference>();
        _resourcesLoader = GetComponent<ResourcesLoader>();
        

        _eventSystem.Initialize();

        _resourcesLoader.Initialize();

        _resourcesLoader.LoadSprite();

        EventSystemReference.Instance.GameManagerStartPlayerDeathSequenceHandler.AddListener(SwitchSatateToPlayerDeathSequenceState);

        SwitchState(_initSate); 

    }

    public void DataLoadStateOnUpdateState()
    {
        Debug.Log("this is on Update for Load Data State");
    }

    public void DataLoadStateOnExitState()
    {
        Debug.Log("this is on Exit for Load Data State");
    }

    public void InitStateOnEnterState()
    {
        Debug.Log("this is on Enter for init State");
       
        _playerManager = FindAnyObjectByType<PlayerMainManger>();
        _enemyMainManger = FindAnyObjectByType<EnemeyMainManger>();
        _bulletMainManger = FindAnyObjectByType<BulletMainManger>();
        _gameUiMainManager = FindAnyObjectByType<GameUiMainManager>();
        _explosionManager = FindAnyObjectByType<ExplosionManager>();
        _fadeinManager = FindAnyObjectByType<FadeinManager>();
        _esMainManager = FindAnyObjectByType<ESMainManager>();
        

        _playerManager.Initialize();
        _enemyMainManger.Initialize();
        _bulletMainManger.Initialize();
        _gameUiMainManager.Initialize();
        _explosionManager.Initialize();
        _fadeinManager.Initialize();
        _esMainManager.Initialize();

        SwitchState(_gameLoopState);
    }

    public void InitStateOnUpdateState()
    {
        Debug.Log("this is on Update for init State");
    }

    public void InitStateOnExitState()
    {
        Debug.Log("this is on Exit for init State");
    }

    public void GameLoopOnEnterState()
    {   
        Debug.Log("this is on ENTER for GAMELOOP State");

        //_enemyMainManger.BeginWave(1, 10); 
        //_esMainManager.BeginWave(2, 15);
        //BEGTUB //BEGTUB
        _playerManager.SetScore();

       // DataPersistenceManager.Instance.LoadGame();
    }

    public void OnGameLoopOnUpdateState()
    {
        Debug.Log("this is on ENTER for GAMEUPDATE State");
        _playerManager.UpdateScript();
        //_enemyMainManger.UpdateScript();
        _esMainManager.UpdateScript();
        _bulletMainManger.UpdateScript();
        _explosionManager.UpdateScript();

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            _esMainManager.PutAllEnemiesToSleep();
            _explosionManager.PutExpolsionToSleep();
            SwitchState(_initSate);
        }
    }

    public void OnGameLoopExitState()
    {

    }

    public void OnEnterPlayerDeathSequenceState()
    {
        //_enemyMainManger.PutAllEnemiesToSleep();
        _esMainManager.PutAllEnemiesToSleep();
        _explosionManager.PutExpolsionToSleep();
        _step = PlayerDeathAnimationStep.start;
        


    }

    public void OnUpdatePlayerDeathSequenceState()
    {

        switch (_step)
        {
            case PlayerDeathAnimationStep.start:
                {
                    _fadeinManager.UpdateScript();
                    if (_playerManager.UpdateScriptPlayerDeath())
                    {
                        _explosionManager.SpwanExposionBodyForPlayerDeath(15);
                        _step++;
                    }  
                }
                break;
            case PlayerDeathAnimationStep.AtCenter:
                {
                    _playerManager.UpdateScriptPlayerDeath();
                    _fadeinManager.UpdateScript();

                    if (_explosionManager.UpdateExpolsitionBodySequence())
                    {
                        _explosionManager.SpwanBigExpostionRequest();
                        _playerManager.SetPlayerShipActiveState(false);
                        _step++;
                    }
                    
                }
                break;

            case PlayerDeathAnimationStep.bigExplosition:
                {
                    if(_explosionManager.UpdateBigExpolsitionBody())
                    {
                        _currentWaitBeforeResettingTime = 0;
                        _gameUiMainManager.SetGameOverObjectActiveState(true);
                        _step++;
                    }
                }
                break;

            case PlayerDeathAnimationStep.RestingGame:
                {
                    if (_currentWaitBeforeResettingTime == _maxWaitBeforeResettingTime)
                    {
                        SwitchState(_initSate);
                    }
                    else
                    {
                        _currentWaitBeforeResettingTime += Time.deltaTime;

                        if (_currentWaitBeforeResettingTime > _maxWaitBeforeResettingTime)
                        {
                            _currentWaitBeforeResettingTime = _maxWaitBeforeResettingTime;
                        }
                    }
                }
                break;

                  
        }
    }

    public void OnExitPlayerDeathSequenceState()
    {

    }

    public void OnEnterLevel1RobotBattle()
    {
        _enemyMainManger.BeginWave(1, 10);
        _playerManager.SetScore();
       // DataPersistenceManager.Instance.LoadGame();
    }

    public void OnUpdateLevel1RobotBattle()
    {
        _playerManager.UpdateScript();
        _enemyMainManger.UpdateScript();
        _bulletMainManger.UpdateScript();
        _explosionManager.UpdateScript();

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SwitchState(_initSate);
        }
    }

    public void OnExitLevel1RobotBattle()
    {

    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _currentState = _loadState;
        _currentState.OnEnterState(this);
    }

    public void Start()
    {
        
    }


    public void Update()
    {
        _currentState.OnUpdateState(this);
    }

    public void OnApplicationQuit()
    {
        //DataPersistenceManager.Instance.SaveGame();
    }

    private void SwitchSatateToPlayerDeathSequenceState()
    {
        SwitchState(_playerDeathSequenceState);
    }
}

