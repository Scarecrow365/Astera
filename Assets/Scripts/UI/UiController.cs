using System;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private MainMenuWindow mainMenuWindow;
    [SerializeField] private MainDataWindow mainDataWindow;
    [SerializeField] private GameOverWindow gameOverWindow;
    [SerializeField] private LevelInfoWindow levelInfoWindow;

    public event Action OnStartButton;
    public event Action OnDeleteSaveButton;

    private void Awake()
    {
        mainMenuWindow.OnStartButton += () => OnStartButton?.Invoke();
        mainMenuWindow.OnDeleteSaveButton += () => OnDeleteSaveButton?.Invoke();
    }

    public void SetJumpCount(int count) => mainDataWindow.SetJumpText(count);
    public void SetScoreCount(int count) => mainDataWindow.SetScoreText(count);
    public void SetGameOverData(int level, int score) => gameOverWindow.SetStatistic(level, score);

    public void SetLevelInfo(int level, int asteroidsCount, int childrenCount)
    {
        levelInfoWindow.SetTextLevel(level);
        levelInfoWindow.SetDataLevelText(asteroidsCount, childrenCount);
    }

    public void ChangeWindowState(State state)
    {
        if (mainMenuWindow != null)
        {
            mainMenuWindow.ChangeState(state);
        }

        if (levelInfoWindow != null)
        {
            levelInfoWindow.ChangeState(state);
        }

        if (mainDataWindow != null)
        {
            mainDataWindow.ChangeState(state);
        }

        if (gameOverWindow != null)
        {
            gameOverWindow.ChangeState(state);
        }
    }
}