using UnityEngine;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private UiController uiController;

    private LevelKeeper _levelKeeper;
    private ShipController _shipController;
    private InputController _inputController;
    private StateController _stateController;
    private AsteroidsController _asteroidsController;

    private void Awake()
    {
        _shipController = new ShipController();
        _asteroidsController = gameObject.AddComponent<AsteroidsController>();
        _levelKeeper = gameObject.AddComponent<LevelKeeper>();
        _inputController = gameObject.AddComponent<InputController>();
        _stateController = gameObject.AddComponent<StateController>();

        _shipController.OnShipDie += SetGameOverData;
        _shipController.OnShipDie += _stateController.GameOver;
        _shipController.OnShipDie += _levelKeeper.RestartLevel;
        _shipController.OnJumpCountChange += uiController.SetJumpCount;

        _levelKeeper.OnLevelEnd += SetLevelData;
        _levelKeeper.OnLevelEnd += _stateController.StartGame;
        _levelKeeper.OnChangeScore += uiController.SetScoreCount;

        uiController.OnStartButton += SetLevelData;
        uiController.OnStartButton += _stateController.StartGame;

        _asteroidsController.OnSetPoints += _levelKeeper.AddScore;
        _asteroidsController.OnRequestChildAsteroid += GetChildAsteroid;

        _inputController.OnJumpPressed += _shipController.Jump;

        _stateController.OnChangeState += StartGame;
        _stateController.OnChangeState += uiController.ChangeWindowState;
    }

    private void Start()
    {
        spawner.Init();
        _levelKeeper.Init();
        _stateController.ChangeState(State.MainMenu);
        _shipController.Init(spawner.GetShip(ShipBody.Standard, ShipTower.Standard));
    }

    private void Update()
    {
        _shipController.UpdateBehavior();

        if (_stateController.CurrentState == State.Game)
        {
            _inputController.UpdateBehaviour();
            _asteroidsController.UpdateBehaviour();
        }
    }

    private void OnDestroy()
    {
        _shipController.OnShipDie -= SetGameOverData;
        _shipController.OnShipDie -= _stateController.GameOver;
        _shipController.OnJumpCountChange -= uiController.SetJumpCount;

        _levelKeeper.OnLevelEnd -= SetLevelData;
        _levelKeeper.OnLevelEnd -= _stateController.StartGame;
        _levelKeeper.OnChangeScore -= uiController.SetScoreCount;

        uiController.OnStartButton -= SetLevelData;
        uiController.OnStartButton -= _stateController.StartGame;

        _asteroidsController.OnSetPoints -= _levelKeeper.AddScore;
        _asteroidsController.OnRequestChildAsteroid -= GetChildAsteroid;

        _inputController.OnJumpPressed -= _shipController.Jump;

        _stateController.OnChangeState -= StartGame;
        _stateController.OnChangeState -= uiController.ChangeWindowState;
    }

    private void GetChildAsteroid(int asteroidsCount)
    {
        for (int i = 0; i < asteroidsCount; i++)
        {
            var poolObject = (PoolObject) Random.Range(0, (int) PoolObject.AsteroidC);
            _asteroidsController.SetNewAsteroid(spawner.GetAsteroid(poolObject));
        }
    }

    private void SetLevelData()
    {
        uiController.SetLevelInfo(
            _levelKeeper.GetCurrentLevel,
            _levelKeeper.GetAsteroidsOnLevel,
            _levelKeeper.GetChildrenOnLevel);
    }

    private void SetGameOverData()
    {
        uiController.SetGameOverData(
            _levelKeeper.GetCurrentLevel,
            _levelKeeper.GetCurrentScore);
    }

    private void StartGame(State state)
    {
        if (state == State.Game)
        {
            _shipController.ResetPosition();
            _asteroidsController.Init(
                spawner.GetStartAsteroidsPack(
                    _levelKeeper.GetAsteroidsOnLevel,
                    _levelKeeper.GetChildrenOnLevel));
        }
    }
}