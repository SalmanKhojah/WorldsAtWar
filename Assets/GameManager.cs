using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public string _stageName = "Stage Zero";

    public GMBaseState _currentState;
    public InitState _initState = new InitState();
    public LoadDataState _loadState = new LoadDataState();

    public PlayerMainManager _playerMainManager;

    public EnemyMainManager _enemyMainManager;
    public BulletMainManager _bulletMainManager;
    public GameUiMainManager _gameUiMainManager;

    public EventSystemRef _eventSystemRef;

    public void SwitchState(GMBaseState nextState)
    {
        _currentState.OnExitState(this);
        _currentState = nextState;
        _currentState.OnEnterState(this);
    }

    public void InitStateOnEnterState()
    {
        Debug.Log("this is on Enter for init State");
        SwitchState(_loadState);
    }
    public void InitStateOnUpdateState()
    {
        Debug.Log("this is on Update for init State");
    }
    public void InitStateOnExitState()
    {
        Debug.Log("this is on Exit for init State");
    }
    public void DataLoadStateOnEnterState()
    {
        Debug.Log("this is on Enter for Data Load State");
    }
    public void DataLoadStateOnUpdateState()
    {
        Debug.Log("this is on Update for Data Load State");
        _playerMainManager.UpdateScript();
        _enemyMainManager.UpdateScript();
        _bulletMainManager.UpdateScript();
    }
    public void DataLoadStateOnExitState()
    {
        Debug.Log("this is on Exit for Data Load State");
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

        _currentState = _initState;
        _initState.OnEnterState(this);
    }

    private void Start()
    {
        _playerMainManager = GameObject.FindAnyObjectByType<PlayerMainManager>();
        _playerMainManager.Init();
        _enemyMainManager = GameObject.FindAnyObjectByType<EnemyMainManager>();
        _enemyMainManager.Init();
        _bulletMainManager = GameObject.FindAnyObjectByType<BulletMainManager>();
        _bulletMainManager.Init();
        _eventSystemRef = GameObject.FindAnyObjectByType<EventSystemRef>();
        _eventSystemRef.Init();
        _gameUiMainManager = GameObject.FindAnyObjectByType<GameUiMainManager>();
        _gameUiMainManager.Init();
    }

    public void Update()
    {
       _currentState.OnUpdateState(this);

       if (Input.GetKeyDown(KeyCode.Space))
       {
            SwitchState(_loadState);
       }
    }
}
