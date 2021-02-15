using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private UiController uiController;

    private LevelKeeper _levelKeeper;
    private ShipController _shipController;
    private InputController _inputController;
    private StateController _stateController;
    private AnalyticsController _analyticsController;
    private AsteroidsController _asteroidsController;

    private void Awake()
    {
        _shipController = new ShipController();
        _inputController = new InputController();
        _analyticsController = new AnalyticsController();
        _levelKeeper = gameObject.AddComponent<LevelKeeper>();
        _stateController = gameObject.AddComponent<StateController>();
        _asteroidsController = gameObject.AddComponent<AsteroidsController>();
        
        _analyticsController.OnLaunchGame();
        _levelKeeper.UpdateDataFromServer();

        _shipController.OnShipDie += SetGameOverData;
        _shipController.OnShipDie += _stateController.GameOver;
        _shipController.OnShipDie += _levelKeeper.RestartLevel;
        _shipController.OnJumpCountChange += uiController.SetJumpCount;

        _levelKeeper.OnLevelEnd += SetLevelData;
        _levelKeeper.OnLevelEnd += _stateController.StartGame;
        _levelKeeper.OnChangeScore += uiController.SetScoreCount;

        uiController.OnStartButton += SetLevelData;
        uiController.OnStartButton += _stateController.StartGame;
        uiController.OnStartButton += _analyticsController.OnPlayerStartGame;

        _asteroidsController.OnGenerationAsteroids += _levelKeeper.AddScore;

        _inputController.OnJumpPressed += _shipController.Jump;
        _inputController.OnFirePressed += Shoot;

        _stateController.OnChangeState += StartGame;
        _stateController.OnChangeState += uiController.ChangeWindowState;
        _stateController.OnChangeState += _shipController.ActivateImmune;
        _stateController.OnChangeState += _asteroidsController.GameOver;
    }

    private void Start()
    {
        spawner.Init();
        _levelKeeper.Init();
        _asteroidsController.Init();
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
        _shipController.OnShipDie -= _levelKeeper.RestartLevel;
        _shipController.OnJumpCountChange -= uiController.SetJumpCount;

        _levelKeeper.OnLevelEnd -= SetLevelData;
        _levelKeeper.OnLevelEnd -= _stateController.StartGame;
        _levelKeeper.OnChangeScore -= uiController.SetScoreCount;

        uiController.OnStartButton -= SetLevelData;
        uiController.OnStartButton -= _stateController.StartGame;
        uiController.OnStartButton -= _analyticsController.OnPlayerStartGame;

        _asteroidsController.OnGenerationAsteroids -= _levelKeeper.AddScore;

        _inputController.OnJumpPressed -= _shipController.Jump;
        _inputController.OnFirePressed -= Shoot;

        _stateController.OnChangeState -= StartGame;
        _stateController.OnChangeState -= uiController.ChangeWindowState;
        _stateController.OnChangeState -= _shipController.ActivateImmune;
        
        _analyticsController.OnQuitGame();
    }

    private void SetLevelData()
    {
        uiController.SetLevelInfo(
            _levelKeeper.GetCurrentLevel,
            _levelKeeper.GetAsteroidsOnLevel,
            _levelKeeper.GetChildrenOnLevel);
        
        _analyticsController.OnPlayerChangedLevel(_levelKeeper.GetCurrentLevel);
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
            _asteroidsController.StartGame(
                spawner.InitAndGetListAsteroids(
                    _levelKeeper.GetAsteroidsOnLevel,
                    _levelKeeper.GetChildrenOnLevel));
        }
    }

    private void Shoot()
    {
        _shipController.SetBulletPosition(spawner.GetBullet());
    }
}